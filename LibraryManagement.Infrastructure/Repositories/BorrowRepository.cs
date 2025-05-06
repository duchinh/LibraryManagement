using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryDbContext _context;

        public BorrowRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Borrow>> GetAllAsync()
        {
            return await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<Borrow?> GetByIdAsync(int id)
        {
            return await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Borrow> AddAsync(Borrow borrow)
        {
            await _context.Borrows.AddAsync(borrow);
            await _context.SaveChangesAsync();
            return borrow;
        }

        public async Task UpdateAsync(Borrow borrow)
        {
            _context.Entry(borrow).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Borrow borrow)
        {
            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Borrows.AnyAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Borrow>> GetByUserIdAsync(int userId)
        {
            return await _context.Borrows
                .Where(b => b.UserId == userId)
                .Include(b => b.Book)
                .ToListAsync();
        }

        public async Task<IEnumerable<Borrow>> GetByBookIdAsync(int bookId)
        {
            return await _context.Borrows
                .Where(b => b.BookId == bookId)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync()
        {
            var borrows = await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.User)
                .ToListAsync();

            return borrows.Where(b => b.DueDate < DateTime.Now && !b.GetIsReturned());
        }

        public async Task<bool> IsBookBorrowedAsync(int bookId)
        {
            var borrows = await _context.Borrows
                .Where(b => b.BookId == bookId)
                .ToListAsync();

            return borrows.Any(b => !b.GetIsReturned());
        }

        public async Task<bool> HasUserBorrowedBookAsync(int userId, int bookId)
        {
            var borrows = await _context.Borrows
                .Where(b => b.UserId == userId && b.BookId == bookId)
                .ToListAsync();

            return borrows.Any(b => !b.GetIsReturned());
        }

        public async Task<int> GetUserBorrowCountAsync(int userId)
        {
            var borrows = await _context.Borrows
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return borrows.Count(b => !b.GetIsReturned());
        }

        public async Task<IEnumerable<Borrow>> GetBorrowsByDateRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Borrows.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(b => b.BorrowDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(b => b.BorrowDate <= endDate.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> IsBookAvailableAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            return book != null && book.Quantity > 0;
        }

        public async Task<bool> HasUserReachedBorrowLimitAsync(int userId)
        {
            var borrowCount = await _context.Borrows
                .CountAsync(b => b.UserId == userId && !b.GetIsReturned());
            return borrowCount >= 5; // Giới hạn mượn 5 cuốn sách
        }

        public async Task<bool> HasUserOverdueBooksAsync(int userId)
        {
            return await _context.Borrows
                .AnyAsync(b => b.UserId == userId && b.DueDate < DateTime.Now && !b.GetIsReturned());
        }

        public async Task<bool> IsBorrowOverdueAsync(int borrowId)
        {
            var borrow = await _context.Borrows.FindAsync(borrowId);
            return borrow != null && borrow.DueDate < DateTime.Now && !borrow.GetIsReturned();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}