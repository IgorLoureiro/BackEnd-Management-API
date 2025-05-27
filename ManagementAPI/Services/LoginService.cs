using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ManagementAPI.DTO;
using ManagementAPI.DTO.AuthController;
using ManagementAPI.Interfaces;
using ManagementAPI.Helpers;

namespace ManagementAPI.Services;

public class LoginService : ILoginService
{
    private readonly string _emailSender;
    private readonly string _emailSenderName;
    private readonly string _emailSenderAppPassword;
    private readonly IJwtService _jwtService;
    private readonly IDefaultUserRepository _userRepository;
    private readonly IMailerService _mailerService;

    public LoginService(IDefaultUserRepository userRepository, IJwtService jwtService, IMailerService mailerService)
    {
        _emailSender = Environment.GetEnvironmentVariable("EmailSender")!;
        _emailSenderName = Environment.GetEnvironmentVariable("EmailSenderName")!;
        _emailSenderAppPassword = Environment.GetEnvironmentVariable("EmailSenderAppPassword")!;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mailerService = mailerService;
    }
    public string GetEmailSender()
    {
        return _emailSender;
    }

    public string GetEmailSenderName()
    {
        return _emailSenderName;
    }

    public string GetEmailSenderAppPassword()
    {
        return _emailSenderAppPassword;
    }

    public async Task<string?> ValidateLoginAsync(LoginRequest User)
    {
        var loginUser = await _userRepository.GetUserByEmailAsync(User.Email);
        if (loginUser == null) return null;

        /* validate password */
        var isValidPassword = PasswordEncryptionHelper.VerifyPassword(User.Password, loginUser.Password);
        if (!isValidPassword) return null;

        /* Generate JWT Token */
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, loginUser.Email),
                new Claim(JwtRegisteredClaimNames.Name, loginUser.Username),
                new Claim(ClaimTypes.Role, loginUser.Role),
            };

        /* return token */
        var token = _jwtService.GenerateToken(claims);
        return token;
    }

    public async Task<LoginOtpResponseDto?> ValidateLoginByOtp(LoginOtpRequestDto User)
    {
        /* Find existing user */
        var loginUser = await _userRepository.GetUserByEmailAsync(User.Email);
        if (loginUser == null) return null;

        /* Validate OTP Expiration */
        if (loginUser.OtpCode == null || !loginUser.OtpExpiration.HasValue || loginUser.OtpExpiration.Value < DateTime.UtcNow) return null;

        /* Validate Otp code */
        var isValidPassword = PasswordEncryptionHelper.VerifyPassword(User.Password, loginUser.OtpCode);
        if (!isValidPassword) return null;

        /* Generate JWT Token */
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, loginUser.Email),
                new Claim(JwtRegisteredClaimNames.Name, loginUser.Username),
                new Claim(ClaimTypes.Role, loginUser.Role),
            };

        var token = _jwtService.GenerateToken(claims);

        return new LoginOtpResponseDto { token = token };
    }

    public async Task SendOtp(string recipientAddress)
    {
        /* Find existing user */
        var loginUser = await _userRepository.GetUserByEmailAsync(recipientAddress);

        if (loginUser == null) return;

        /* Generate OTP code */
        int rangeOtp = 4;
        var otpCode = GenerateOtpCode(rangeOtp);

        /* update user password to use otp code */
        loginUser.OtpCode = BCrypt.Net.BCrypt.HashPassword(otpCode);
        loginUser.OtpExpiration = DateTime.UtcNow.AddMinutes(5); // expira em 5 minutos

        await _userRepository.UpdateUser(loginUser);

        /* Generate Template */
        string recipientName = recipientAddress;
        string recipientSubject = "Código de Acesso - Sharp Guard";
        string recipientMessage = $"Seu código de acesso é: {otpCode}";

        /* Send template via email */
        await _mailerService.SenderMail(recipientName, recipientAddress, recipientSubject, recipientMessage);
    }

    public string GenerateOtpCode(int length = 4)
    {
        var random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => (char)('0' + random.Next(0, 10)))
            .ToArray());
    }

    public string GenerateRecoverPasswordCode()
    {
        byte[] bytes = new byte[4];
        RandomNumberGenerator.Fill(bytes);
        int numero = BitConverter.ToInt32(bytes, 0) % 1000000;

        return Math.Abs(numero).ToString("D6");
    }
}