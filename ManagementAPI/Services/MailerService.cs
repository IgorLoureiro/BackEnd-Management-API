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

    public MailerService()
    {
        senderEmailAddress = Environment.GetEnvironmentVariable("EmailSender") ?? "";
        senderEmailPassword = Environment.GetEnvironmentVariable("EmailSenderAppPassword") ?? "";
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
        string recipientName = "Random Recipient";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmailAddress));
        message.To.Add(new MailboxAddress(recipientName, email));
        message.Subject = "Código de Acesso - Sharp Guard";

        message.Body = new TextPart("plain")
        {
            Text = $"Seu código de verificação é: {otpCode}"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync("sandbox.smtp.mailtrap.io", 587, SecureSocketOptions.StartTls); // sandbox.smtp.mailtrap.io
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

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync("sandbox.smtp.mailtrap.io", 587, SecureSocketOptions.StartTls);
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
}