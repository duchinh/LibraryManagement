using LibraryApp.Application.Services;
using LibraryApp.Domain.Interfaces;
using Moq;

namespace LibraryApp.Tests.ServiceTests
{
    [TestFixture]
    public class DashboardServiceTests
    {
        private Mock<IDashboardRepository> _repoMock;
        private DashboardService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IDashboardRepository>();
            _service = new DashboardService(_repoMock.Object);
        }

        [Test]
        public async Task GetDashboardAsync_ReturnsCorrectStatistics()
        {
            _repoMock.Setup(r => r.GetCountsAsync())
                .ReturnsAsync((10, 5, 20, 7));

            _repoMock.Setup(r => r.GetMostBorrowedBooksAsync())
                .ReturnsAsync(
                [
                    ("Book A", 15),
                    ("Book B", 12)
                ]);

            _repoMock.Setup(r => r.GetUserActivitiesAsync())
                .ReturnsAsync(
                [
                    ("user1", 5, 3, 2, 1, 2),
                    ("user2", 8, 5, 4, 2, 2)
                ]);

            var result = await _service.GetDashboardAsync();

            Assert.Multiple(() =>
            {
                Assert.That(result.TotalBooks, Is.EqualTo(10));
                Assert.That(result.TotalBorrowedBooks, Is.EqualTo(5));
                Assert.That(result.TotalUsers, Is.EqualTo(20));
                Assert.That(result.TotalBorrowingRequests, Is.EqualTo(7));

                Assert.That(result.MostBorrowedBooks, Has.Count.EqualTo(2));
            });
            Assert.Multiple(() =>
            {
                Assert.That(result.MostBorrowedBooks[0].Title, Is.EqualTo("Book A"));
                Assert.That(result.MostBorrowedBooks[0].BorrowCount, Is.EqualTo(15));

                Assert.That(result.UserActivities, Has.Count.EqualTo(2));
            });
            Assert.Multiple(() =>
            {
                Assert.That(result.UserActivities[0].UserName, Is.EqualTo("user1"));
                Assert.That(result.UserActivities[0].RequestsMade, Is.EqualTo(5));
                Assert.That(result.UserActivities[0].BooksBorrowed, Is.EqualTo(3));
                Assert.That(result.UserActivities[0].RequestsDone, Is.EqualTo(2));
                Assert.That(result.UserActivities[0].RequestsRejected, Is.EqualTo(1));
                Assert.That(result.UserActivities[0].RequestsPending, Is.EqualTo(2));
            });
        }
    }
}
