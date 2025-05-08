using LibraryApp.Application.Common;
using LibraryApp.Application.DTOs.Auth;
using LibraryApp.Application.Interfaces;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Interfaces;
using Moq;
using Microsoft.Extensions.Configuration;

namespace LibraryApp.Tests.ServiceTests
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IAuthRepository> _authRepositoryMock;
        private Mock<IConfiguration> _configurationMock;
        private IAuthService _authService;
        private Mock<IJwtService> _jwtServiceMock;

        [SetUp]
        public void Setup()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("glK4EQRhQecQP2XK+/G+iG/157799dHmz76UPajrKyY=");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("FakeIssuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("FakeAudience");
            _jwtServiceMock = new Mock<IJwtService>();
            _authService = new AuthService(_authRepositoryMock.Object, new JwtService(_configurationMock.Object));
        }

        [Test]
        public async Task Register_ReturnsError_WhenUsernameExists()
        {
            var request = new RegisterRequestDto { UserName = "existingUser" };
            _authRepositoryMock.Setup(r => r.UserNameExistsAsync(request.UserName)).ReturnsAsync(true);

            var result = await _authService.Register(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Message, Is.EqualTo("Username already exists"));
            });
        }

        [Test]
        public async Task Register_ReturnsError_WhenEmailExists()
        {
            var request = new RegisterRequestDto { UserName = "newUser", Email = "test@example.com" };
            _authRepositoryMock.Setup(r => r.UserNameExistsAsync(request.UserName)).ReturnsAsync(false);
            _authRepositoryMock.Setup(r => r.EmailExistsAsync(request.Email)).ReturnsAsync(true);

            var result = await _authService.Register(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Message, Is.EqualTo("Email already exists"));
            });
        }

        [Test]
        public async Task Register_ReturnsSuccess_WhenUserIsNew()
        {
            var request = new RegisterRequestDto
            {
                UserName = "newUser",
                Email = "test@example.com",
                Password = "password123"
            };
            _authRepositoryMock.Setup(r => r.UserNameExistsAsync(request.UserName)).ReturnsAsync(false);
            _authRepositoryMock.Setup(r => r.EmailExistsAsync(request.Email)).ReturnsAsync(false);

            var result = await _authService.Register(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Message, Is.EqualTo("Registration successful"));
            });
        }

        [Test]
        public async Task Login_ReturnsError_WhenUserDoesNotExist()
        {
            var request = new LoginRequestDto { UserName = "notfound", Password = "pwd" };
            _authRepositoryMock.Setup(r => r.GetByUserNameAsync(request.UserName)).ReturnsAsync((User?)null);

            var result = await _authService.Login(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Message, Is.EqualTo("Invalid username or password"));
            });
        }

        [Test]
        public async Task Login_ReturnsError_WhenPasswordIsWrong()
        {
            var user = new User { UserName = "test", PasswordHash = PasswordHasher.HashPassword("correctpwd") };
            var request = new LoginRequestDto { UserName = "test", Password = "wrongpwd" };
            _authRepositoryMock.Setup(r => r.GetByUserNameAsync(request.UserName)).ReturnsAsync(user);

            var result = await _authService.Login(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Message, Is.EqualTo("Invalid username or password"));
            });
        }

        [Test]
        public async Task Login_ReturnsError_WhenUserIsInactive()
        {
            var user = new User
            {
                UserName = "test",
                PasswordHash = PasswordHasher.HashPassword("password"),
                IsActive = false
            };
            var request = new LoginRequestDto { UserName = "test", Password = "password" };
            _authRepositoryMock.Setup(r => r.GetByUserNameAsync(request.UserName)).ReturnsAsync(user);

            var result = await _authService.Login(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Message, Is.EqualTo("Account is deactivated"));
            });
        }

        [Test]
        public async Task Login_ReturnsError_WhenUserNameIsEmpty()
        {
            var request = new LoginRequestDto { UserName = "", Password = "password" };

            var result = await _authService.Login(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Message, Is.EqualTo("Invalid username or password"));
            });
        }

        [Test]
        public async Task Login_ReturnsError_WhenPasswordIsEmpty()
        {
            var request = new LoginRequestDto { UserName = "test", Password = "" };

            var result = await _authService.Login(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Message, Is.EqualTo("Invalid username or password"));
            });
        }

        [Test]
        public async Task Login_ReturnsSuccess_WhenCredentialsAreCorrect()
        {
            var user = new User
            {
                UserName = "test",
                PasswordHash = PasswordHasher.HashPassword("password"),
                IsActive = true,
                Role = 0,
            };
            var request = new LoginRequestDto { UserName = "test", Password = "password" };
            _authRepositoryMock.Setup(r => r.GetByUserNameAsync(request.UserName)).ReturnsAsync(user);

            var result = await _authService.Login(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Message, Is.EqualTo("Login successful"));
                Assert.That(result.Role, Is.EqualTo(user.Role));
            });
        }
    }
}
