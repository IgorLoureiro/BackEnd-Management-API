using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using ManagementAPI.Interfaces;

namespace ManagementAPI.Services;

public class MailerService : IMailerService
{
    private readonly string senderName = "System OTP";

    private readonly string senderEmailAddress;

    private readonly string senderEmailPassword;

    private readonly string smtpServer;

    private readonly string smtpPort;



    public MailerService()
    {
        senderEmailAddress = Environment.GetEnvironmentVariable("EMAIL_SENDER") ?? "";
        senderEmailPassword = Environment.GetEnvironmentVariable("EMAIL_SENDER_APP_PASSWORD") ?? "";
        smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? "";
        smtpPort = Environment.GetEnvironmentVariable("SMTP_PORT") ?? "0";

    }

    public string GetSenderEmailAddress()
    {
        return senderEmailAddress;
    }

    public string GetSenderEmailPassword()
    {
        return senderEmailPassword;
    }

    public async Task SendOtpEmail(string email, string otpCode)
    {
        /* generate OTP template */
        string recipientName = "usuário";
        string senderName = "Sharp Guard";

        string htmlTemplate = await File.ReadAllTextAsync("templates/otp.html");

        string htmlBody = htmlTemplate
             .Replace("{{USER_NAME}}", recipientName)
             .Replace("{{OTP_CODE}}", otpCode);


        /* send template email */
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmailAddress));
        message.To.Add(new MailboxAddress(recipientName, email));
        message.Subject = "Código de Acesso - Sharp Guard";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody,
        };

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpServer, int.Parse(smtpPort), SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(senderEmailAddress, senderEmailPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SenderMail(string recipientName, string recipientAddress, string recipientSubject, string recipientMessage)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(senderName, senderEmailAddress));
        message.To.Add(new MailboxAddress(recipientName, recipientAddress));
        message.Subject = recipientSubject;

        message.Body = new TextPart("plain")
        {
            Text = recipientMessage
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(smtpServer, int.Parse(smtpPort), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(senderEmailAddress, senderEmailPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            Console.WriteLine("Email enviado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
        }

    }
}