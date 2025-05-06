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

namespace LibraryManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int MaxBorrowsPerUser = 5;
        private readonly IUserRepository _userRepository;
        private readonly IBorrowRepository _borrowRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IBorrowRepository borrowRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _borrowRepository = borrowRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Check email uniqueness if email is being changed
            if (user.Email != existingUser.Email)
            {
                if (await _userRepository.GetByEmailAsync(user.Email) != null)
                {
                    throw new InvalidOperationException("Email already exists");
                }
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Check if user has any active borrows
            var borrows = await _borrowRepository.GetByUserIdAsync(id);
            if (borrows.Any(b => !b.GetIsReturned()))
            {
                throw new InvalidOperationException("Cannot delete user with active borrows");
            }

            await _userRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _userRepository.ExistsAsync(id);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _userRepository.ExistsByUsernameAsync(username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _userRepository.ExistsByEmailAsync(email);
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            if (await _userRepository.ExistsByUsernameAsync(user.Username))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại");

            if (await _userRepository.ExistsByEmailAsync(user.Email))
                throw new InvalidOperationException("Email đã tồn tại");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.CreatedAt = DateTime.Now;
            user.Role = UserRole.NormalUser;
            user.IsActive = true;

            var addedUser = await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return addedUser;
        }

        public async Task<int> GetUserBorrowCountAsync(int userId)
        {
            var borrows = await _borrowRepository.GetByUserIdAsync(userId);
            return borrows.Count(b => !b.GetIsReturned());
        }

        public async Task<bool> CanUserBorrowAsync(int userId)
        {
            var borrows = await _borrowRepository.GetByUserIdAsync(userId);
            var count = borrows.Count(b => !b.GetIsReturned());
            return count < MaxBorrowsPerUser;
        }

        public async Task<IEnumerable<User>> GetUsersWithOverdueBooksAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var result = new List<User>();

            foreach (var user in users)
            {
                var borrows = await _borrowRepository.GetByUserIdAsync(user.Id);
                if (borrows.Any(b => !b.GetIsReturned() && b.DueDate < DateTime.Now))
                {
                    result.Add(user);
                }
            }

            return result;
        }

        public async Task<IEnumerable<UserBorrowingStatsDTO>> GetUserBorrowingStatsAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var stats = new List<UserBorrowingStatsDTO>();

            foreach (var user in users)
            {
                var borrows = await _borrowRepository.GetByUserIdAsync(user.Id);
                var totalBorrows = borrows.Count();
                var currentBorrows = borrows.Count(b => !b.GetIsReturned());
                var overdueBorrows = borrows.Count(b => !b.GetIsReturned() && b.DueDate < DateTime.Now);

                stats.Add(new UserBorrowingStatsDTO
                {
                    UserId = user.Id,
                    UserName = $"{user.FirstName} {user.LastName}",
                    TotalBorrows = totalBorrows,
                    CurrentBorrows = currentBorrows,
                    OverdueBorrows = overdueBorrows
                });
            }

            return stats;
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}