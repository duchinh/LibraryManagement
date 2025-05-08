using Moq;
using LibraryApp.Api.Controllers;
using LibraryApp.Application.Interfaces;
using LibraryApp.Application.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LibraryApp.Tests.ControllerTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim("Role", "Admin")
            ], "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task GetUserById_WithValidId_ReturnsOkResult()
        {
            var userId = Guid.NewGuid();
            var expectedUser = new UserDto { Id = userId, UserName = "testuser" };

            _mockUserService.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            var result = await _controller.GetUserById(userId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(expectedUser));
            _mockUserService.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task GetUserById_WithInvalidId_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync((UserDto?)null);

            var result = await _controller.GetUserById(userId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAllUsers_ReturnsOkResultWithUsers()
        {
            var expectedUsers = new List<UserDto>
            {
                new() { Id = Guid.NewGuid(), UserName = "user1" },
                new() { Id = Guid.NewGuid(), UserName = "user2" }
            };

            _mockUserService.Setup(x => x.GetAllUsersAsync())
                .ReturnsAsync(expectedUsers);

            var result = await _controller.GetAllUsers();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(expectedUsers));
        }

        [Test]
        public async Task UpdateUser_WithValidData_ReturnsOkResult()
        {
            var updateDto = new UpdateUserDto { Id = Guid.NewGuid(), UserName = "updated" };
            var expectedUser = new UserDto { Id = updateDto.Id, UserName = updateDto.UserName };

            _mockUserService.Setup(x => x.UpdateUserAsync(updateDto))
                .ReturnsAsync(expectedUser);

            var result = await _controller.UpdateUser(updateDto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(expectedUser));
        }

        [Test]
        public async Task UpdateUser_WithNonExistingId_ReturnsNotFound()
        {
            var updateDto = new UpdateUserDto { Id = Guid.NewGuid() };
            _mockUserService.Setup(x => x.UpdateUserAsync(updateDto))
                .ReturnsAsync((UserDto?)null);

            var result = await _controller.UpdateUser(updateDto);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteUser_WithValidId_ReturnsNoContent()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.DeleteUserAsync(userId))
                .ReturnsAsync(true);

            var result = await _controller.DeleteUser(userId);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteUser_WithInvalidId_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.DeleteUserAsync(userId))
                .ReturnsAsync(false);

            var result = await _controller.DeleteUser(userId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

    }
}
