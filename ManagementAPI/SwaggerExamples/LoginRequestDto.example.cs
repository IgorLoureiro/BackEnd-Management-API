using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerExamples;

public class LoginRequestDtoExample : IExamplesProvider<LoginRequestDto>
{
    public LoginRequestDto GetExamples()
    {
        return new LoginRequestDto
        {
            Email = "clark.kent@mail.com.br",
            Password = "Abc!1234"
        };
    }
}