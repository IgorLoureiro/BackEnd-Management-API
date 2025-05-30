using Moq;
using ManagementAPI.Controller;
using ManagementAPI.Interfaces;
using ManagementAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Enums;

namespace ManagementAPI.Tests.ManagementAPI.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldReturn201Created_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var dto = new CreateUserRequestDto
            {
                Username = "test",
                Email = "test@email.com",
                Password = "123456",
                Role = "admin"
            };

            _mockUserService
                .Setup(s => s.CreateUserAsync(dto))
                .ReturnsAsync(UserServiceResult.Success);

            // Act
            var result = await _controller.CreateUser(dto);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetUsersList_ShouldReturnOkWithUserList()
        {
            // Arrange
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = 1, Username = "user1", Email = "user1@email.com", Role = "admin" }
            };

            _mockUserService
                .Setup(s => s.GetListUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsersList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(okResult.Value);
            Assert.Single(returnedUsers);
        }

        [Fact]
        public async Task GetUser_ShouldReturnOkWithUser_WhenUserExists()
        {
            // Arrange
            var user = new UserResponseDto { Id = 1, Username = "user1", Email = "email", Role = "admin" };

            _mockUserService
                .Setup(s => s.GetUserByIdAsync(1))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal(1, returnedUser.Id);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserService
                .Setup(s => s.GetUserByIdAsync(999))
                .ReturnsAsync((UserResponseDto)null);

            // Act
            var result = await _controller.GetUser(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("Not Found", notFoundResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnOkWithUpdatedUser_WhenSuccessful()
        {
            // Arrange
            var updateDto = new UpdateUserRequestDto { Email = "new@email.com", Role = "user" };
            var updatedUser = new UserResponseDto { Id = 1, Username = "user1", Email = "new@email.com", Role = "user" };

            _mockUserService
                .Setup(s => s.UpdateUserAsync(1, updateDto))
                .ReturnsAsync(updatedUser);

            // Act
            var result = await _controller.UpdateUser(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal("new@email.com", returnedUser.Email);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var updateDto = new UpdateUserRequestDto { Email = "none", Role = "none" };

            _mockUserService
                .Setup(s => s.UpdateUserAsync(99, updateDto))
                .ReturnsAsync((UserResponseDto)null);

            // Act
            var result = await _controller.UpdateUser(99, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("Not Found", notFoundResult.Value.ToString());
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var deletedUser = new UserResponseDto { Id = 1, Username = "deleted" };

            _mockUserService
                .Setup(s => s.DeleteUserByIdAsync(1))
                .ReturnsAsync(deletedUser);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserService
                .Setup(s => s.DeleteUserByIdAsync(999))
                .ReturnsAsync((UserResponseDto)null);

            // Act
            var result = await _controller.DeleteUser(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("Not Found", notFoundResult.Value.ToString());
        }
    }
}
