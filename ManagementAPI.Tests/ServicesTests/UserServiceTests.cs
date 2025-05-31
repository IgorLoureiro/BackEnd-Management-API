using ManagementAPI.DTO;
using ManagementAPI.Enums;
using ManagementAPI.Helpers;
using ManagementAPI.Interfaces;
using ManagementAPI.Models;
using ManagementAPI.Services;
using Moq;

namespace ManagementAPI.Tests.ManagementAPI.Tests.ServicesTests
{
    public class UserServiceTests
    {
        private readonly Mock<IDefaultUserRepository> _repositoryMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IDefaultUserRepository>();
            _service = new UserService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUserResponseDto_WhenUserExists()
        {
            // Arrange
            var user = new UserTable { Id = 1, Username = "TestUser", Email = "testuser@example.com", Role = "user" };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestUser", result!.Username);
            Assert.Equal(1, result.Id);
            Assert.Equal("testuser@example.com", result.Email);
            Assert.Equal("user", result.Role);
            _repositoryMock.Verify(r => r.GetUserByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserNotExists()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((UserTable?)null);

            // Act
            var result = await _service.GetUserByIdAsync(1);

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(r => r.GetUserByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetListUserAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<UserTable>
    {
        new UserTable { Id = 1, Username = "UserA", Email = "usera@example.com" },
        new UserTable { Id = 2, Username = "UserB", Email = "userb@example.com" }
    };
            _repositoryMock.Setup(r => r.GetUserListAsync(2, 1)).ReturnsAsync(users);

            // Act
            var result = await _service.GetListUserAsync(2, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Contains(result, u => u.Id == 1 && u.Username == "UserA" && u.Email == "usera@example.com");
            Assert.Contains(result, u => u.Id == 2 && u.Username == "UserB" && u.Email == "userb@example.com");

            _repositoryMock.Verify(r => r.GetUserListAsync(2, 1), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnUsernameAlreadyExists()
        {
            // Arrange
            var existingUsername = "ExistingUser";
            _repositoryMock.Setup(r => r.GetUserByNameAsync(existingUsername)).ReturnsAsync(new UserTable());

            // Act
            var result = await _service.CreateUserAsync(new CreateUserRequestDto
            {
                Username = existingUsername,
                Email = "newemail@example.com",
                Password = "password123",
                Role = "user"
            });

            // Assert
            Assert.Equal(UserServiceResult.UsernameAlreadyExists, result);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnEmailAlreadyExists()
        {
            // Arrange
            var newUsername = "NewUser";
            var existingEmail = "existingemail@example.com";

            _repositoryMock.Setup(r => r.GetUserByNameAsync(newUsername)).ReturnsAsync((UserTable?)null);
            _repositoryMock.Setup(r => r.GetUserByEmailAsync(existingEmail)).ReturnsAsync(new UserTable());

            // Act
            var result = await _service.CreateUserAsync(new CreateUserRequestDto
            {
                Username = newUsername,
                Email = existingEmail,
                Password = "password123",
                Role = "user"
            });

            // Assert
            Assert.Equal(UserServiceResult.EmailAlreadyExists, result);

            _repositoryMock.Verify(r => r.GetUserByNameAsync(newUsername), Times.Once);
            _repositoryMock.Verify(r => r.GetUserByEmailAsync(existingEmail), Times.Once);
            _repositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<UserTable>()), Times.Never); 
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuccess()
        {
            // Arrange
            var newUsername = "NewUser";
            var newEmail = "newemail@example.com";

            _repositoryMock.Setup(r => r.GetUserByNameAsync(newUsername)).ReturnsAsync((UserTable?)null);
            _repositoryMock.Setup(r => r.GetUserByEmailAsync(newEmail)).ReturnsAsync((UserTable?)null);
            _repositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<UserTable>())).ReturnsAsync(new UserTable());

            // Act
            var result = await _service.CreateUserAsync(new CreateUserRequestDto
            {
                Username = newUsername,
                Email = newEmail,
                Password = "password123",
                Role = "user"
            });

            // Assert
            Assert.Equal(UserServiceResult.Success, result);

            // Verifica se os métodos foram chamados uma vez
            _repositoryMock.Verify(r => r.GetUserByNameAsync(newUsername), Times.Once);
            _repositoryMock.Verify(r => r.GetUserByEmailAsync(newEmail), Times.Once);
            _repositoryMock.Verify(r => r.CreateUserAsync(It.Is<UserTable>(u =>
                u.Username == newUsername &&
                u.Email == newEmail)), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateAndReturnUserResponseDto()
        {
            // Arrange
            var original = new UserTable
            {
                Id = 1,
                Username = "OriginalUser",
                Email = "oldemail@example.com",
                Password = PasswordEncryptionHelper.HashPassword("oldpassword"),
                Role = "user"
            };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(original);
            _repositoryMock.Setup(r => r.UpdateUser(It.IsAny<UserTable>())).ReturnsAsync(original);

            // Act
            var result = await _service.UpdateUserAsync(1, new UpdateUserRequestDto
            {
                Email = "newemail@example.com",
                Username = "UpdatedUser",
                Password = "newpassword",
                Role = "admin"
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedUser", result!.Username);
            Assert.Equal("newemail@example.com", result.Email);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldDeleteAndReturnUserResponseDto()
        {
            // Arrange
            var user = new UserTable
            {
                Id = 1,
                Username = "UserToDelete",
                Email = "userdelete@example.com",
                Role = "admin"
            };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.DeleteUser(user)).ReturnsAsync(user);

            // Act
            var result = await _service.DeleteUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserToDelete", result!.Username);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            int userId = 1;
            _repositoryMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync((UserTable?)null);

            // Act
            var result = await _service.DeleteUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteUser(It.IsAny<UserTable>()), Times.Never);
        }
    }
}
