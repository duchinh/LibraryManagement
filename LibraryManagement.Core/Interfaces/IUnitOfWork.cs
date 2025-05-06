using System;
using System.Threading.Tasks;
using LibraryManagement.Core.Interfaces.Repositories;

namespace LibraryManagement.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        IBorrowRepository Borrows { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();
    }
}