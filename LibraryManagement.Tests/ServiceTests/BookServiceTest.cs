using AutoMapper;
using LibraryApp.Application.DTOs.Book;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Interfaces;
using Moq;

namespace LibraryApp.Tests.ServiceTests
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IBookRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private BookService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new BookService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsMappedBooks()
        {
            var books = new List<Book> { new(), new() };
            var bookDtos = new List<BookDto> { new(), new() };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(books)).Returns(bookDtos);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.EqualTo(bookDtos));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsBookDto_WhenBookExists()
        {
            var book = new Book { Id = Guid.NewGuid() };
            var bookDto = new BookDto { Id = book.Id };

            _repositoryMock.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(bookDto);

            var result = await _service.GetByIdAsync(book.Id);

            Assert.That(result, Is.EqualTo(bookDto));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            var bookId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);

            var result = await _service.GetByIdAsync(bookId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_CreatesAndReturnsBookDto()
        {
            var createDto = new CreateBookDto { TotalCopies = 3 };
            var book = new Book { TotalCopies = 3 };
            var bookDto = new BookDto();

            _mapperMock.Setup(m => m.Map<Book>(createDto)).Returns(book);
            _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(bookDto);

            var result = await _service.CreateAsync(createDto);

            _repositoryMock.Verify(r => r.AddAsync(book), Times.Once);
            Assert.Multiple(() =>
            {
                Assert.That(book.AvailableCopies, Is.EqualTo(3));
                Assert.That(result, Is.EqualTo(bookDto));
            });
        }

        [Test]
        public async Task UpdateAsync_ReturnsUpdatedBookDto_WhenBookExists()
        {
            var bookId = Guid.NewGuid();
            var dto = new UpdateBookDto();
            var book = new Book { Id = bookId };
            var updatedDto = new BookDto();

            _repositoryMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map(dto, book));
            _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(updatedDto);

            var result = await _service.UpdateAsync(bookId, dto);

            _repositoryMock.Verify(r => r.UpdateAsync(book), Times.Once);
            Assert.That(result, Is.EqualTo(updatedDto));
        }

        [Test]
        public async Task UpdateAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            var bookId = Guid.NewGuid();
            var dto = new UpdateBookDto();

            _repositoryMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);

            var result = await _service.UpdateAsync(bookId, dto);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_ReturnsTrue_WhenBookExists()
        {
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId };

            _repositoryMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);

            var result = await _service.DeleteAsync(bookId);

            _repositoryMock.Verify(r => r.DeleteAsync(book), Times.Once);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenBookDoesNotExist()
        {
            var bookId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);

            var result = await _service.DeleteAsync(bookId);

            Assert.That(result, Is.False);
        }
    }
}