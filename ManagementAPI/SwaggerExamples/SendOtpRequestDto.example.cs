using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO.AuthController;

namespace ManagementAPI.SwaggerExamples;

public class SendOtpRequestDtoExample : IExamplesProvider<SendOtpRequestDto>
{
    public SendOtpRequestDto GetExamples()
    {
        return new SendOtpRequestDto
        {
            Email = "barry.allen@gmail.com",
        };
    }
}