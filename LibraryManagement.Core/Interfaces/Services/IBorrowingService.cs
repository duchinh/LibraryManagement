using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IBorrowingService
    {
        Task<BookBorrowingRequest> CreateBorrowingRequestAsync(int userId, List<int> bookIds);
        Task<BookBorrowingRequest> GetBorrowingRequestByIdAsync(int id);
        Task<IEnumerable<BookBorrowingRequest>> GetUserBorrowingRequestsAsync(int userId);
        Task<IEnumerable<BookBorrowingRequest>> GetAllBorrowingRequestsAsync();
        Task ApproveBorrowingRequestAsync(int requestId, int approverId);
        Task RejectBorrowingRequestAsync(int requestId, int approverId, string reason);
        Task ReturnBooksAsync(int requestId, List<int> bookIds);
        Task<bool> CanUserBorrowMoreBooksAsync(int userId);
        Task<int> GetUserBorrowingCountThisMonthAsync(int userId);
    }
}