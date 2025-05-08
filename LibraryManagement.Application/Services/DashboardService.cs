using LibraryManagement.Core.DTOs;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.Interfaces.Repositories;

namespace LibraryManagement.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<DashboardStatisticsDto> GetDashboardAsync()
        {
            var (totalBooks, totalBorrowed, totalUsers, totalRequests) = await _repository.GetCountsAsync();
            var mostBorrowedBooksRaw = await _repository.GetMostBorrowedBooksAsync();
            var userActivitiesRaw = await _repository.GetUserActivitiesAsync();

            var mostBorrowedBooks = mostBorrowedBooksRaw
                .Select(b => new MostBorrowedBookDto { Title = b.title, BorrowCount = b.count })
                .ToList();

            var userActivities = userActivitiesRaw
                .Select(u => new UserActivityDto
                {
                    UserName = u.userName,
                    RequestsMade = u.requests,
                    BooksBorrowed = u.books,
                    RequestsDone = u.done,
                    RequestsRejected = u.rejected,
                    RequestsPending = u.pending
                })
                .ToList();

            return new DashboardStatisticsDto
            {
                TotalBooks = totalBooks,
                TotalBorrowedBooks = totalBorrowed,
                TotalUsers = totalUsers,
                TotalBorrowingRequests = totalRequests,
                MostBorrowedBooks = mostBorrowedBooks,
                UserActivities = userActivities
            };
        }
    }
}