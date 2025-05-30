namespace ManagementAPI.Interfaces;

public interface IMailerService
{
    string GetSenderEmailAddress();
    string GetSenderEmailPassword();
    Task SendOtpEmail(string email, string otpCode);
    Task SenderMail(string recipientName, string recipientAddress, string recipientSubject, string recipientMessage);
}


