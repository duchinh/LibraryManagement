using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDTOs>> GetAllUsersAsync();
        Task<UserDTOs?> GetUserByIdAsync(Guid id);
        Task<UserDTOs?> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}