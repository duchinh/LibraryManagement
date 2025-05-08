using LibraryApp.Application.DTOs.BookBorrowing;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Interfaces;
using Moq;
using AutoMapper;
using LibraryApp.Application.Interfaces;

namespace LibraryApp.Tests.ServiceTests
{
    [TestFixture]
    public class BookBorrowingRequestDetailServiceTests
    {
        private Mock<IBookBorrowingRequestDetailRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private IBookBorrowingRequestDetailService _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IBookBorrowingRequestDetailRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new BookBorrowingRequestDetailService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetDetailsByRequestIdAsync_ReturnsMappedDetails()
        {
            var requestId = Guid.NewGuid();
            var requestDetails = new List<BookBorrowingRequestDetail>
            {
                new() { BookId = Guid.NewGuid(), Id = requestId },
                new() { BookId = Guid.NewGuid(), Id = requestId }
            };
            var expectedDtoList = new List<BookBorrowingRequestDetailDto>
            {
                new() { BookId = requestDetails[0].BookId, Id = requestDetails[0].Id },
                new() { BookId = requestDetails[1].BookId, Id = requestDetails[1].Id }
            };

            _repositoryMock.Setup(r => r.GetByRequestIdAsync(requestId)).ReturnsAsync(requestDetails);
            _mapperMock.Setup(m => m.Map<List<BookBorrowingRequestDetailDto>>(requestDetails)).Returns(expectedDtoList);

            var result = await _service.GetDetailsByRequestIdAsync(requestId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(expectedDtoList));
                _repositoryMock.Verify(r => r.GetByRequestIdAsync(requestId), Times.Once);
                _mapperMock.Verify(m => m.Map<List<BookBorrowingRequestDetailDto>>(requestDetails), Times.Once);
            });
        }

    }
}