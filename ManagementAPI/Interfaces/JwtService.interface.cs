using System.Security.Claims;

namespace ManagementAPI.Interfaces;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims);

    public IEnumerable<Claim> GenerateClaims(string sub, string email, string username, string role);
}
