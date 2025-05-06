using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<Book> CreateBookAsync(CreateBookDTO bookDto)
        {
            // Kiểm tra ISBN trùng lặp
            var existingBook = await _bookRepository.GetByISBNAsync(bookDto.ISBN);
            if (existingBook != null)
            {
                throw new InvalidOperationException("ISBN đã tồn tại trong hệ thống");
            }

            // Kiểm tra danh mục
            var category = await _categoryRepository.GetByIdAsync(bookDto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("Danh mục không tồn tại");
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                ISBN = bookDto.ISBN,
                CategoryId = bookDto.CategoryId,
                Category = category,
                Publisher = bookDto.Publisher,
                PublicationYear = bookDto.PublicationYear,
                Quantity = bookDto.Quantity,
                AvailableQuantity = bookDto.Quantity,
                Description = bookDto.Description,
                Status = BookStatus.Available,
                CreatedAt = DateTime.UtcNow
            };

            return await _bookRepository.AddAsync(book);
        }

        public async Task<Book> UpdateBookAsync(int id, UpdateBookDTO bookDto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException("Không tìm thấy sách");
            }

            // Kiểm tra ISBN trùng lặp nếu ISBN thay đổi
            if (book.ISBN != bookDto.ISBN)
            {
                var existingBook = await _bookRepository.GetByISBNAsync(bookDto.ISBN);
                if (existingBook != null)
                {
                    throw new InvalidOperationException("ISBN đã tồn tại trong hệ thống");
                }
            }

            // Kiểm tra danh mục
            var category = await _categoryRepository.GetByIdAsync(bookDto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("Danh mục không tồn tại");
            }

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.ISBN = bookDto.ISBN;
            book.CategoryId = bookDto.CategoryId;
            book.Publisher = bookDto.Publisher;
            book.PublicationYear = bookDto.PublicationYear;
            book.Quantity = bookDto.Quantity;
            book.Description = bookDto.Description;
            book.UpdatedAt = DateTime.UtcNow;

            return await _bookRepository.UpdateAsync(book);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }

            return await _bookRepository.DeleteAsync(book);
        }

        public async Task<Book> UpdateBookStatusAsync(int id, BookStatus status)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }

            book.Status = status;
            book.UpdatedAt = DateTime.UtcNow;

            return await _bookRepository.UpdateAsync(book);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _bookRepository.GetByCategoryIdAsync(categoryId);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _bookRepository.SearchAsync(searchTerm);
        }
    }
}