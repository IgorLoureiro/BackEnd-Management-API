using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class UserListResponseDtoExample : IExamplesProvider<List<UserResponseDto>>
{
    public List<UserResponseDto> GetExamples()
    {
        return new List<UserResponseDto>
        {
            new UserResponseDto
            {
                Id = 1,
                Username = "barry_allen",
                Email = "barry.allen@gmail.com",
                Role = "user",
            },
            new UserResponseDto
            {
                Id = 2,
                Username = "steven_rogers",
                Email = "steve.rogers@gmail.com",
                Role = "admin",
            }
        };
    }
}