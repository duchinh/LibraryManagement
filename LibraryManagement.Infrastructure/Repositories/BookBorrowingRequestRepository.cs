using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookBorrowingRequestRepository(LibraryDbContext context) : IBookBorrowingRequestRepository
    {
        private readonly LibraryDbContext _context = context;

        public async Task<List<BookBorrowingRequest>> GetAllRequestsAsync()
        {
            return await _context.BookBorrowingRequests
                .Include(r => r.User)
                .Include(r => r.BookBorrowingRequestDetails)
                .ThenInclude(d => d.Book)
                .ToListAsync();
        }

        public async Task<BookBorrowingRequest?> GetRequestByIdAsync(Guid requestId)
        {
            return await _context.BookBorrowingRequests
                .Include(r => r.BookBorrowingRequestDetails)
                .FirstOrDefaultAsync(r => r.Id == requestId);
        }

        public async Task<int> GetMonthlyRequestCountAsync(Guid userId, DateTime monthStart, DateTime monthEnd)
        {
            return await _context.BookBorrowingRequests
                .Where(r => r.UserId == userId
                    && r.RequestDate >= monthStart
                    && r.RequestDate <= monthEnd
                    && r.Status == RequestStatus.Approved || r.Status == RequestStatus.Waiting)
                .CountAsync();
        }

        public async Task<List<BookBorrowingRequest>> GetAllRequestsForUserAsync(Guid userId)
        {
            return await _context.BookBorrowingRequests
                .Where(r => r.UserId == userId)
                .Include(r => r.BookBorrowingRequestDetails)
                .ThenInclude(d => d.Book)
                .ToListAsync();
        }

        public async Task<BookBorrowingRequest> CreateAsync(BookBorrowingRequest request)
        {
            await _context.BookBorrowingRequests.AddAsync(request);
            return request;
        }

        public async Task<List<Book>> GetBooksByIdsAsync(List<Guid> bookIds)
        {
            return await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}