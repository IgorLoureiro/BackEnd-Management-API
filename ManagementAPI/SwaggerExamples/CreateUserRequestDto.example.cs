using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class CreateUserRequestDtoExample : IExamplesProvider<CreateUserRequestDto>
{
    public CreateUserRequestDto GetExamples()
    {
        return new CreateUserRequestDto
        {
            Username = "barry_allen",
            Email = "barry.allen@gmail.com",
            Password = "1234",
            Role = "user",
        };
    }
}