using ManagementAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Helpers;
using ManagementAPI.Interfaces;
using ManagementAPI.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace ManagementAPI.Controller;

[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;

    public AuthController(ILoginService loginService) : base()
    {
        _loginService = loginService;
    }

    [HttpPost("Login")]
    [ProducesResponseType(typeof(LoginOtpResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(LoginBadRequestDtoExample))]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto User)
    {
        var token = await _loginService.ValidateLoginAsync(User);

        if (token == null) return Unauthorized(new { Message = "Invalid username or password." });

        return Ok(new { token = token });
    }

    [HttpPost("login-otp")]
    [ProducesResponseType(typeof(LoginOtpResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(LoginBadRequestDtoExample))]
    public async Task<ActionResult<LoginOtpResponseDto>> LoginOtp([FromBody] LoginOtpRequestDto User)
    {
        var loginOtpResponse = await _loginService.ValidateLoginByOtp(User);

        if (loginOtpResponse == null)
        {
            return Unauthorized(ErrorResponseMappingHelper.Create(401, "Login", "Invalid username or password."));
        }

        if (string.IsNullOrEmpty(loginOtpResponse.token))
        {
            return Unauthorized(ErrorResponseMappingHelper.Create(401, "Token", "Token cannot be empty."));
        }

        return Ok(loginOtpResponse);
    }

    [HttpPost("send-otp")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(SendOtpBadRequestDtoExample))]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDto request)
    {
        try
        {
            string senderAddress = request.Email!;

            await _loginService.SendOtp(senderAddress);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao enviar e-mail: {ex.Message}");
        }
    }
}