using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
    }
}