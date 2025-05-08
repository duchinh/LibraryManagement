using LibraryApp.Api.Controllers;
using LibraryApp.Application.DTOs.BookBorrowing;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApp.Tests.ControllerTests
{
    [TestFixture]
    public class RequestDetailControllerTests
    {
        private Mock<IBookBorrowingRequestDetailService> _serviceMock;
        private RequestDetailController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IBookBorrowingRequestDetailService>();
            _controller = new RequestDetailController(_serviceMock.Object);
        }

        [Test]
        public async Task GetDetailsByRequestId_ReturnsOk_WhenDetailsExist()
        {
            var requestId = Guid.NewGuid();
            var details = new List<BookBorrowingRequestDetailDto>
            {
                new() { Id = Guid.NewGuid(), BookId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), BookId = Guid.NewGuid() }
            };

            _serviceMock.Setup(s => s.GetDetailsByRequestIdAsync(requestId)).ReturnsAsync(details);

            var result = await _controller.GetDetailsByRequestId(requestId);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(details));
        }

        [Test]
        public async Task GetDetailsByRequestId_ReturnsNotFound_WhenNoDetailsExist()
        {
            var requestId = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetDetailsByRequestIdAsync(requestId)).ReturnsAsync([]);

            var result = await _controller.GetDetailsByRequestId(requestId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
