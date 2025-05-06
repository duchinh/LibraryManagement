using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateAccessToken(User user);
        Task<string> GenerateRefreshToken();
        Task<bool> ValidateToken(string token);
        Task<(string accessToken, string refreshToken)> GenerateTokens(User user);
        Task<bool> ValidateRefreshTokenAsync(string refreshToken);
        Task<string> GetUserIdFromTokenAsync(string token);
    }
}