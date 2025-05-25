using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.DTO;

namespace ManagementAPI.SwaggerMocks;

public class MailerRequestDtoExample : IExamplesProvider<MailerRequestDto>
{
    public MailerRequestDto GetExamples()
    {
        return new MailerRequestDto
        {
            Sender = "remetente@email.com",
            To = "destinatario@email.com",
            Subject = "Teste de envio",
            Body = "Olá, este é um teste de envio de email."
        };
    }
}