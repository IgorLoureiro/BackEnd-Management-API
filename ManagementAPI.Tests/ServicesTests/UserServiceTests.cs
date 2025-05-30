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
            var user = new UserTable { Id = 1, Username = "Franciely", Email = "fran@email.com", Role = "admin" };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _service.GetUserByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Franciely", result!.Username);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserNotExists()
        {
            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((UserTable?)null);

            var result = await _service.GetUserByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetListUserAsync_ShouldReturnListOfUsers()
        {
            var users = new List<UserTable>
            {
                new UserTable { Id = 1, Username = "A", Email = "a@email.com" },
                new UserTable { Id = 2, Username = "B", Email = "b@email.com" }
            };
            _repositoryMock.Setup(r => r.GetUserListAsync(2, 1)).ReturnsAsync(users);

            var result = await _service.GetListUserAsync(2, 1);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnUsernameAlreadyExists()
        {
            _repositoryMock.Setup(r => r.GetUserByNameAsync("Franciely")).ReturnsAsync(new UserTable());

            var result = await _service.CreateUserAsync(new CreateUserRequestDto
            {
                Username = "Franciely",
                Email = "nova@email.com",
                Password = "123456",
                Role = "user"
            });

            Assert.Equal(UserServiceResult.UsernameAlreadyExists, result);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnEmailAlreadyExists()
        {
            _repositoryMock.Setup(r => r.GetUserByNameAsync("Nova")).ReturnsAsync((UserTable?)null);
            _repositoryMock.Setup(r => r.GetUserByEmailAsync("fran@email.com")).ReturnsAsync(new UserTable());

            var result = await _service.CreateUserAsync(new CreateUserRequestDto
            {
                Username = "Nova",
                Email = "fran@email.com",
                Password = "123456",
                Role = "user"
            });

            Assert.Equal(UserServiceResult.EmailAlreadyExists, result);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuccess()
        {
            _repositoryMock.Setup(r => r.GetUserByNameAsync("Nova")).ReturnsAsync((UserTable?)null);
            _repositoryMock.Setup(r => r.GetUserByEmailAsync("nova@email.com")).ReturnsAsync((UserTable?)null);
            _repositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<UserTable>())).ReturnsAsync(new UserTable());

            var result = await _service.CreateUserAsync(new CreateUserRequestDto
            {
                Username = "Nova",
                Email = "nova@email.com",
                Password = "123456",
                Role = "user"
            });

            Assert.Equal(UserServiceResult.Success, result);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateAndReturnUserResponseDto()
        {
            var original = new UserTable
            {
                Id = 1,
                Username = "Franciely",
                Email = "old@email.com",
                Password = PasswordEncryptionHelper.HashPassword("123456"), // ✅ Corrigido
                Role = "user"
            };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(original);
            _repositoryMock.Setup(r => r.UpdateUser(It.IsAny<UserTable>())).ReturnsAsync(original);

            var result = await _service.UpdateUserAsync(1, new UpdateUserRequestDto
            {
                Email = "new@email.com",
                Username = "FranNova",
                Password = "novaSenha",
                Role = "admin"
            });

            Assert.NotNull(result);
            Assert.Equal("FranNova", result!.Username);
            Assert.Equal("new@email.com", result.Email);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldDeleteAndReturnUserResponseDto()
        {
            var user = new UserTable
            {
                Id = 1,
                Username = "Franciely",
                Email = "fran@email.com",
                Role = "admin"
            };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.DeleteUser(user)).ReturnsAsync(user);

            var result = await _service.DeleteUserByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Franciely", result!.Username);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnNull_WhenUserNotFound()
        {
            _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((UserTable?)null);

            var result = await _service.DeleteUserByIdAsync(1);

            Assert.Null(result);
        }
    }
}
