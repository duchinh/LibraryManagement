using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            try
            {
                _logger.LogInformation("Getting all books");
                var books = await _bookRepository.GetAllBooksAsync();
                if (books == null || !books.Any())
                {
                    _logger.LogWarning("No books found");
                    return new List<BookDTO>();
                }
                return _mapper.Map<IEnumerable<BookDTO>>(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all books");
                throw;
            }
        }

        public async Task<BookDTO> GetBookByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }

            return _mapper.Map<BookDTO>(book);
        }

        public async Task<BookDTO> CreateBookAsync(CreateBookDTO bookDto)
        {

            var book = _mapper.Map<Book>(bookDto);
            book.AvailableQuantity = book.Quantity;

            await _bookRepository.AddBookAsync(book);
            return _mapper.Map<BookDTO>(book);
        }

        public async Task<BookDTO> UpdateBookAsync(Guid id, UpdateBookDTO bookDto)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException("Không tìm thấy sách");
            }

            _mapper.Map(bookDto, book);
            book.UpdatedAt = DateTime.UtcNow;
            await _bookRepository.UpdateBookAsync(book);

            return _mapper.Map<BookDTO>(book);
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null) return false;

            await _bookRepository.DeleteBookAsync(book);

            return true;
        }
    }
}