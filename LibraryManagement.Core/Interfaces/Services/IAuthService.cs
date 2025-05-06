using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password);
        Task<(string accessToken, string refreshToken)> RegisterAsync(
            string username,
            string password,
            string email,
            string fullName,
            string? phone
        );
        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task ResetPasswordAsync(string email);
        Task VerifyEmailAsync(string email, string token);
    }
}