using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO.AuthController;

namespace ManagementAPI.SwaggerExamples;

public class LoginOtpRequestDtoExample : IExamplesProvider<LoginOtpRequestDto>
{
    public LoginOtpRequestDto GetExamples()
    {
        return new LoginOtpRequestDto
        {
            Email = "barry.allen@gmail.com",
            Password = "1234"
        };
    }
}