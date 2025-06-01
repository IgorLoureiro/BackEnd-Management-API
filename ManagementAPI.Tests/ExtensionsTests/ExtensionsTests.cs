using System;
using Microsoft.Extensions.DependencyInjection;
using ManagementAPI.Extensions;
using Xunit;

namespace ManagementAPI.Tests.ServiceExtensionsTests
{
    public class ServiceExtensionsTests
    {
        [Fact]
        public void AddJwt_ConfiguresJwtAuthentication_WhenEnvironmentVariablesPresent()
        {
            // Arrange
            Environment.SetEnvironmentVariable("JWT_ISSUER", "TestIssuer");
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "TestAudience");
            Environment.SetEnvironmentVariable("JWT_SECRET", "SuperSecretKey12345");

            var services = new ServiceCollection();

            // Act
            var result = services.AddJwt();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AddJwt_ThrowsException_WhenJwtIssuerMissing()
        {
            // Arrange
            Environment.SetEnvironmentVariable("JWT_ISSUER", null);
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "some-audience");
            Environment.SetEnvironmentVariable("JWT_SECRET", "some-secret");

            var services = new ServiceCollection();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => services.AddJwt());
            Assert.Equal("JWT_ISSUER não configurado.", ex.Message);
        }

        [Fact]
        public void AddJwt_ThrowsException_WhenJwtSecretMissing()
        {
            // Arrange
            Environment.SetEnvironmentVariable("JWT_ISSUER", "some-issuer");
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "some-audience");
            Environment.SetEnvironmentVariable("JWT_SECRET", null);

            var services = new ServiceCollection();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => services.AddJwt());
            Assert.Equal("JWT_SECRET não configurado.", ex.Message);
        }
    }
}
