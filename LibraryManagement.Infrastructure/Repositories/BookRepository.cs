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
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => !b.IsDeleted)
                .ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<Book> AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteAsync(Book book)
        {
            book.IsDeleted = true;
            book.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<IEnumerable<Book>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => b.CategoryId == categoryId && !b.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchAsync(string searchTerm)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => !b.IsDeleted && (
                    b.Title.Contains(searchTerm) ||
                    b.Author.Contains(searchTerm) ||
                    b.ISBN.Contains(searchTerm)
                ))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => !b.IsDeleted && b.Status == BookStatus.Available)
                .ToListAsync();
        }

        public async Task<bool> IsBookAvailableAsync(int bookId)
        {
            var book = await _context.Books
                .Include(b => b.BorrowingDetails)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
                return false;

            var borrows = await _context.Borrows
                .Where(b => b.BookId == bookId)
                .ToListAsync();

            var activeLoans = borrows.Count(b => !b.GetIsReturned());

            return book.Quantity > activeLoans;
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

        public async Task<IEnumerable<Book>> GetBorrowedBooksAsync(int userId)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => !b.IsDeleted && b.Status == BookStatus.Borrowed)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _context.Books
                .Where(b => b.CategoryId == categoryId)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
        {
            return await _context.Books
                .Where(b => b.Author == author)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByPublisherAsync(string publisher)
        {
            return await _context.Books
                .Where(b => b.Author == publisher)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.Books.AnyAsync(b => b.Title == title && !b.IsDeleted);
        }

        public async Task<bool> ExistsByISBNAsync(string isbn)
        {
            return await _context.Books.AnyAsync(b => b.ISBN == isbn && !b.IsDeleted);
        }

        public async Task UpdateQuantityAsync(int id, int quantityChange)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null && !book.IsDeleted)
            {
                book.AvailableQuantity += quantityChange;
                if (book.AvailableQuantity <= 0)
                {
                    book.Status = BookStatus.Unavailable;
                }
                else if (book.Status == BookStatus.Unavailable)
                {
                    book.Status = BookStatus.Available;
                }
                book.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Book>> GetOverdueBooksAsync()
        {
            var borrows = await _context.Borrows
                .Include(b => b.Book)
                .Where(b => b.DueDate < DateTime.Now)
                .ToListAsync();

            var overdueBorrows = borrows
                .Where(b => !b.GetIsReturned())
                .Select(b => b.Book)
                .Distinct()
                .ToList();

            return overdueBorrows;
        }

        public async Task<int> GetAvailableCopiesAsync(int bookId)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
                return 0;

            var borrows = await _context.Borrows
                .Where(b => b.BookId == bookId)
                .ToListAsync();

            var activeLoans = borrows.Count(b => !b.GetIsReturned());

            return book.Quantity - activeLoans;
        }

        public async Task<Book> GetByISBNAsync(string isbn)
        {
            return await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted);
        }

        public async Task<IEnumerable<Book>> GetByAuthorAsync(string author)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => b.Author.Contains(author) && !b.IsDeleted)
                .ToListAsync();
        }
    }
}