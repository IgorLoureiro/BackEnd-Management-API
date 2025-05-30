using ManagementAPI.Models;
using ManagementAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using AppDbContext = ManagementAPI.Context.DbContext;

namespace ManagementAPI.Tests.RepositoryTests
{
    public class UserRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUser_WhenUserIsValid()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var username = "UserTest";
            var email = "usertest@email.com";
            var password = "senha123";

            var user = new UserTable
            {
                Username = username,
                Email = email,
                Password = password
            };

            // Act
            var result = await repository.CreateUserAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
            Assert.Equal(email, result.Email);
            Assert.Equal(password, result.Password);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnCorrectUser_WhenEmailExists()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var username = "TestUser";
            var email = "testuser@email.com";
            var password = "123";

            var user = new UserTable
            {
                Username = username,
                Email = email,
                Password = password
            };

            dbContext.Set<UserTable>().Add(user);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetUserByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result?.Username);
        }

        [Fact]
        public async Task DeleteUser_ShouldRemoveUser_WhenUserExists()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var username = "Deletar";
            var email = "del@email.com";
            var password = "pass";

            var user = new UserTable
            {
                Username = username,
                Email = email,
                Password = password
            };

            var created = await repository.CreateUserAsync(user);

            // Act
            var deleted = await repository.DeleteUser(created);
            var result = await repository.GetUserByIdAsync(created.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnCorrectUser_WhenUserExists()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var username = "Joana";
            var email = "joana@email.com";
            var password = "senha";

            var user = new UserTable
            {
                Username = username,
                Email = email,
                Password = password
            };

            var created = await repository.CreateUserAsync(user);

            // Act
            var result = await repository.GetUserByIdAsync(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result?.Username);
        }

        [Fact]
        public async Task GetUserByNameAsync_ShouldReturnCorrectUser_WhenUserExists()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var username = "Carlos";
            var email = "carlos@email.com";
            var password = "abc";

            var user = new UserTable
            {
                Username = username,
                Email = email,
                Password = password
            };

            await repository.CreateUserAsync(user);

            // Act
            var result = await repository.GetUserByNameAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result?.Username);
        }

        [Fact]
        public async Task GetUserListAsync_ShouldReturnPaginatedUsers_WhenPageAndLimitProvided()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var users = Enumerable.Range(1, 10).Select(i => new UserTable
            {
                Username = $"User{i}",
                Email = $"user{i}@email.com",
                Password = "123"
            });

            dbContext.Set<UserTable>().AddRange(users);
            await dbContext.SaveChangesAsync();

            var page = 2;
            var limit = 5;

            // Act
            var result = await repository.GetUserListAsync(limit: limit, page: page);

            // Assert
            Assert.Equal(limit, result.Count);
            Assert.Equal("User6", result[0].Username);
        }



        [Fact]
        public async Task UpdateUser_ShouldModifyUser_WhenUserExists_Mocked()
        {
            var mockRepository = new Mock<DefaultUserRepository>(MockBehavior.Strict);

            var originalUser = new UserTable
            {
                Username = "Maria",
                Email = "maria@email.com",
                Password = "pass"
            };

            var updatedUsername = "MariaUpdated";

            var updatedUser = new UserTable
            {
                Username = updatedUsername,
                Email = originalUser.Email,
                Password = originalUser.Password
            };

            mockRepository
                .Setup(r => r.UpdateUser(It.Is<UserTable>(u => u.Username == originalUser.Username)))
                .ReturnsAsync(updatedUser);

            var result = await mockRepository.Object.UpdateUser(originalUser);

            Assert.NotNull(result);
            Assert.Equal(updatedUsername, result.Username);

            mockRepository.Verify(r => r.UpdateUser(It.IsAny<UserTable>()), Times.Once);
        }

    }
}
