namespace LibraryManagement.Core.Enums
{
    public enum RequestStatus
    {
        Waiting,     // Đang chờ duyệt
        Approved,   // Đã được duyệt & đang mượn 
        Rejected,   // Bị từ chối         
    }
}