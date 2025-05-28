using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class InternalServerErrorDtoExample : IExamplesProvider<InternalServerErrorDto>
{
    public InternalServerErrorDto GetExamples()
    {
        return new InternalServerErrorDto
        {
            status = 500,
            message = "An unexpected error occurred. Please try again later."
        };
    }
}