using System.Security.Claims;

namespace ManagementAPI.Services;

public interface IJwtService
{
    string GerarToken(IEnumerable<Claim> claims);
}
