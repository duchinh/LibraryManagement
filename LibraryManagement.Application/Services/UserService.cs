using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.Interfaces.Repositories;
using BCrypt.Net;
using LibraryManagement.Core.DTOs;
using System.Linq;
using AutoMapper;
namespace LibraryManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTOs?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return _mapper.Map<UserDTOs>(user);
        }

        public async Task<List<UserDTOs>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return _mapper.Map<List<UserDTOs>>(users);
        }

        public async Task<UserDTOs?> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetUserByIdAsync(updateUserDto.Id);
            if (user == null)

                return null;

            var existingPassword = user.PasswordHash;

            _mapper.Map(updateUserDto, user);
            user.PasswordHash = existingPassword;
            user.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateUserAsync(user);

            return _mapper.Map<UserDTOs>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}