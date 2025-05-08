using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetByUserNameAsync(string username);
        Task<bool> UserNameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task AddUserAsync(User user);
    }
}