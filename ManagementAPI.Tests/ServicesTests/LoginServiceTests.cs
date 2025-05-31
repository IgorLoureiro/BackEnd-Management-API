using ManagementAPI.DTO;
using ManagementAPI.Enums;
using ManagementAPI.Helpers;
using ManagementAPI.Interfaces;
using ManagementAPI.Models;
using ManagementAPI.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IDefaultUserRepository> _repositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IDefaultUserRepository>();
        _userService = new UserService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
    {
        var user = new UserTable { Id = 1, Username = "test", Email = "test@test.com", Role = "Admin" };
        _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("test", result.Username);
        Assert.Equal("Admin", result.Role);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUser_WhenFound()
    {
        var user = new UserTable { Id = 1, Username = "test", Email = "test@test.com", Role = "Admin" };
        _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(user.Username, result.Username);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((UserTable?)null);

        var result = await _userService.GetUserByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        _repositoryMock.Setup(r => r.GetUserByIdAsync(999)).ReturnsAsync((UserTable)null);

        var result = await _userService.GetUserByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetListUserAsync_ReturnsList()
    {
        var users = new List<UserTable>
        {
            new UserTable { Id = 1, Username = "user1", Email = "user1@mail.com" },
            new UserTable { Id = 2, Username = "user2", Email = "user2@mail.com" }
        };
        _repositoryMock.Setup(r => r.GetUserListAsync(10, 1)).ReturnsAsync(users);

        var result = await _userService.GetListUserAsync(10, 1);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Username == "user1");
    }

    [Fact]
    public async Task GetListUserAsync_ReturnsMappedUsers()
    {
        var users = new List<UserTable>
        {
            new UserTable { Id = 1, Username = "user1", Email = "user1@mail.com" },
            new UserTable { Id = 2, Username = "user2", Email = "user2@mail.com" }
        };

        _repositoryMock.Setup(r => r.GetUserListAsync(10, 1)).ReturnsAsync(users);

        var result = await _userService.GetListUserAsync(10, 1);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsUsernameAlreadyExists_WhenUsernameExists()
    {
        var dto = new CreateUserRequestDto { Username = "existing", Email = "new@mail.com", Password = "123456", Role = "User" };
        _repositoryMock.Setup(r => r.GetUserByNameAsync("existing")).ReturnsAsync(new UserTable());

        var result = await _userService.CreateUserAsync(dto);

        Assert.Equal(UserServiceResult.UsernameAlreadyExists, result);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsEmailAlreadyExists_WhenEmailExists()
    {
        var dto = new CreateUserRequestDto { Username = "newuser", Email = "exists@mail.com", Password = "123456", Role = "User" };
        _repositoryMock.Setup(r => r.GetUserByNameAsync("newuser")).ReturnsAsync((UserTable)null);
        _repositoryMock.Setup(r => r.GetUserByEmailAsync("exists@mail.com")).ReturnsAsync(new UserTable());

        var result = await _userService.CreateUserAsync(dto);

        Assert.Equal(UserServiceResult.EmailAlreadyExists, result);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsSuccess_WhenUserIsCreated()
    {
        var dto = new CreateUserRequestDto { Username = "newuser", Email = "new@mail.com", Password = "123456", Role = "User" };

        _repositoryMock.Setup(r => r.GetUserByNameAsync(dto.Username)).ReturnsAsync((UserTable)null);
        _repositoryMock.Setup(r => r.GetUserByEmailAsync(dto.Email)).ReturnsAsync((UserTable)null);
        _repositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<UserTable>())).ReturnsAsync(new UserTable());

        var result = await _userService.CreateUserAsync(dto);

        Assert.Equal(UserServiceResult.Success, result);
    }

    [Fact]
    public async Task UpdateUserAsync_ReturnsUpdatedUser_WhenSuccessful()
    {
        // Arrange
        var originalUser = new UserTable
        {
            Id = 1,
            Username = "old",
            Email = "old@mail.com",
            Password = PasswordEncryptionHelper.HashPassword("oldpass"),
            Role = "User"
        };

        var updateDto = new UpdateUserRequestDto
        {
            Username = "new",
            Email = "new@mail.com",
            Password = "newpass",
            Role = "Admin"
        };

        var updatedUser = new UserTable
        {
            Id = 1,
            Username = "new",
            Email = "new@mail.com",
            Password = PasswordEncryptionHelper.HashPassword("newpass"),
            Role = "Admin"
        };

        _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(originalUser);
        _repositoryMock.Setup(r => r.UpdateUser(It.IsAny<UserTable>())).ReturnsAsync(updatedUser);

        // Act
        var result = await _userService.UpdateUserAsync(1, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("new", result.Username);
        Assert.Equal("new@mail.com", result.Email);
        Assert.Equal("Admin", result.Role);
    }


    [Fact]
    public async Task UpdateUserAsync_ReturnsNull_WhenUserNotFound()
    {
        _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((UserTable)null);

        var result = await _userService.UpdateUserAsync(1, new UpdateUserRequestDto());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteUserByIdAsync_ReturnsDeletedUser_WhenFound()
    {
        var user = new UserTable { Id = 1, Username = "toDelete", Email = "del@mail.com", Role = "User" };

        _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.DeleteUser(user)).Returns(Task.FromResult(user));

        var result = await _userService.DeleteUserByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("toDelete", result.Username);
    }

    [Fact]
    public async Task DeleteUserByIdAsync_ReturnsNull_WhenUserNotFound()
    {
        _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((UserTable)null);

        var result = await _userService.DeleteUserByIdAsync(1);

        Assert.Null(result);
    }
}
