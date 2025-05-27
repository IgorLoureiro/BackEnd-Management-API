using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ManagementAPI.Context;
using ManagementAPI.DTO;
using ManagementAPI.Helpers;
using ManagementAPI.Repository;


namespace ManagementAPI.Services;

public class LoginService : ILoginService
{

    private readonly string? _emailSender;
    private readonly string? _emailSenderName;
    private readonly string? _emailSenderAppPassword;
    private readonly IJwtService _jwtService;
    private readonly IDefaultUserRepository _userRepository;

    public LoginService(IJwtService jwtService, IDefaultUserRepository defaultUserRepository)
    {
        _emailSender = Environment.GetEnvironmentVariable("EMAIL_SENDER");
        _emailSenderName = Environment.GetEnvironmentVariable("EMAIL_SENDER_NAME");
        _emailSenderAppPassword = Environment.GetEnvironmentVariable("EMAIL_SENDER_APP_PASSWORD");
        _userRepository = defaultUserRepository;
        _jwtService = jwtService;
    }

    public async Task<string> ValidateLoginAsync(LoginRequest User)
    {
        var loginUser = await _userRepository.GetUserByEmailAsync(User.Email);

        if (PasswordEncryptionHelper.VerifyPassword(User.Password, loginUser.Password))
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, loginUser.Email),
                new Claim(JwtRegisteredClaimNames.Name, loginUser.Username),
            };

            // Gerar token JWT
            var token = _jwtService.GerarToken(claims);

            // Retornar token e dados necessários
            return token;
        }
        return "";
    }

    public static string GenerateRecoverPasswordCode()
    {
        byte[] bytes = new byte[4];
        RandomNumberGenerator.Fill(bytes);
        int numero = BitConverter.ToInt32(bytes, 0) % 1000000;
        
        return Math.Abs(numero).ToString("D6");
    }

    public string? GetEmailSender()
    {
        return _emailSender;
    }
    
    public string? GetEmailSenderName()
    {
        return _emailSenderName;
    }
    
    public string? GetEmailSenderAppPassword()
    {
        return _emailSenderAppPassword;
    }
}