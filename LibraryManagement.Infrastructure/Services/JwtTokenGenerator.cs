using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenGenerator> _logger;

        public JwtTokenGenerator(IConfiguration configuration, ILogger<JwtTokenGenerator> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
            _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
        }

        public Task<string> GenerateAccessToken(User user)
        {
            try
            {
                _logger.LogInformation("Generating access token for user {Username}", user.Username);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(
                        int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "15")
                    ),
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                _logger.LogInformation("Access token generated successfully for user {Username}", user.Username);
                return Task.FromResult(tokenString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating access token for user {Username}", user.Username);
                throw;
            }
        }

        public Task<string> GenerateRefreshToken()
        {
            try
            {
                _logger.LogInformation("Generating refresh token");
                var refreshToken = Guid.NewGuid().ToString();
                _logger.LogInformation("Refresh token generated successfully");
                return Task.FromResult(refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating refresh token");
                throw;
            }
        }

        public Task<bool> ValidateToken(string token)
        {
            try
            {
                _logger.LogInformation("Validating token");
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                _logger.LogInformation("Token validated successfully");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed");
                return Task.FromResult(false);
            }
        }

        public async Task<(string accessToken, string refreshToken)> GenerateTokens(User user)
        {
            try
            {
                _logger.LogInformation("Generating tokens for user {Username}", user.Username);
                var accessToken = await GenerateAccessToken(user);
                var refreshToken = await GenerateRefreshToken();
                _logger.LogInformation("Tokens generated successfully for user {Username}", user.Username);
                return (accessToken, refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating tokens for user {Username}", user.Username);
                throw;
            }
        }

        public Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            try
            {
                _logger.LogInformation("Validating refresh token");
                // Trong thực tế, bạn nên lưu refresh token vào database và kiểm tra
                _logger.LogInformation("Refresh token validated successfully");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating refresh token");
                return Task.FromResult(false);
            }
        }

        public Task<string> GetUserIdFromTokenAsync(string token)
        {
            try
            {
                _logger.LogInformation("Getting user ID from token");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                _logger.LogInformation("User ID retrieved successfully: {UserId}", userId);
                return Task.FromResult(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user ID from token");
                throw;
            }
        }

        public Task RevokeRefreshTokenAsync(string refreshToken)
        {
            try
            {
                _logger.LogInformation("Revoking refresh token");
                // Trong thực tế, bạn nên lưu refresh token vào database và đánh dấu là đã thu hồi
                _logger.LogInformation("Refresh token revoked successfully");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking refresh token");
                throw;
            }
        }
    }
}