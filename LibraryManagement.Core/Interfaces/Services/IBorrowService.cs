using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IBorrowService
    {
        Task<IEnumerable<Borrow>> GetAllBorrowsAsync();
        Task<Borrow?> GetBorrowByIdAsync(int id);
        Task<Borrow> CreateBorrowAsync(Borrow borrow);
        Task UpdateBorrowAsync(Borrow borrow);
        Task DeleteBorrowAsync(int id);
        Task<IEnumerable<Borrow>> GetBorrowsByUserIdAsync(int userId);
        Task<IEnumerable<Borrow>> GetBorrowsByBookIdAsync(int bookId);
        Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync();
        Task ReturnBookAsync(int borrowId);
        Task<bool> IsBookAvailableAsync(int bookId);
        Task<bool> HasUserReachedBorrowLimitAsync(int userId);
        Task<bool> HasUserOverdueBooksAsync(int userId);
        Task<bool> IsBorrowOverdueAsync(int borrowId);
    }
} 