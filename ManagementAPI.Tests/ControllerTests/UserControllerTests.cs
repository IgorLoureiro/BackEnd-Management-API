﻿using ManagementAPI.Controller;
using ManagementAPI.DTO;
using ManagementAPI.Enums;
using ManagementAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;

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
        public async Task CreateUser_WhenUserIsValid_ShouldReturnCreated()
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
        public async Task GetUsersList_WhenCalled_ShouldReturnOkWithUserList()
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
        public async Task GetUser_WhenUserExists_ShouldReturnOkWithUser()
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
        public async Task GetUser_WhenUserDoesNotExist_ShouldReturnNotFound()
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
        public async Task UpdateUser_WhenUserExists_ShouldReturnOkWithUpdatedUser()
        {
            // Arrange
            var newEmail = "new@email.com";
            var updateDto = new UpdateUserRequestDto { Email = newEmail, Role = "user" };
            var updatedUser = new UserResponseDto { Id = 1, Username = "user1", Email = newEmail, Role = "user" };

            _mockUserService
                .Setup(s => s.UpdateUserAsync(1, updateDto))
                .ReturnsAsync(updatedUser);

            // Act
            var result = await _controller.UpdateUser(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal(newEmail, returnedUser.Email);
        }

        [Fact]
        public async Task UpdateUser_WhenUserDoesNotExist_ShouldReturnNotFound()
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
        public async Task DeleteUser_WhenUserExists_ShouldReturnOk()
        {
            // Arrange
            var userId = 1;
            var deletedUser = new UserResponseDto { Id = userId, Username = "deleted" };

            _mockUserService
                .Setup(s => s.DeleteUserByIdAsync(userId))
                .ReturnsAsync(deletedUser);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockUserService.Verify(s => s.DeleteUserByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_WhenUserDoesNotExist_ShouldReturnNotFound()
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

        [Fact]
        public async Task GetUsersList_WhenCalledByAuthorizedUser_ShouldReturnOkWithUserList()
        {
            // Arrange
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = 1, Username = "user1", Email = "user1@email.com", Role = "admin" }
            };

            _mockUserService
                .Setup(s => s.GetListUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(users);

            //generates an Admin type of user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "admin")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.GetUsersList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(okResult.Value);
            Assert.Single(returnedUsers);
        }
    }
}
