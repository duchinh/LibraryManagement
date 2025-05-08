using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(Guid id);
        Task<List<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(Book book);
        Task<List<Book>> GetBooksByIdsAsync(List<Guid> bookIds);
        Task UpdateBooksAsync(List<Book> books);
    }
}