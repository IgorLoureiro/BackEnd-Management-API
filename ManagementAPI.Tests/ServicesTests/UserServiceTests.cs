using System.Threading.Tasks;
using ManagementAPI.DTO;        
using ManagementAPI.Models;
using ManagementAPI.Repository;
using ManagementAPI.Services;
using Moq;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IDefaultUserRepository> _mockRepo;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IDefaultUserRepository>();
        _userService = new UserService(_mockRepo.Object);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnNull_WhenDefaultUserIsNull()
    {
        // Act
        var result = await _userService.UpdateUserAsync(1, null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((UserTable)null);

        // Act
        var result = await _userService.UpdateUserAsync(1, new DefaultUserRequest { Username = "newUser" });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnUpdatedUser_WhenUpdateSuccessful()
    {
        // Arrange
        var id = 1;

        var oldPasswordHash = BCrypt.Net.BCrypt.HashPassword("oldPassword");

        var existingUser = new UserTable
        {
            Id = id,
            Username = "oldUser",
            Email = "old@email.com",
            Password = oldPasswordHash
        };

        var updateData = new DefaultUserRequest
        {
            Username = "newUser",
            Email = "new@email.com",
            Password = "newPassword"
        };

        _mockRepo.Setup(r => r.GetUserByIdAsync(id))
                 .ReturnsAsync(existingUser);

        _mockRepo.Setup(r => r.UpdateUser(It.IsAny<UserTable>()))
                 .ReturnsAsync((UserTable updatedUser) => updatedUser);

        // Act
        var result = await _userService.UpdateUserAsync(id, updateData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateData.Username, result.Username);
        Assert.Equal(updateData.Email, result.Email);
        Assert.NotEqual(existingUser.Password, result.Password);
    }

    [Fact]
    public async Task DeleteUserByIdAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((UserTable)null);

        // Act
        var result = await _userService.DeleteUserByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteUserByIdAsync_ShouldReturnDeletedUser_WhenDeleteSuccessful()
    {
        // Arrange
        var existingUser = new UserTable
        {
            Id = 1,
            Username = "userToDelete",
            Email = "delete@email.com",
            Password = "hash"
        };

        _mockRepo.Setup(r => r.GetUserByIdAsync(existingUser.Id))
                 .ReturnsAsync(existingUser);

        // Act
        var result = await _userService.DeleteUserByIdAsync(existingUser.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingUser.Id, result.Id);
        Assert.Equal(existingUser.Username, result.Username);
    }
}
