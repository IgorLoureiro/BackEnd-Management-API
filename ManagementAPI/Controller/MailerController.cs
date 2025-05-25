using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Services;
using ManagementAPI.DTO;

namespace ManagementAPI.Controller;

[ApiController]
[Route("[controller]")]

public class MailerController : ControllerBase
{

    private readonly MailerService _mailerService;

    public MailerController(MailerService mailerService) : base()
    {
        _mailerService = mailerService;
    }

    [HttpPost("sender-mail")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> SendEmail([FromBody] MailerRequestDto request)
    {
        try
        {
            string senderName = request.Sender!;
            string senderAddress = request.To!;
            string senderSubject = request.Subject!;
            string senderMessage = request.Body!;

            await _mailerService.SenderMail(senderName, senderAddress, senderSubject, senderMessage);
            return Ok("Email enviado com sucesso!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao enviar e-mail: {ex.Message}");
        }
    }


}