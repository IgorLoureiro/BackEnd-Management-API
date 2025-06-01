using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ManagementAPI.Interfaces;
using ManagementAPI.Services;
using Microsoft.IdentityModel.Tokens;

namespace ManagementAPI.Tests.JwtServiceTests
{
    public class JwtServiceTests
    {
        public JwtServiceTests()
        {
            // Set up environment variables required by JwtService
            Environment.SetEnvironmentVariable("JWT_SECRET", "super_secret_key_for_testing_123!");
            Environment.SetEnvironmentVariable("JWT_ISSUER", "TestIssuer");
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "TestAudience");
            Environment.SetEnvironmentVariable("JWT_EXPIRE", "60");
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken_WhenClaimsAreProvided()
        {
            // Arrange
            IJwtService jwtService = new JwtService();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Email, "user@example.com")
            };

            // Act
            var token = jwtService.GenerateToken(claims);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));

            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = "TestIssuer",
                ValidAudience = "TestAudience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_secret_key_for_testing_123!")),
                ClockSkew = TimeSpan.Zero
            };

            handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            Assert.NotNull(validatedToken);
            Assert.IsType<JwtSecurityToken>(validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Equal("TestAudience", jwtToken.Audiences.First());
        }

        [Fact]
        public void GenerateToken_ShouldThrowException_WhenEnvVariablesAreMissing()
        {
            // Arrange
            Environment.SetEnvironmentVariable("JWT_SECRET", null);
            Environment.SetEnvironmentVariable("JWT_ISSUER", null);
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", null);
            Environment.SetEnvironmentVariable("JWT_EXPIRE", null);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new JwtService());
            Assert.Equal("JWT Secret não configurado.", exception.Message);
        }
    }
}
