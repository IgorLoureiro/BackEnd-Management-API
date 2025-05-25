using System;
using System.Security.Cryptography;

namespace ManagementAPI.Services;

public class LoginService
{
    private readonly string _emailSender;
    private readonly string _emailSenderName;
    private readonly string? _emailSenderAppPassword;
    private readonly MailerService _mailerService;

    public LoginService(MailerService mailerService)
    {
        _emailSender = Environment.GetEnvironmentVariable("EmailSender")!;
        _emailSenderName = Environment.GetEnvironmentVariable("EmailSenderName")!;
        _emailSenderAppPassword = Environment.GetEnvironmentVariable("EmailSenderAppPassword")!;
        _mailerService = mailerService;
    }

    public async Task SendOtp(string recipientAddress)
    {
        /* Generate CTP code */
        int rangeOtp = 4;
        var otpCode = GenerateOtpCode(rangeOtp);

        /* Generate Template */
        string recipientName = recipientAddress;
        string recipientSubject = "Código de Acesso - Sharp Guard";
        string recipientMessage = $"Seu código de acesso é: {otpCode}";

        /* Send template via email */
        await _mailerService.SenderMail(recipientName, recipientAddress, recipientSubject, recipientMessage);
    }

    public static string GenerateRecoverPasswordCode()
    {
        byte[] bytes = new byte[4];
        RandomNumberGenerator.Fill(bytes);
        int numero = BitConverter.ToInt32(bytes, 0) % 1000000;

        return Math.Abs(numero).ToString("D6");
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

    public string GenerateOtpCode(int length = 4)
    {
        var random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => (char)('0' + random.Next(0, 10)))
            .ToArray());
    }
}