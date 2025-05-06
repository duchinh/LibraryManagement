using System;
using System.Threading.Tasks;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;
        private IBookRepository _books = null!;
        private ICategoryRepository _categories = null!;
        private IUserRepository _users = null!;
        private IBorrowRepository _borrows = null!;
        private IDbContextTransaction _transaction = null!;

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
        }

        public IBookRepository Books
        {
            get
            {
                _books ??= new BookRepository(_context);
                return _books;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                _categories ??= new CategoryRepository(_context);
                return _categories;
            }
        }

        public IUserRepository Users
        {
            get
            {
                _users ??= new UserRepository(_context);
                return _users;
            }
        }

        public IBorrowRepository Borrows
        {
            get
            {
                _borrows ??= new BorrowRepository(_context);
                return _borrows;
            }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}