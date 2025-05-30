using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class UpdateUserBadRequestDtoExample : IExamplesProvider<BadRequestResponseDto>
{
    public BadRequestResponseDto GetExamples()
    {
        return new BadRequestResponseDto
        {
            Status = 400,
            Errors = new Dictionary<string, List<string>>
            {
                { "Email", new List<string> { "The Email field is required if provided.", "The Email field is not a valid e-mail address." } },
                { "Password", new List<string> { "The Password field is required if provided." } },
                { "Username", new List<string> { "The Username field is required if provided." } },
                { "Role", new List<string> { "The Role field is required if provided.", "Role must be either 'admin' or 'user'." } }

            }
        };
    }
}