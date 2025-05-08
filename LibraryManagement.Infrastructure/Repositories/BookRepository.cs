using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(LibraryDbContext context, ILogger<BookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            return await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _context.Books
                    .Include(b => b.Category)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all books from database");
                throw;
            }
        }

        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetBooksByIdsAsync(List<Guid> ids)
        {
            return await _context.Books
                .Where(b => ids.Contains(b.Id))
                .ToListAsync();
        }

        public async Task UpdateBooksAsync(List<Book> books)
        {
            _context.Books.UpdateRange(books);
            await _context.SaveChangesAsync();
        }

    }
}