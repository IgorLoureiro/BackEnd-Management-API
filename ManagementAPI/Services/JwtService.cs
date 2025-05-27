using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ManagementAPI.Interfaces;

namespace ManagementAPI.Services;

public class JwtService : IJwtService
{
    private readonly string _secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT Secret não configurado.");
    private readonly string _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT Issuer não configurado.");
    private readonly string _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT Audience não configurado.");
    private readonly double _expirationMinutes = double.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE") ?? "30");

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: creds
        );

        var bearerToken = new JwtSecurityTokenHandler().WriteToken(token);
        return bearerToken;
    }
}

