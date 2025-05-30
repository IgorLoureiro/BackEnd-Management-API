using ManagementAPI.Models;
using ManagementAPI.Repository;
using Microsoft.EntityFrameworkCore;
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
        public async Task CreateUserAsync_ShouldAddUser()
        {
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Franciely",
                Email = "franciely@email.com",
                Password = "senha123"
            };

            var result = await repository.CreateUserAsync(user);

            Assert.NotNull(result);
            Assert.Equal("Franciely", result.Username);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnCorrectUser()
        {
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Teste",
                Email = "teste@email.com",
                Password = "123"
            };

            await repository.CreateUserAsync(user);

            var result = await repository.GetUserByEmailAsync("teste@email.com");

            Assert.NotNull(result);
            Assert.Equal("Teste", result?.Username);
        }

        [Fact]
        public async Task DeleteUser_ShouldRemoveUser()
        {
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Deletar",
                Email = "del@email.com",
                Password = "pass"
            };

            var created = await repository.CreateUserAsync(user);
            var deleted = await repository.DeleteUser(created);
            var result = await repository.GetUserByIdAsync(created.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnCorrectUser()
        {
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Joana",
                Email = "joana@email.com",
                Password = "senha"
            };

            var created = await repository.CreateUserAsync(user);

            var result = await repository.GetUserByIdAsync(created.Id);

            Assert.NotNull(result);
            Assert.Equal("Joana", result?.Username);
        }

        [Fact]
        public async Task GetUserByNameAsync_ShouldReturnCorrectUser()
        {
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Carlos",
                Email = "carlos@email.com",
                Password = "abc"
            };

            await repository.CreateUserAsync(user);

            var result = await repository.GetUserByNameAsync("Carlos");

            Assert.NotNull(result);
            Assert.Equal("Carlos", result?.Username);
        }

        [Fact]
        public async Task GetUserListAsync_ShouldReturnPaginatedUsers()
        {
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            for (int i = 1; i <= 10; i++)
            {
                await repository.CreateUserAsync(new UserTable
                {
                    Username = $"User{i}",
                    Email = $"user{i}@email.com",
                    Password = "123"
                });
            }

            var result = await repository.GetUserListAsync(limit: 5, page: 2);

            Assert.Equal(5, result.Count);
            Assert.Equal("User6", result[0].Username);
        }

        [Fact]
        public async Task UpdateUser_ShouldModifyUser()
        {
            var dbContext = CreateInMemoryDbContext();
            var repository = new DefaultUserRepository(dbContext);

            var user = new UserTable
            {
                Username = "Maria",
                Email = "maria@email.com",
                Password = "pass"
            };

            var created = await repository.CreateUserAsync(user);

            created.Username = "MariaUpdated";

            var updated = await repository.UpdateUser(created);

            Assert.NotNull(updated);
            Assert.Equal("MariaUpdated", updated?.Username);
        }
    }
}
