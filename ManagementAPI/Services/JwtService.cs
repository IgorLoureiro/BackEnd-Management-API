using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManagementAPI.Services
{


    public class JwtService() : IJwtService
    {
        private readonly string _secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new Exception("JWT Secret não configurado.");
        private readonly string _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("JWT Issuer não configurado.");
        private readonly string _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("JWT Audience não configurado.");
        private readonly double _expirationMinutes = double.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE") ?? "30");

        public string GerarToken(IEnumerable<Claim> claims)
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}

