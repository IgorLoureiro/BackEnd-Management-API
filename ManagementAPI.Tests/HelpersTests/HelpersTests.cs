using Xunit;
using Microsoft.AspNetCore.Mvc;
using ManagementAPI.DTO;
using ManagementAPI.Enums;
using ManagementAPI.Helpers;
using ManagementAPI.Models;
using ManagementAPI.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ManagementAPI.Tests.HelpersTests
{
    public class ErrorResponseMappingHelperTests
    {
        [Fact]
        public void Create_WithSingleError_ShouldReturnProperDto()
        {
            // Arrange
            int status = 400;
            string field = "field";
            string message = "error message";

            // Act
            var result = ErrorResponseMappingHelper.Create(status, field, message);

            // Assert
            Assert.Equal(400, result.Status);
            Assert.True(result.Errors.ContainsKey("field"));
            Assert.Single(result.Errors["field"]);
            Assert.Equal("error message", result.Errors["field"][0]);
        }

        [Fact]
        public void Create_WithMultipleErrors_ShouldReturnProperDto()
        {
            // Arrange
            var errors = new Dictionary<string, List<string>>
            {
                { "field1", new List<string> { "error 1" } },
                { "field2", new List<string> { "error 2", "error 3" } }
            };

            // Act
            var result = ErrorResponseMappingHelper.Create(422, errors);

            // Assert
            Assert.Equal(422, result.Status);
            Assert.Equal(2, result.Errors.Count);
            Assert.Equal("error 2", result.Errors["field2"][0]);
        }
    }

    public class PasswordEncryptionHelperTests
    {
        [Fact]
        public void HashPassword_And_VerifyPassword_ShouldSucceed()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var hash = PasswordEncryptionHelper.HashPassword(password);
            var isValid = PasswordEncryptionHelper.VerifyPassword(password, hash);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_WithWrongPassword_ShouldFail()
        {
            // Arrange
            var password = "TestPassword123!";
            var wrongPassword = "WrongPassword";
            var hash = PasswordEncryptionHelper.HashPassword(password);

            // Act
            var isValid = PasswordEncryptionHelper.VerifyPassword(wrongPassword, hash);

            // Assert
            Assert.False(isValid);
        }
    }

    public class UserControllerHelperTests
    {
        [Theory]
        [InlineData(UserServiceResult.Success, 201)]
        [InlineData(UserServiceResult.InvalidUser, 400)]
        [InlineData(UserServiceResult.UsernameAlreadyExists, 409)]
        [InlineData(UserServiceResult.EmailAlreadyExists, 409)]
        public void ToActionResult_ShouldReturnExpectedStatus(UserServiceResult result, int expectedStatus)
        {
            // Arrange & Act
            var actionResult = result.ToActionResult();
            var statusCode = (actionResult as ObjectResult)?.StatusCode ?? (actionResult as StatusCodeResult)?.StatusCode;

            // Assert
            Assert.Equal(expectedStatus, statusCode);
        }
    }

    public class UserServiceErrorResultMapperTests
    {
        [Theory]
        [InlineData(UserServiceResult.Success, 201)]
        [InlineData(UserServiceResult.UsernameAlreadyExists, 409)]
        [InlineData(UserServiceResult.EmailAlreadyExists, 409)]
        [InlineData(UserServiceResult.GenerationFailed, 400)]
        [InlineData(UserServiceResult.CreationFailed, 400)]
        public void ToActionResult_ShouldMapCorrectStatusCode(UserServiceResult result, int expectedStatus)
        {
            // Arrange & Act
            var actionResult = UserServiceErrorResultMapper.ToActionResult(result);
            var statusCode = (actionResult as ObjectResult)?.StatusCode ?? (actionResult as StatusCodeResult)?.StatusCode;

            // Assert
            Assert.Equal(expectedStatus, statusCode);
        }
    }
}
