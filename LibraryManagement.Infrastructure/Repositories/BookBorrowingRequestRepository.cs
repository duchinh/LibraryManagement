using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookBorrowingRequestRepository : IBookBorrowingRequestRepository
    {
        private readonly LibraryDbContext _context;

        public BookBorrowingRequestRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetAllAsync()
        {
            return await _context.BookBorrowingRequests
                .Include(r => r.Requestor)
                .Include(r => r.Approver)
                .Include(r => r.Details)
                    .ThenInclude(d => d.Book)
                .ToListAsync();
        }

        public async Task<BookBorrowingRequest> GetByIdAsync(int id)
        {
            return await _context.BookBorrowingRequests
                .Include(r => r.Requestor)
                .Include(r => r.Approver)
                .Include(r => r.Details)
                    .ThenInclude(d => d.Book)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<BookBorrowingRequest> AddAsync(BookBorrowingRequest request)
        {
            await _context.BookBorrowingRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<BookBorrowingRequest> UpdateAsync(BookBorrowingRequest request)
        {
            _context.BookBorrowingRequests.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetByUserIdAsync(int userId)
        {
            return await _context.BookBorrowingRequests
                .Include(r => r.Requestor)
                .Include(r => r.Approver)
                .Include(r => r.Details)
                    .ThenInclude(d => d.Book)
                .Where(r => r.RequestorId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetUserRequestsThisMonthAsync(int userId)
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _context.BookBorrowingRequests
                .Where(r => r.RequestorId == userId &&
                           r.RequestDate >= startOfMonth)
                .ToListAsync();
        }
    }
}