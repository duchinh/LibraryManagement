using LibraryApp.API.Controllers;
using LibraryApp.Application.DTOs;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApp.Tests.ControllerTests
{
    [TestFixture]
    public class DashboardControllerTest
    {
        private Mock<IDashboardService> _mockDashboardService;
        private DashboardController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockDashboardService = new Mock<IDashboardService>();
            _controller = new DashboardController(_mockDashboardService.Object);
        }

        [Test]
        public async Task GetDashboard_ReturnsOkWithDashboardStatistics()
        {
            var expectedStats = new DashboardStatisticsDto
            {
                TotalBooks = 100,
                TotalUsers = 50,
            };

            _mockDashboardService
                .Setup(s => s.GetDashboardAsync())
                .ReturnsAsync(expectedStats);

            var actionResult = await _controller.GetDashboard();
            var result = actionResult.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result!.StatusCode, Is.EqualTo(200));
                Assert.That(result.Value, Is.EqualTo(expectedStats));
            });
        }
    }
}
