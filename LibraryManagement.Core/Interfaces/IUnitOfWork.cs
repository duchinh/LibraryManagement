// using System;
// using System.Threading.Tasks;
// using LibraryManagement.Core.Interfaces.Repositories;
// using LibraryManagement.Core.Entities;

// namespace LibraryManagement.Core.Interfaces
// {
//     public interface IUnitOfWork : IDisposable
//     {
//         IBookRepository Books { get; }
//         ICategoryRepository Categories { get; }
//         IUserRepository Users { get; }
//         IBookBorrowingRequestRepository BookBorrowingRequests { get; }
//         IBookBorrowingRequestDetailRepository BookBorrowingRequestDetails { get; }
//         IAuthRepository Auth { get; }
//         IDashboardRepository Dashboard { get; }
//         Task BeginTransactionAsync();
//         Task CommitAsync();
//         Task RollbackAsync();
//         Task SaveChangesAsync();
//     }
// }