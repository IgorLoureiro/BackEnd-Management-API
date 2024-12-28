using System;
using System.Security.Cryptography;

namespace ManagementAPI.Services;

public class LoginService
{

    private readonly string _emailSender;
    private readonly string _emailSenderName;
    private readonly string? _emailSenderAppPassword;

    public LoginService()
    {
        _emailSender = Environment.GetEnvironmentVariable("EmailSender");
        _emailSenderName = Environment.GetEnvironmentVariable("EmailSenderName");
        _emailSenderAppPassword = Environment.GetEnvironmentVariable("EmailSenderAppPassword");
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
}