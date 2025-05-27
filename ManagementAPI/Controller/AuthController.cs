using ManagementAPI.Context;
using ManagementAPI.DTO;
using ManagementAPI.DTO.AuthController;
using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Helpers;
using ManagementAPI.Interfaces;

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
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest User)
    {
        var token = await _loginService.ValidateLoginAsync(User);

        if (token == null) return Unauthorized(new { Message = "Invalid username or password." });

        return Ok(new { token = token });
    }

    [HttpPost("login-otp")]
    [ProducesResponseType(typeof(LoginOtpResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
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