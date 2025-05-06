using System;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;

namespace LibraryManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new Exception("Invalid username or password");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            if (!user.IsActive)
                throw new Exception("Account is not active");

            return await _jwtTokenGenerator.GenerateTokens(user);
        }

        public async Task<(string accessToken, string refreshToken)> RegisterAsync(
            string username,
            string password,
            string email,
            string fullName,
            string? phone)
        {
            if (await _userRepository.GetByUsernameAsync(username) != null)
                throw new Exception("Username already exists");

            if (await _userRepository.GetByEmailAsync(email) != null)
                throw new Exception("Email already exists");

            var nameParts = fullName.Split(' ', 2);
            var user = new User
            {
                Username = username,
                Email = email,
                FirstName = nameParts[0],
                LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty,
                PhoneNumber = phone,
                Role = UserRole.NormalUser,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Send email verification
            var verificationToken = Guid.NewGuid().ToString();
            user.EmailVerificationToken = verificationToken;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            await _emailSender.SendEmailVerificationAsync(user.Email, verificationToken);

            return await _jwtTokenGenerator.GenerateTokens(user);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            // TODO: Implement refresh token validation and generation
            throw new NotImplementedException();
        }

        public async Task LogoutAsync(string refreshToken)
        {
            // TODO: Implement logout logic (e.g., invalidate refresh token)
            throw new NotImplementedException();
        }

        public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                throw new Exception("Current password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task ResetPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");

            var resetToken = Guid.NewGuid().ToString();
            user.ResetPasswordToken = resetToken;
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            await _emailSender.SendPasswordResetAsync(email, resetToken);
        }

        public async Task VerifyEmailAsync(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("Email không tồn tại");

            if (user.EmailVerificationToken != token)
                throw new Exception("Token xác thực không hợp lệ");

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}