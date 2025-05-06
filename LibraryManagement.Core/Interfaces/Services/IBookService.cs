using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(CreateBookDTO bookDto);
        Task<Book> UpdateBookAsync(int id, UpdateBookDTO bookDto);
        Task<bool> DeleteBookAsync(int id);
        Task<Book> UpdateBookStatusAsync(int id, BookStatus status);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
    }
}