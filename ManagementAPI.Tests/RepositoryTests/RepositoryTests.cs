using ManagementAPI.Models;
using ManagementAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace ManagementAPI.Tests.RepositoryTests
{
    public class UserRepositoryTests
    {
        private ManagementAPI.Context.DbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ManagementAPI.Context.DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ManagementAPI.Context.DbContext(options);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUser()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Franciely",
                Email = "franciely@email.com",
                Password = "senha123"
            };

            // Act
            var result = await repository.CreateUserAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Franciely", result.Username);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Teste",
                Email = "teste@email.com",
                Password = "123"
            };

            await repository.CreateUserAsync(user);

            // Act
            var result = await repository.GetUserByEmailAsync("teste@email.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Teste", result?.Username);
        }

        [Fact]
        public async Task DeleteUser_ShouldRemoveUser()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Deletar",
                Email = "del@email.com",
                Password = "pass"
            };

            var created = await repository.CreateUserAsync(user);

            // Act
            var deleted = await repository.DeleteUser(created);
            var result = await repository.GetUserByIdAsync(created.Id);

            // Assert
            Assert.Null(result);
        }
    }
}
