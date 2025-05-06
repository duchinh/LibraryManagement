namespace LibraryManagement.Core.Enums
{
    public enum BorrowStatus
    {
        Pending,    // Đang chờ duyệt
        Approved,   // Đã được duyệt & đang mượn
        Borrowed,   // Đang mượn
        Returned,   // Đã trả
        Rejected,   // Bị từ chối
        Overdue     // Quá hạn chưa trả
    }
} 