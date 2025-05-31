using ManagementAPI.Controller;
using ManagementAPI.DTO;
using ManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementAPI.Tests.ControllerTests
{
    public class AuthControllerTests
    {
        private readonly Mock<ILoginService> _mockLoginService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockLoginService = new Mock<ILoginService>();
            _controller = new AuthController(_mockLoginService.Object);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenTokenIsValid()
        {
            // Arrange
            var request = new LoginRequestDto();
            var expectedToken = "valid_token";

            _mockLoginService.Setup(s => s.ValidateLoginAsync(request)).ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Contains("token", okResult.Value!.ToString());
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenTokenIsNull()
        {
            // Arrange
            var request = new LoginRequestDto();

            _mockLoginService.Setup(s => s.ValidateLoginAsync(request)).ReturnsAsync((string?)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task LoginOtp_ReturnsOk_WhenTokenIsValid()
        {
            // Arrange
            var request = new LoginOtpRequestDto();
            var response = new LoginOtpResponseDto { token = "valid_token" };

            _mockLoginService.Setup(s => s.ValidateLoginByOtp(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.LoginOtp(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task LoginOtp_ReturnsUnauthorized_WhenResponseIsNull()
        {
            // Arrange
            var request = new LoginOtpRequestDto();

            _mockLoginService.Setup(s => s.ValidateLoginByOtp(request)).ReturnsAsync((LoginOtpResponseDto?)null);

            // Act
            var result = await _controller.LoginOtp(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task LoginOtp_ReturnsUnauthorized_WhenTokenIsEmpty()
        {
            // Arrange
            var request = new LoginOtpRequestDto();
            var response = new LoginOtpResponseDto { token = "" };

            _mockLoginService.Setup(s => s.ValidateLoginByOtp(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.LoginOtp(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task SendOtp_ReturnsOk_WhenNoExceptionThrown()
        {
            // Arrange
            var request = new SendOtpRequestDto { Email = "test@example.com" };

            _mockLoginService.Setup(s => s.SendOtp(request.Email!)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SendOtp(request);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task SendOtp_Returns500_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new SendOtpRequestDto { Email = "test@example.com" };

            _mockLoginService.Setup(s => s.SendOtp(It.IsAny<string>()))
                             .ThrowsAsync(new Exception("SMTP failure"));

            // Act
            var result = await _controller.SendOtp(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Contains("Erro ao enviar e-mail", statusCodeResult.Value!.ToString());
        }
    }
}
