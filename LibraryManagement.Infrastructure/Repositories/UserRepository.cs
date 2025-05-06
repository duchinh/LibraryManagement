using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Borrows)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted)
                ?? throw new Exception("User not found");
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted)
                ?? throw new Exception("User not found");
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            // Soft delete
            user.IsDeleted = true;
            await UpdateAsync(user);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username && !u.IsDeleted);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<IEnumerable<Borrow>> GetUserBorrowsAsync(int userId)
        {
            return await _context.Borrows
                .Where(b => b.UserId == userId)
                .Include(b => b.Book)
                .ToListAsync();
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken &&
                                        u.RefreshTokenExpiresAt > DateTime.Now);
        }

        public async Task<IEnumerable<User>> SearchAsync(string keyword)
        {
            return await _context.Users
                .Where(u => u.Username.Contains(keyword) ||
                           u.Email.Contains(keyword) ||
                           u.FullName.Contains(keyword))
                .Include(u => u.Borrows)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersWithOverdueBooksAsync()
        {
            var usersWithOverdueBooks = await _context.Users
                .Include(u => u.Borrows)
                .Where(u => u.Borrows.Any(b => !b.GetIsReturned() && b.DueDate < DateTime.Now))
                .ToListAsync();

            return usersWithOverdueBooks;
        }

        public async Task<bool> VerifyPasswordAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.EmailVerificationToken != token)
                return false;

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetByEmailVerificationTokenAsync(string token)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
        }

        public async Task<User> GetByResetPasswordTokenAsync(string token)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUserBorrowCountAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Borrows)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return 0;

            return user.Borrows.Count(b => !b.GetIsReturned());
        }

        public async Task<bool> HasOverdueBooksAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Borrows)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            return user.Borrows.Any(b => !b.GetIsReturned() && b.DueDate < DateTime.Now);
        }
    }
}