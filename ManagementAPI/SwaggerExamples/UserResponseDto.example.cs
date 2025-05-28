using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class UserResponseDtoExample : IExamplesProvider<UserResponseDto>
{
    public UserResponseDto GetExamples()
    {
        return new UserResponseDto
        {
            Id = 1,
            Username = "barry_allen",
            Email = "barry.allen@gmail.com",
            Role = "user",
        };
    }
}