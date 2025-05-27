using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO.AuthController;

namespace ManagementAPI.SwaggerExamples;

public class LoginOtpResponseDtoExample : IExamplesProvider<LoginOtpResponseDto>
{
    public LoginOtpResponseDto GetExamples()
    {
        return new LoginOtpResponseDto
        {
            token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
        };
    }
}