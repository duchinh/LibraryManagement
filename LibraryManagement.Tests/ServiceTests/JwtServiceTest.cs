using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace LibraryApp.Tests.ServiceTests
{
    [TestFixture]
    public class JwtServiceTests
    {
        private Mock<IConfiguration> _configurationMock;
        private JwtService _jwtService;
        private User _testUser;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("glK4EQRhQecQP2XK+/G+iG/157799dHmz76UPajrKyY=");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("http://localhost:5184");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("http://localhost:5184");

            _jwtService = new JwtService(_configurationMock.Object);

            _testUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Role = LibraryApp.Domain.Enums.UserRole.User,
                Email = "testuser@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123-456-7890",
                Address = "123 Test St.",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
        }

        [Test]
        public void GenerateToken_ReturnsValidToken()
        {
            var token = _jwtService.GenerateToken(_testUser);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            Assert.That(jsonToken, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(jsonToken?.Issuer, Is.EqualTo(_configurationMock.Object["Jwt:Issuer"]));
                Assert.That(jsonToken?.Audiences.FirstOrDefault(), Is.EqualTo(_configurationMock.Object["Jwt:Audience"]));
                Assert.That(jsonToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value, Is.EqualTo(_testUser.UserName));
                Assert.That(jsonToken?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, Is.EqualTo(_testUser.Id.ToString()));
                Assert.That(jsonToken?.Claims.FirstOrDefault(c => c.Type == "Role")?.Value, Is.EqualTo(_testUser.Role.ToString()));
                Assert.That(jsonToken?.Claims.FirstOrDefault(c => c.Type == "Email")?.Value, Is.EqualTo(_testUser.Email));
            });
        }
    }
}
