using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IBookBorrowingRequestRepository
    {
        Task<List<BookBorrowingRequest>> GetAllRequestsAsync();
        Task<BookBorrowingRequest?> GetRequestByIdAsync(Guid requestId);
        Task<List<BookBorrowingRequest>> GetAllRequestsForUserAsync(Guid userId);
        Task<int> GetMonthlyRequestCountAsync(Guid userId, DateTime monthStart, DateTime monthEnd);
        Task<BookBorrowingRequest> CreateAsync(BookBorrowingRequest request);
        Task<List<Book>> GetBooksByIdsAsync(List<Guid> bookIds);
        Task SaveChangesAsync();
    }
}