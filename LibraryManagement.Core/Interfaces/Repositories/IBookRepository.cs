using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<Book> GetByISBNAsync(string isbn);
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<bool> DeleteAsync(Book book);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Book>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Book>> GetByAuthorAsync(string author);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<bool> IsBookAvailableAsync(int bookId);
        Task<IEnumerable<Book>> GetBorrowedBooksAsync(int userId);
        Task<IEnumerable<Book>> GetOverdueBooksAsync();
        Task<bool> ExistsByTitleAsync(string title);
        Task<bool> ExistsByISBNAsync(string isbn);
        Task UpdateQuantityAsync(int id, int quantityChange);
    }
}