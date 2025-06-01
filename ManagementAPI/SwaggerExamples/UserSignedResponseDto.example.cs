using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class UserSignedResponseDtoExample : IExamplesProvider<UserSignedDto>
{
    public UserSignedDto GetExamples()
    {
        return new UserSignedDto
        {
            Id = "1",
            Username = "barry_allen",
            Email = "barry.allen@gmail.com",
            Role = "user",
        };
    }
}