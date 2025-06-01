using Xunit;
using ManagementAPI.SwaggerExamples;
using ManagementAPI.DTO;
using System.Collections.Generic;

namespace ManagementAPI.Tests.SwaggerExamplesTests
{
    public class SwaggerExamplesTests
    {
        [Fact]
        public void CreateUserBadRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new CreateUserBadRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal(400, example.Status);
            Assert.True(example.Errors.ContainsKey("Email"));
            Assert.Contains("The Email field is required.", example.Errors["Email"]);
        }

        [Fact]
        public void CreateUserRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new CreateUserRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal("barry_allen", example.Username);
            Assert.Equal("barry.allen@gmail.com", example.Email);
            Assert.Equal("1234", example.Password);
            Assert.Equal("user", example.Role);
        }

        [Fact]
        public void InternalServerErrorDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new InternalServerErrorDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal(500, example.status);
            Assert.Equal("An unexpected error occurred. Please try again later.", example.message);
        }

        [Fact]
        public void LoginBadRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new LoginBadRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal(400, example.Status);
            Assert.Contains("Email", example.Errors.Keys);
            Assert.Contains("The Email field is required.", example.Errors["Email"]);
        }

        [Fact]
        public void LoginOtpRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new LoginOtpRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal("barry.allen@gmail.com", example.Email);
            Assert.Equal("1234", example.Password);
        }

        [Fact]
        public void LoginOtpResponseDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new LoginOtpResponseDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.StartsWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9", example.token);
        }

        [Fact]
        public void LoginRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new LoginRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal("clark.kent@mail.com.br", example.Email);
            Assert.Equal("Abc!1234", example.Password);
        }

        [Fact]
        public void NotFoundErrorDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new NotFoundErrorDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal(404, example.status);
            Assert.Equal("Not Found", example.error);
            Assert.Contains("not found", example.message.ToLower());
        }

        [Fact]
        public void SendOtpBadRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new SendOtpBadRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal(400, example.Status);
            Assert.True(example.Errors.ContainsKey("Email"));
        }

        [Fact]
        public void SendOtpRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new SendOtpRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal("barry.allen@gmail.com", example.Email);
        }

        [Fact]
        public void UpdateUserBadRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new UpdateUserBadRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal(400, example.Status);
            Assert.Contains("Email", example.Errors.Keys);
        }

        [Fact]
        public void UpdateUserRequestDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new UpdateUserRequestDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal("barry_allen", example.Username);
            Assert.Equal("barry.allen@gmail.com", example.Email);
            Assert.Equal("1234", example.Password);
            Assert.Equal("user", example.Role);
        }

        [Fact]
        public void UserListResponseDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new UserListResponseDtoExample();

            // Act
            var exampleList = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(exampleList);
            Assert.Equal(2, exampleList.Count);
            Assert.Equal("barry_allen", exampleList[0].Username);
            Assert.Equal("steven_rogers", exampleList[1].Username);
        }

        [Fact]
        public void UserResponseDtoExample_ShouldReturnExpectedExample()
        {
            // Arrange
            var exampleProvider = new UserResponseDtoExample();

            // Act
            var example = exampleProvider.GetExamples();

            // Assert
            Assert.NotNull(example);
            Assert.Equal(1, example.Id);
            Assert.Equal("barry_allen", example.Username);
            Assert.Equal("barry.allen@gmail.com", example.Email);
            Assert.Equal("user", example.Role);
        }
    }
}
