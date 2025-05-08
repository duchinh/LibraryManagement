using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO> GetBookByIdAsync(Guid id);
        Task<BookDTO> CreateBookAsync(CreateBookDTO bookDto);
        Task<BookDTO> UpdateBookAsync(Guid id, UpdateBookDTO bookDto);
        Task<bool> DeleteBookAsync(Guid id);

    }
}