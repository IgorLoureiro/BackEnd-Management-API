using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class CreateUserBadRequestDtoExample : IExamplesProvider<BadRequestResponseDto>
{
    public BadRequestResponseDto GetExamples()
    {
        return new BadRequestResponseDto
        {
            Status = 400,
            Errors = new Dictionary<string, List<string>>
            {
                { "Email", new List<string> { "The Email field is required.", "The Email field is not a valid e-mail address." } },
                { "Password", new List<string> { "The Password field is required." } },
                { "Username", new List<string> { "The Username field is required." } },
                { "Role", new List<string> { "The Role field is required.", "Role must be either 'admin' or 'user'." } }

            }
        };
    }
}