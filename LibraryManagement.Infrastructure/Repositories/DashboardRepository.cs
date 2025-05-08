using LibraryManagement.Core.Enums;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly LibraryDbContext _context;

        public DashboardRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<(int totalBooks, int totalBorrowed, int totalUsers, int totalRequests)> GetCountsAsync()
        {
            var totalBooks = await _context.Books.CountAsync();
            var totalBorrowed = await _context.BookBorrowingRequests.CountAsync(d => d.ReturnedDate == null);
            var totalUsers = await _context.Users.CountAsync();
            var totalRequests = await _context.BookBorrowingRequests.CountAsync();
            return (totalBooks, totalBorrowed, totalUsers, totalRequests);
        }

        public async Task<List<(string title, int count)>> GetMostBorrowedBooksAsync()
        {
            var rawData = await _context.BookBorrowingRequestDetails
                .Include(d => d.Book)
                .Where(d => d.Book != null)
                .GroupBy(d => d.Book.Title)
                .Select(g => new
                {
                    Title = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            return rawData
                .Select(g => (g.Title, g.Count))
                .ToList();
        }

        public async Task<List<(string userName, int requests, int books, int done, int rejected, int pending)>> GetUserActivitiesAsync()
        {
            var users = await _context.Users
                .Include(u => u.BookBorrowingRequests)
                    .ThenInclude(r => r.BookBorrowingRequestDetails)
                .ToListAsync();

            var result = users
                .Select(u => (
                    UserName: u.UserName,
                    RequestsMade: u.BookBorrowingRequests.Count,
                    BooksBorrowed: u.BookBorrowingRequests
                        .SelectMany(r => r.BookBorrowingRequestDetails)
                        .Count(),
                        RequestDone: u.BookBorrowingRequests.Count(r => r.Status == RequestStatus.Approved),
                        RequestReject: u.BookBorrowingRequests.Count(r => r.Status == RequestStatus.Rejected),
                        RequestPending: u.BookBorrowingRequests.Count(r => r.Status == RequestStatus.Waiting)

                ))
                .OrderByDescending(x => x.BooksBorrowed)
                .Take(5)
                .ToList();

            return result;
        }
    }

}
