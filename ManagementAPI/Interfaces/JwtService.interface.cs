using System.Security.Claims;

namespace ManagementAPI.Interfaces;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims);
}
