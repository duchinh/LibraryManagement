using LibraryApp.API.Controllers;
using LibraryApp.Application.DTOs.Book;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApp.Tests.ControllerTests
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IBookService> _bookServiceMock;
        private BookController _controller;

        [SetUp]
        public void SetUp()
        {
            _bookServiceMock = new Mock<IBookService>();
            _controller = new BookController(_bookServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOk_WhenBooksExist()
        {
            var books = new List<BookDto>
            {
                new BookDto { Id = Guid.NewGuid(), Title = "Book 1", Author = "Author 1" },
                new BookDto { Id = Guid.NewGuid(), Title = "Book 2", Author = "Author 2" }
            };
            _bookServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(books);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(books));
            });
        }

        [Test]
        public async Task GetById_ReturnsOk_WhenBookExists()
        {
            var bookId = Guid.NewGuid();
            var book = new BookDto { Id = bookId, Title = "Test Book", Author = "Test Author" };
            _bookServiceMock.Setup(s => s.GetByIdAsync(bookId)).ReturnsAsync(book);

            var result = await _controller.GetById(bookId);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(book));
            });
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookId = Guid.NewGuid();
            _bookServiceMock.Setup(s => s.GetByIdAsync(bookId)).ReturnsAsync((BookDto?)null);

            var result = await _controller.GetById(bookId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            var dto = new CreateBookDto { Title = "New Book", Author = "New Author" };
            var book = new BookDto { Id = Guid.NewGuid(), Title = dto.Title, Author = dto.Author };
            _bookServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(book);

            var result = await _controller.Create(dto);

            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));
                Assert.That(createdAtActionResult.RouteValues, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(createdAtActionResult.RouteValues!["id"], Is.EqualTo(book.Id));
                Assert.That(createdAtActionResult.Value, Is.EqualTo(book));
            });
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Title", "Required");

            var result = await _controller.Create(new CreateBookDto());

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_ReturnsOk_WhenBookIsUpdated()
        {
            var bookId = Guid.NewGuid();
            var dto = new UpdateBookDto { Title = "Updated Book", Author = "Updated Author" };
            var updatedBook = new BookDto { Id = bookId, Title = dto.Title, Author = dto.Author };
            _bookServiceMock.Setup(s => s.UpdateAsync(bookId, dto)).ReturnsAsync(updatedBook);

            var result = await _controller.Update(bookId, dto);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(updatedBook));
            });
        }

        [Test]
        public async Task Update_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookId = Guid.NewGuid();
            var dto = new UpdateBookDto { Title = "Updated Book", Author = "Updated Author" };
            _bookServiceMock.Setup(s => s.UpdateAsync(bookId, dto)).ReturnsAsync((BookDto?)null);

            var result = await _controller.Update(bookId, dto);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenBookIsDeleted()
        {
            var bookId = Guid.NewGuid();
            _bookServiceMock.Setup(s => s.DeleteAsync(bookId)).ReturnsAsync(true);

            var result = await _controller.Delete(bookId);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookId = Guid.NewGuid();
            _bookServiceMock.Setup(s => s.DeleteAsync(bookId)).ReturnsAsync(false);

            var result = await _controller.Delete(bookId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
