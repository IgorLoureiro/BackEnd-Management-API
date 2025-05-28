using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class NotFoundErrorDtoExample : IExamplesProvider<NotFoundErrorDto>
{
    public NotFoundErrorDto GetExamples()
    {
        return new NotFoundErrorDto
        {
            status = 404,
            message = "User with id '123' not found!",
            error = "Not Found",
        };
    }
}