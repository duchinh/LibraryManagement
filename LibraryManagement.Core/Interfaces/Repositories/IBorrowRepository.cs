using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IBorrowRepository : IGenericRepository<Borrow>
    {
        Task<IEnumerable<Borrow>> GetAllAsync();
        Task<Borrow?> GetByIdAsync(int id);
        Task<Borrow> AddAsync(Borrow borrow);
        Task UpdateAsync(Borrow borrow);
        Task DeleteAsync(Borrow borrow);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Borrow>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Borrow>> GetByBookIdAsync(int bookId);
        Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync();
        Task<bool> IsBookBorrowedAsync(int bookId);
        Task<bool> IsBookAvailableAsync(int bookId);
        Task<bool> HasUserReachedBorrowLimitAsync(int userId);
        Task<bool> HasUserOverdueBooksAsync(int userId);
        Task<bool> IsBorrowOverdueAsync(int borrowId);
        Task<IEnumerable<Borrow>> GetBorrowsByDateRangeAsync(DateTime? startDate, DateTime? endDate);
    }
}