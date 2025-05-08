using LibraryApp.Api.Controllers;
using LibraryApp.Application.DTOs.BookBorrowing;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApp.Tests.ControllerTests
{
    [TestFixture]
    public class BorrowingControllerTests
    {
        private Mock<IBookBorrowingRequestService> _serviceMock;
        private BorrowingController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IBookBorrowingRequestService>();
            _controller = new BorrowingController(_serviceMock.Object);
        }

        [Test]
        public async Task BorrowBooks_ReturnsOk_WhenRequestIsSuccessful()
        {
            var dto = new BookBorrowingRequestCreateDto { UserId = Guid.NewGuid(), BookIds = new List<Guid> { Guid.NewGuid() } };
            var result = new BookBorrowingRequestResponseDto { Id = Guid.NewGuid(), UserId = dto.UserId };

            _serviceMock.Setup(s => s.BorrowBooksAsync(dto)).ReturnsAsync(result);

            var response = await _controller.BorrowBooks(dto);

            var okResult = response as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(result));
            });
        }

        [Test]
        public async Task BorrowBooks_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            var dto = new BookBorrowingRequestCreateDto { UserId = Guid.NewGuid(), BookIds = new List<Guid> { Guid.NewGuid() } };

            _serviceMock.Setup(s => s.BorrowBooksAsync(dto)).ThrowsAsync(new Exception("Error"));

            var response = await _controller.BorrowBooks(dto);

            var badRequestResult = response as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetAllRequestsForUser_ReturnsOk_WhenRequestsExist()
        {
            var userId = Guid.NewGuid();
            var requests = new List<BookBorrowingRequestResponseDto>
            {
                new BookBorrowingRequestResponseDto { UserId = userId }
            };

            _serviceMock.Setup(s => s.GetAllRequestsForUserAsync(userId)).ReturnsAsync(requests);

            var result = await _controller.GetAllRequestsForUser(userId);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(requests));
            });
        }

        [Test]
        public async Task GetAllRequestsForAdmin_ReturnsOk_WhenRequestsExist()
        {
            var requests = new List<BookBorrowingRequestResponseDto>
            {
                new BookBorrowingRequestResponseDto { UserId = Guid.NewGuid()}
            };

            _serviceMock.Setup(s => s.GetAllRequestsAsync()).ReturnsAsync(requests);

            var result = await _controller.GetAllRequestsForAdmin();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(requests));
            });
        }

        [Test]
        public async Task UpdateRequestStatus_ReturnsOk_WhenRequestIsUpdated()
        {
            var requestId = Guid.NewGuid();
            var dto = new BookBorrowingRequestUpdateStatusDto { Status = RequestStatus.Approved };
            var updatedRequest = new BookBorrowingRequestResponseDto { Id = requestId, Status = RequestStatus.Approved };

            _serviceMock.Setup(s => s.UpdateRequestStatusAsync(requestId, dto)).ReturnsAsync(updatedRequest);

            var result = await _controller.UpdateRequestStatus(requestId, dto);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(updatedRequest));
            });
        }

        [Test]
        public async Task UpdateRequestStatus_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            var requestId = Guid.NewGuid();
            var dto = new BookBorrowingRequestUpdateStatusDto { Status = RequestStatus.Approved };

            _serviceMock.Setup(s => s.UpdateRequestStatusAsync(requestId, dto)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.UpdateRequestStatus(requestId, dto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }
    }
}
