using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByEmailVerificationTokenAsync(string token);
        Task<User> GetByResetPasswordTokenAsync(string token);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> VerifyPasswordAsync(string username, string password);
        Task<IEnumerable<Borrow>> GetUserBorrowsAsync(int userId);
        Task<IEnumerable<User>> GetUsersWithOverdueBooksAsync();
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<IEnumerable<User>> SearchAsync(string keyword);
    }
}