using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Core.DTOs;
using LibraryManagement.Core.Enums;
using AutoMapper;

namespace LibraryManagement.Application.Services
{
    public class BorrowingRequestService : IBorrowRequestService
    {
        private readonly IBookBorrowingRequestRepository _requestRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BorrowingRequestService(
            IBookBorrowingRequestRepository requestRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _requestRepository = requestRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BorrowingRequestDTO> BorrowBooksAsync(CreateBorrowingRequestDTO dto)
        {
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddSeconds(-1);

            var monthlyRequestCount = await _requestRepository.GetMonthlyRequestCountAsync(dto.UserId, monthStart, monthEnd);

            if (monthlyRequestCount >= 3)
                throw new Exception("You have reached the monthly request limit (3 requests).");

            var books = await _bookRepository.GetBooksByIdsAsync(dto.BookIds);

            if (books.Count != dto.BookIds.Count)
                throw new Exception("One or more selected books do not exist.");

            if (books.Any(b => b.AvailableQuantity <= 0))
                throw new Exception("One or more books are not available.");

            var request = _mapper.Map<BookBorrowingRequest>(dto);

            request.Id = Guid.NewGuid();
            request.RequestDate = now;
            request.CreatedAt = now;

            request.BookBorrowingRequestDetails = books.Select(book => new BookBorrowingRequestDetail
            {
                Id = Guid.NewGuid(),
                BookId = book.Id,
                BorrowDate = now,
                DueDate = now.AddDays(14),
                CreatedAt = now
            }).ToList();

            foreach (var book in books)
            {
                book.AvailableQuantity--;
            }

            await _requestRepository.CreateAsync(request);
            await _bookRepository.UpdateBooksAsync(books);
            await _requestRepository.SaveChangesAsync();

            return _mapper.Map<BorrowingRequestDTO>(request);
        }

        public async Task<List<BorrowingRequestDTO>> GetAllRequestsForUserAsync(Guid userId)
        {
            var requests = await _requestRepository.GetAllRequestsForUserAsync(userId);

            return requests.Select(r => _mapper.Map<BorrowingRequestDTO>(r)).ToList();
        }

        public async Task<List<BorrowingRequestDTO>> GetAllRequestsAsync()
        {
            var requests = await _requestRepository.GetAllRequestsAsync();

            return requests.Select(r => _mapper.Map<BorrowingRequestDTO>(r)).ToList();
        }

        public async Task<BorrowingRequestDTO> UpdateRequestStatusAsync(Guid requestId, UpdateBorrowingRequestDTO dto)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId) ?? throw new Exception("Request not found.");

            _mapper.Map(dto, request);

            if (dto.Status == RequestStatus.Approved)
            {
                request.Note = "Approved by Admin";
                request.ApprovalDate = DateTime.UtcNow;
            }
            else if (dto.Status == RequestStatus.Rejected)
            {
                request.Note = "Rejected by Admin";
                request.ReturnedDate = DateTime.UtcNow;

                foreach (var detail in request.BookBorrowingRequestDetails)
                {
                    var book = await _bookRepository.GetBookByIdAsync(detail.BookId);
                    if (book != null)
                    {
                        book.AvailableQuantity += 1;
                    }
                }
            }

            await _requestRepository.SaveChangesAsync();

            return _mapper.Map<BorrowingRequestDTO>(request);
        }
    }
}