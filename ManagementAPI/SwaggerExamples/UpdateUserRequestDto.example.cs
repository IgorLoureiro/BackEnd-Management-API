using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class UpdateUserRequestDtoExample : IExamplesProvider<UpdateUserRequestDto>
{
    public UpdateUserRequestDto GetExamples()
    {
        return new UpdateUserRequestDto
        {
            Username = "barry_allen",
            Email = "barry.allen@gmail.com",
            Password = "1234",
            Role = "user",
        };
    }
}