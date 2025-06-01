using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class UserSignedUnauthorizedResponseDtoExample : IExamplesProvider<UnauthorizedResponseDto>
{
    public UnauthorizedResponseDto GetExamples()
    {
        return new UnauthorizedResponseDto
        {
            StatusCode = 401,
            Error = "Unauthorized",
            Message = "Unauthorized access. Please log in to continue."
        };
    }
}