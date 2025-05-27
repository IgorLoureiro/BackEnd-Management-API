using System;
using System.Threading.Tasks;
using ManagementAPI.Controller;
using ManagementAPI.DTO;
using ManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ManagementAPI.Tests.ControllerTests
{
    public class ControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public ControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnOk_WhenUserIsCreated()
        {
            var userRequest = new DefaultUserResponse
            {
                Username = "test",
                Email = "test@email.com",
                Password = "123"
            };

            var serviceResult = UserServiceResult.Success;

            _mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<DefaultUserResponse>()))
                .ReturnsAsync(serviceResult);

            var result = await _userController.CreateUser(userRequest);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetUser_ShouldReturnOk_WhenUserExists()
        {
            var user = new DefaultUserResponse { Username = "found", Email = "found@email.com" };
            _mockUserService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _userController.GetUser(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            _mockUserService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((DefaultUserResponse)null);

            var result = await _userController.GetUser(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdated()
        {
            var updatedUser = new DefaultUserResponse { Username = "updated", Email = "updated@email.com" };
            var request = new DefaultUserRequest { Username = "updated", Email = "updated@email.com" };

            _mockUserService.Setup(s => s.UpdateUserAsync(1, It.IsAny<DefaultUserRequest>())).ReturnsAsync(updatedUser);

            var result = await _userController.UpdateUser(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedUser, okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserNotExists()
        {
            var request = new DefaultUserRequest { Username = "any", Email = "any@email.com" };

            _mockUserService.Setup(s => s.UpdateUserAsync(1, It.IsAny<DefaultUserRequest>())).ReturnsAsync((DefaultUserResponse)null);

            var result = await _userController.UpdateUser(1, request);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnOk_WhenUserIsDeleted()
        {
            var deletedUser = new DefaultUserResponse { Username = "deleted", Email = "del@email.com" };

            _mockUserService.Setup(s => s.DeleteUserByIdAsync(1)).ReturnsAsync(deletedUser);

            var result = await _userController.DeleteUser(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(deletedUser, okResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserNotExists()
        {
            _mockUserService.Setup(s => s.DeleteUserByIdAsync(1)).ReturnsAsync((DefaultUserResponse)null);

            var result = await _userController.DeleteUser(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
