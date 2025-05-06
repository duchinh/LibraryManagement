using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IBookBorrowingRequestRepository
    {
        Task<IEnumerable<BookBorrowingRequest>> GetAllAsync();
        Task<BookBorrowingRequest> GetByIdAsync(int id);
        Task<BookBorrowingRequest> AddAsync(BookBorrowingRequest request);
        Task<BookBorrowingRequest> UpdateAsync(BookBorrowingRequest request);
        Task<IEnumerable<BookBorrowingRequest>> GetByUserIdAsync(int userId);
        Task<IEnumerable<BookBorrowingRequest>> GetUserRequestsThisMonthAsync(int userId);
    }
}