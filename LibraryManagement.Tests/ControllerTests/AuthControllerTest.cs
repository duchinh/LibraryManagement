using LibraryApp.Api.Controllers;
using LibraryApp.Application.DTOs.Auth;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApp.Tests.ControllerTests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock;
        private AuthController _controller;

        [SetUp]
        public void SetUp()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Test]
        public async Task Register_ReturnsOk_WhenSuccessful()
        {
            var request = new RegisterRequestDto { UserName = "testuser", Password = "pass" };
            var expectedResponse = new AuthResponseDto { Success = true, Token = "abc123" };
            _authServiceMock.Setup(s => s.Register(request)).ReturnsAsync(expectedResponse);

            var result = await _controller.Register(request);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public async Task Register_ReturnsBadRequest_WhenFailed()
        {
            var request = new RegisterRequestDto { UserName = "testuser", Password = "pass" };
            var expectedResponse = new AuthResponseDto { Success = false, Message = "Registration failed" };
            _authServiceMock.Setup(s => s.Register(request)).ReturnsAsync(expectedResponse);

            var result = await _controller.Register(request);

            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(badRequest.StatusCode, Is.EqualTo(400));
                Assert.That(badRequest.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public async Task Login_ReturnsOk_WhenSuccessful()
        {
            var request = new LoginRequestDto { UserName = "testuser", Password = "pass" };
            var expectedResponse = new AuthResponseDto { Success = true, Token = "jwt_token" };
            _authServiceMock.Setup(s => s.Login(request)).ReturnsAsync(expectedResponse);

            var result = await _controller.Login(request);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public async Task Login_ReturnsBadRequest_WhenFailed()
        {
            var request = new LoginRequestDto { UserName = "testuser", Password = "wrongpass" };
            var expectedResponse = new AuthResponseDto { Success = false, Message = "Invalid credentials" };
            _authServiceMock.Setup(s => s.Login(request)).ReturnsAsync(expectedResponse);

            var result = await _controller.Login(request);

            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(badRequest.StatusCode, Is.EqualTo(400));
                Assert.That(badRequest.Value, Is.EqualTo(expectedResponse));
            });
        }
    }
}
