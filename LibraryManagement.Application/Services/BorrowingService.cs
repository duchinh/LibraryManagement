using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Application.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBookBorrowingRequestRepository _requestRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public BorrowingService(
            IBookBorrowingRequestRepository requestRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _requestRepository = requestRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<BookBorrowingRequest> CreateBorrowingRequestAsync(int userId, List<int> bookIds)
        {
            if (bookIds.Count > 5)
            {
                throw new InvalidOperationException("Không được mượn quá 5 cuốn sách cùng lúc");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Không tìm thấy người dùng");
            }

            var monthlyBorrowingCount = await GetUserBorrowingCountThisMonthAsync(userId);
            if (monthlyBorrowingCount >= 3)
            {
                throw new InvalidOperationException("Bạn đã mượn 3 lần trong tháng này");
            }

            var currentBorrowings = await _requestRepository.GetByUserIdAsync(userId);
            var overdueBooks = currentBorrowings
                .Where(r => r.Status == BorrowingRequestStatus.Approved)
                .SelectMany(r => r.Details)
                .Where(d => d.Status == BorrowingDetailStatus.Borrowed && d.DueDate < DateTime.UtcNow)
                .ToList();

            if (overdueBooks.Any())
            {
                throw new InvalidOperationException("Bạn có sách đã quá hạn, vui lòng trả sách trước khi mượn sách mới");
            }

            var books = new List<Book>();
            foreach (var bookId in bookIds)
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null)
                {
                    throw new ArgumentException($"Không tìm thấy sách với ID {bookId}");
                }
                if (book.Status != BookStatus.Available)
                {
                    throw new InvalidOperationException($"Sách {book.Title} không có sẵn");
                }
                if (book.AvailableQuantity <= 0)
                {
                    throw new InvalidOperationException($"Sách {book.Title} đã hết");
                }
                books.Add(book);
            }

            var request = new BookBorrowingRequest
            {
                RequestorId = userId,
                RequestDate = DateTime.UtcNow,
                Status = BorrowingRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var details = books.Select(book => new BookBorrowingRequestDetail
            {
                BookId = book.Id,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(14), // 2 tuần
                Status = BorrowingDetailStatus.Borrowed,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            request.Details = details;

            return await _requestRepository.AddAsync(request);
        }

        public async Task<BookBorrowingRequest> GetBorrowingRequestByIdAsync(int id)
        {
            return await _requestRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetUserBorrowingRequestsAsync(int userId)
        {
            return await _requestRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetAllBorrowingRequestsAsync()
        {
            return await _requestRepository.GetAllAsync();
        }

        public async Task ApproveBorrowingRequestAsync(int requestId, int approverId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new ArgumentException("Request not found");
            }

            if (request.Status != BorrowingRequestStatus.Pending)
            {
                throw new InvalidOperationException("Request is not in pending status");
            }

            var approver = await _userRepository.GetByIdAsync(approverId);
            if (approver == null || approver.Role != UserRole.SuperUser)
            {
                throw new UnauthorizedAccessException("Only super users can approve requests");
            }

            request.ApproverId = approverId;
            request.ApprovalDate = DateTime.UtcNow;
            request.Status = BorrowingRequestStatus.Approved;
            request.UpdatedAt = DateTime.UtcNow;

            foreach (var detail in request.Details)
            {
                var book = await _bookRepository.GetByIdAsync(detail.BookId);
                if (book != null)
                {
                    book.AvailableQuantity--;
                    if (book.AvailableQuantity == 0)
                    {
                        book.Status = BookStatus.Unavailable;
                    }
                    await _bookRepository.UpdateAsync(book);
                }
            }

            await _requestRepository.UpdateAsync(request);
        }

        public async Task RejectBorrowingRequestAsync(int requestId, int approverId, string reason)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new ArgumentException("Request not found");
            }

            if (request.Status != BorrowingRequestStatus.Pending)
            {
                throw new InvalidOperationException("Request is not in pending status");
            }

            var approver = await _userRepository.GetByIdAsync(approverId);
            if (approver == null || approver.Role != UserRole.SuperUser)
            {
                throw new UnauthorizedAccessException("Only super users can reject requests");
            }

            request.ApproverId = approverId;
            request.ApprovalDate = DateTime.UtcNow;
            request.Status = BorrowingRequestStatus.Rejected;
            request.RejectionReason = reason;
            request.UpdatedAt = DateTime.UtcNow;

            await _requestRepository.UpdateAsync(request);
        }

        public async Task ReturnBooksAsync(int requestId, List<int> bookIds)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new ArgumentException("Request not found");
            }

            if (request.Status != BorrowingRequestStatus.Approved)
            {
                throw new InvalidOperationException("Only approved requests can be returned");
            }

            foreach (var bookId in bookIds)
            {
                var detail = request.Details.FirstOrDefault(d => d.BookId == bookId);
                if (detail != null)
                {
                    detail.ReturnDate = DateTime.UtcNow;
                    detail.Status = BorrowingDetailStatus.Returned;
                    detail.UpdatedAt = DateTime.UtcNow;

                    var book = await _bookRepository.GetByIdAsync(bookId);
                    if (book != null)
                    {
                        book.AvailableQuantity++;
                        if (book.Status == BookStatus.Unavailable)
                        {
                            book.Status = BookStatus.Available;
                        }
                        await _bookRepository.UpdateAsync(book);
                    }
                }
            }

            await _requestRepository.UpdateAsync(request);
        }

        public async Task<bool> CanUserBorrowMoreBooksAsync(int userId)
        {
            var monthlyBorrowingCount = await GetUserBorrowingCountThisMonthAsync(userId);
            return monthlyBorrowingCount < 3;
        }

        public async Task<int> GetUserBorrowingCountThisMonthAsync(int userId)
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var requests = await _requestRepository.GetByUserIdAsync(userId);
            return requests.Count(r =>
                r.RequestDate >= startOfMonth &&
                r.RequestDate <= endOfMonth &&
                r.Status == BorrowingRequestStatus.Approved
            );
        }
    }
}