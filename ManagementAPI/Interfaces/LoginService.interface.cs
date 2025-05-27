using ManagementAPI.DTO.AuthController;
using ManagementAPI.DTO;

namespace ManagementAPI.Interfaces
{
    public interface ILoginService
    {
        Task<string?> ValidateLoginAsync(LoginRequest User);

        Task<LoginOtpResponseDto?> ValidateLoginByOtp(LoginOtpRequestDto User);

        Task SendOtp(string recipientAddress);

        string GenerateOtpCode(int length = 4);

        string GenerateRecoverPasswordCode();

        string GetEmailSenderAppPassword();

        string GetEmailSenderName();

        string GetEmailSender();
    }
}
