using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ManagementAPI.Services;

public class MailerService 
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

    public void SenderMail(string recipientName, string recipientAddress, string recipientSubject, string recipientMessage) 
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(senderName, senderEmailAddress));
        message.To.Add(new MailboxAddress(recipientName, recipientAddress));
        message.Subject = recipientSubject;

        message.Body = new TextPart("plain")
        {
            Text =  recipientMessage
        };
        
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(senderEmailAddress, senderEmailPassword);
                client.Send(message);
                client.Disconnect(true);
                Console.WriteLine("Email enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }
        
    }
}