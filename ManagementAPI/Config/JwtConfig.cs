
namespace ManagementAPI.Settings
{
    public class JwtConfig
    {
        public required string Secret { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required double ExpireMinutes { get; init; }

        public static JwtConfig FromEnvironment()
        {
            return new JwtConfig
            {
                Secret = Environment.GetEnvironmentVariable("JWT_SECRET")
                         ?? throw new Exception("JWT_SECRET não configurado."),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                         ?? throw new Exception("JWT_ISSUER não configurado."),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                         ?? throw new Exception("JWT_AUDIENCE não configurado."),
                ExpireMinutes = double.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE") ?? "30")
            };
        }
    }
}
