using ManagementAPI.DTO;
using System.Security.Claims;

namespace ManagementAPI.Interfaces
{
    public interface ILoginService
    {
        UserSignedDto? GetSignedUser(ClaimsIdentity identity);

        Task<string?> ValidateLoginAsync(LoginRequestDto User);

        Task<LoginOtpResponseDto?> ValidateLoginByOtp(LoginOtpRequestDto User);

        Task SendOtp(string recipientAddress);

        string GenerateOtpCode(int length = 4);

        string GenerateRecoverPasswordCode();

        string GetEmailSenderAppPassword();

        string GetEmailSenderName();

        string GetEmailSender();
    }
}
