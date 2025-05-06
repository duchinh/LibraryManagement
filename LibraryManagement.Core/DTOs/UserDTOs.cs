using System;

namespace LibraryManagement.Core.DTOs
{
    public class UserBorrowingStatsDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalBorrows { get; set; }
        public int CurrentBorrows { get; set; }
        public int OverdueBorrows { get; set; }
    }
}