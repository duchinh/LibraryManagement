namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<(int totalBooks, int totalBorrowed, int totalUsers, int totalRequests)> GetCountsAsync();
        Task<List<(string title, int count)>> GetMostBorrowedBooksAsync();
        Task<List<(string userName, int requests, int books, int done, int rejected, int pending)>> GetUserActivitiesAsync();
    }
}
