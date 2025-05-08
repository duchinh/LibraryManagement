namespace LibraryManagement.Core.DTOs
{
    public class DashboardStatisticsDto
    {
        public int TotalBooks { get; set; }
        public int TotalBorrowedBooks { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBorrowingRequests { get; set; }
        public List<MostBorrowedBookDto> MostBorrowedBooks { get; set; } = [];
        public List<UserActivityDto> UserActivities { get; set; } = [];
    }

    public class MostBorrowedBookDto
    {
        public string Title { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }

    public class UserActivityDto
    {
        public string UserName { get; set; } = string.Empty;
        public int RequestsMade { get; set; }
        public int BooksBorrowed { get; set; }
        public int RequestsDone { get; set; }
        public int RequestsRejected { get; set; }
        public int RequestsPending { get; set; }
    }
}