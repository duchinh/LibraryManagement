using AutoMapper;
using LibraryApp.Application.DTOs.User;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Interfaces;
using Moq;

namespace LibraryApp.Tests.ServiceTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetUserByIdAsync_WhenUserExists_ReturnsUserDto()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedPassword",
                UpdatedAt = DateTime.UtcNow
            };
            var userDto = new UserDto
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user))
                .Returns(userDto);

            var result = await _userService.GetUserByIdAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(userDto));
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(userId));
                Assert.That(result.FirstName, Is.EqualTo("John"));
                Assert.That(result.LastName, Is.EqualTo("Doe"));
                Assert.That(result.Email, Is.EqualTo("john.doe@example.com"));
            });
        }

        [Test]
        public async Task GetUserByIdAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((User?)null);

            var result = await _userService.GetUserByIdAsync(userId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsListOfUserDtos()
        {
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new User { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
            };
            var userDtos = new List<UserDto>
            {
                new UserDto { Id = users[0].Id, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new UserDto { Id = users[1].Id, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(users);
            _mapperMock.Setup(mapper => mapper.Map<List<UserDto>>(users))
                .Returns(userDtos);

            var result = await _userService.GetAllUsersAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<UserDto>>());
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].FirstName, Is.EqualTo("John"));
                Assert.That(result[1].FirstName, Is.EqualTo("Jane"));
            });
        }

        [Test]
        public async Task GetAllUsersAsync_WhenNoUsersExist_ReturnsEmptyList()
        {
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(new List<User>());
            _mapperMock.Setup(mapper => mapper.Map<List<UserDto>>(It.IsAny<List<User>>()))
                .Returns(new List<UserDto>());

            var result = await _userService.GetAllUsersAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task UpdateUserAsync_WhenUserExists_UpdatesAndReturnsUserDto()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedPassword",
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };
            var updateUserDto = new UpdateUserDto
            {
                Id = userId,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com"
            };
            var updatedUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                PasswordHash = "hashedPassword",
                UpdatedAt = DateTime.UtcNow
            };
            var userDto = new UserDto
            {
                Id = userId,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(existingUser);
            _mapperMock.Setup(mapper => mapper.Map(updateUserDto, existingUser))
                .Callback<UpdateUserDto, User>((dto, user) =>
                {
                    user.FirstName = dto.FirstName;
                    user.LastName = dto.LastName;
                    user.Email = dto.Email;
                });
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(updatedUser);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(updatedUser))
                .Returns(userDto);

            var result = await _userService.UpdateUserAsync(updateUserDto);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(userId));
                Assert.That(result.FirstName, Is.EqualTo("John"));
                Assert.That(result.LastName, Is.EqualTo("Smith"));
                Assert.That(result.Email, Is.EqualTo("john.smith@example.com"));
                Assert.That(updatedUser.PasswordHash, Is.EqualTo("hashedPassword"));
            });
        }

        [Test]
        public async Task UpdateUserAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            var userId = Guid.NewGuid();
            var updateUserDto = new UpdateUserDto { Id = userId };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((User?)null);

            var result = await _userService.UpdateUserAsync(updateUserDto);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteUserAsync_WhenUserExists_ReturnsTrue()
        {
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.DeleteUserAsync(userId))
                .ReturnsAsync(true);

            var result = await _userService.DeleteUserAsync(userId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteUserAsync_WhenUserDoesNotExist_ReturnsFalse()
        {
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.DeleteUserAsync(userId))
                .ReturnsAsync(false);

            var result = await _userService.DeleteUserAsync(userId);

            Assert.That(result, Is.False);
        }
    }
}