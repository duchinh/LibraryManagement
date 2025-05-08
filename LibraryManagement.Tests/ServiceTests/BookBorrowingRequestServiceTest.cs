using AutoMapper;
using LibraryApp.Application.DTOs.BookBorrowing;
using LibraryApp.Application.Interfaces;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using LibraryApp.Domain.Interfaces;
using Moq;

namespace LibraryApp.Tests.ServiceTests
{
    [TestFixture]
    public class BookBorrowingRequestServiceTests
    {
        private Mock<IBookBorrowingRequestRepository> _requestRepoMock;
        private Mock<IBookRepository> _bookRepoMock;
        private Mock<IMapper> _mapperMock;
        private IBookBorrowingRequestService _service;

        [SetUp]
        public void Setup()
        {
            _requestRepoMock = new Mock<IBookBorrowingRequestRepository>();
            _bookRepoMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new BookBorrowingRequestService(
                _requestRepoMock.Object,
                _bookRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task BorrowBooksAsync_ThrowsException_WhenMonthlyLimitReached()
        {
            var dto = new BookBorrowingRequestCreateDto
            {
                UserId = Guid.NewGuid(),
                BookIds = new List<Guid> { Guid.NewGuid() }
            };

            _requestRepoMock.Setup(r => r.GetMonthlyRequestCountAsync(dto.UserId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(3);

            var ex = await Task.Run(() => Assert.ThrowsAsync<Exception>(async () => await _service.BorrowBooksAsync(dto)));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.Message, Is.EqualTo("You have reached the monthly request limit (3 requests)."));
        }

        [Test]
        public async Task BorrowBooksAsync_ThrowsException_WhenBooksDoNotExist()
        {
            var dto = new BookBorrowingRequestCreateDto
            {
                UserId = Guid.NewGuid(),
                BookIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            _requestRepoMock.Setup(r => r.GetMonthlyRequestCountAsync(dto.UserId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(1);
            _bookRepoMock.Setup(b => b.GetBooksByIdsAsync(dto.BookIds))
                .ReturnsAsync(new List<Book>());

            var ex = await Task.Run(() => Assert.ThrowsAsync<Exception>(async () => await _service.BorrowBooksAsync(dto)));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.Message, Is.EqualTo("One or more selected books do not exist."));
        }

        [Test]
        public async Task BorrowBooksAsync_CreatesRequest_WhenBooksAreAvailable()
        {
            var dto = new BookBorrowingRequestCreateDto
            {
                UserId = Guid.NewGuid(),
                BookIds = new List<Guid> { Guid.NewGuid() }
            };

            var books = new List<Book>
            {
                new Book { Id = dto.BookIds[0], AvailableCopies = 1 }
            };

            var request = new BookBorrowingRequest
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetail>()
            };

            var responseDto = new BookBorrowingRequestResponseDto
            {
                Id = request.Id,
                UserId = request.UserId
            };

            _requestRepoMock.Setup(r => r.GetMonthlyRequestCountAsync(dto.UserId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(1);
            _bookRepoMock.Setup(b => b.GetBooksByIdsAsync(dto.BookIds))
                .ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<BookBorrowingRequest>(dto)).Returns(request);
            _mapperMock.Setup(m => m.Map<BookBorrowingRequestResponseDto>(request)).Returns(responseDto);

            var result = await _service.BorrowBooksAsync(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(responseDto));
                _requestRepoMock.Verify(r => r.CreateAsync(It.IsAny<BookBorrowingRequest>()), Times.Once);
                _bookRepoMock.Verify(b => b.UpdateBooksAsync(It.IsAny<List<Book>>()), Times.Once);
            });
        }

        [Test]
        public Task UpdateRequestStatusAsync_ThrowsException_WhenRequestNotFound()
        {
            var dto = new BookBorrowingRequestUpdateStatusDto
            {
                Status = RequestStatus.Approved
            };
            var requestId = Guid.NewGuid();

            _requestRepoMock.Setup(r => r.GetRequestByIdAsync(requestId))
                .ReturnsAsync((BookBorrowingRequest?)null);

            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateRequestStatusAsync(requestId, dto));
            Assert.That(ex.Message, Is.EqualTo("Request not found."));

            return Task.CompletedTask;
        }

        [Test]
        public async Task UpdateRequestStatusAsync_RejectsRequest_UpdatesBookCopies()
        {
            var requestId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            var dto = new BookBorrowingRequestUpdateStatusDto
            {
                Status = RequestStatus.Rejected
            };

            var request = new BookBorrowingRequest
            {
                Id = requestId,
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetail>
                {
                    new BookBorrowingRequestDetail
                    {
                        Id = Guid.NewGuid(),
                        BookId = bookId
                    }
                }
            };

            var book = new Book
            {
                Id = bookId,
                AvailableCopies = 2
            };

            _requestRepoMock.Setup(r => r.GetRequestByIdAsync(requestId))
                .ReturnsAsync(request);

            _bookRepoMock.Setup(b => b.GetByIdAsync(bookId))
                .ReturnsAsync(book);

            _mapperMock.Setup(m => m.Map(dto, request))
                .Callback(() =>
                {
                    request.Status = dto.Status;
                });

            _mapperMock.Setup(m => m.Map<BookBorrowingRequestResponseDto>(request))
                .Returns(new BookBorrowingRequestResponseDto { Id = requestId, Status = RequestStatus.Rejected });

            var result = await _service.UpdateRequestStatusAsync(requestId, dto);

            Assert.That(result.Status, Is.EqualTo(RequestStatus.Rejected));
            Assert.That(book.AvailableCopies, Is.EqualTo(3));
            _requestRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}