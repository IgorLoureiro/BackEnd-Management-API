using ManagementAPI.DTO;

namespace ManagementAPI.Services;

public interface ILoginService
{
    Task<string> ValidateLoginAsync(LoginRequest user);
    string? GetEmailSender();
    string? GetEmailSenderName();
    string? GetEmailSenderAppPassword();
}
