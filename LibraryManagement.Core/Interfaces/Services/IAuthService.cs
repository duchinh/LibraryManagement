using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterRequestDto request);
        Task<AuthResponseDto> Login(LoginRequestDto request);
    }
}