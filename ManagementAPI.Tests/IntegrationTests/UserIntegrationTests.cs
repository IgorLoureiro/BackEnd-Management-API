using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ManagementAPI.Tests.RepositoryTests
{
    public class UserIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, AuthHandlerTests>("Test", options => { });

                    services.PostConfigure<AuthenticationOptions>(options =>
                    {
                        options.DefaultAuthenticateScheme = "Test";
                        options.DefaultChallengeScheme = "Test";
                    });
                });
            }).CreateClient();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        [Fact]
        public async Task CreateUser_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var newUser = new
            {
                Name = "Maria Teste",
                Email = "maria@email.com",
                Password = "123456"
            };
            var content = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/user", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetUsersList_DefaultParams_ReturnsOk()
        {
            // Arrange
            // no data required, using default (limit = 10, page = 1)

            // Act
            var response = await _client.GetAsync("/user");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUser_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 99999;

            // Act
            var response = await _client.GetAsync($"/user/{nonExistentId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new
            {
                Name = "Novo Nome",
                Email = "novo@email.com"
            };
            var content = new StringContent(JsonSerializer.Serialize(updateDto), Encoding.UTF8, "application/json");
            int nonExistentId = 99999;

            // Act
            var response = await _client.PutAsync($"/user/{nonExistentId}", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 99999;

            // Act
            var response = await _client.DeleteAsync($"/user/{nonExistentId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}