using ManagementAPI.DTO;
using ManagementAPI.Helpers;
using ManagementAPI.Interfaces;
using ManagementAPI.Models;
using ManagementAPI.Services;
using Moq;
using System.Security.Claims;

namespace ManagementAPI.Tests.LoginServiceTests
{
    public class LoginService_OTPTests
    {
        private readonly Mock<IDefaultUserRepository> _repositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IMailerService> _mailerServiceMock;
        private readonly LoginService _loginService;

        public LoginService_OTPTests()
        {
            _repositoryMock = new Mock<IDefaultUserRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _mailerServiceMock = new Mock<IMailerService>();
            _loginService = new LoginService(_repositoryMock.Object, _jwtServiceMock.Object, _mailerServiceMock.Object);
        }

        [Fact]
        public async Task ValidateLoginByOtp_ReturnsNull_WhenUserNotFound()
        {
            _repositoryMock.Setup(r => r.GetUserByEmailAsync("test@mail.com")).ReturnsAsync((UserTable)null);

            var result = await _loginService.ValidateLoginByOtp(new LoginOtpRequestDto { Email = "test@mail.com", Password = "1234" });

            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateLoginByOtp_ReturnsNull_WhenOtpExpired()
        {
            var user = new UserTable
            {
                Email = "test@mail.com",
                OtpCode = PasswordEncryptionHelper.HashPassword("1234"),
                OtpExpiration = DateTime.UtcNow.AddMinutes(-1)
            };
            _repositoryMock.Setup(r => r.GetUserByEmailAsync("test@mail.com")).ReturnsAsync(user);

            var result = await _loginService.ValidateLoginByOtp(new LoginOtpRequestDto { Email = "test@mail.com", Password = "1234" });

            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateLoginByOtp_ReturnsNull_WhenOtpInvalid()
        {
            var user = new UserTable
            {
                Email = "test@mail.com",
                OtpCode = PasswordEncryptionHelper.HashPassword("5678"),
                OtpExpiration = DateTime.UtcNow.AddMinutes(10)
            };
            _repositoryMock.Setup(r => r.GetUserByEmailAsync("test@mail.com")).ReturnsAsync(user);

            var result = await _loginService.ValidateLoginByOtp(new LoginOtpRequestDto { Email = "test@mail.com", Password = "1234" });

            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateLoginByOtp_ReturnsToken_WhenOtpIsValid()
        {
            var user = new UserTable
            {
                Id = 1,
                Username = "user",
                Email = "test@mail.com",
                Role = "User",
                OtpCode = PasswordEncryptionHelper.HashPassword("1234"),
                OtpExpiration = DateTime.UtcNow.AddMinutes(5)
            };
            _repositoryMock.Setup(r => r.GetUserByEmailAsync("test@mail.com")).ReturnsAsync(user);
            _jwtServiceMock.Setup(j => j.GenerateToken(It.IsAny<IEnumerable<Claim>>())).Returns("mocktoken");

            var result = await _loginService.ValidateLoginByOtp(new LoginOtpRequestDto { Email = "test@mail.com", Password = "1234" });

            Assert.NotNull(result);
            Assert.Equal("Bearer mocktoken", result?.token);
        }
    }
}
