using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class SendOtpBadRequestDtoExample : IExamplesProvider<BadRequestResponseDto>
{
    public BadRequestResponseDto GetExamples()
    {
        return new BadRequestResponseDto
        {
            Status = 400,
            Errors = new Dictionary<string, List<string>>
            {
                { "Email", new List<string> { "The Email field is required.", "The Email field is not a valid e-mail address." } },
            }
        };
    }
}