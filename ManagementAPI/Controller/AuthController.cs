using ManagementAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Helpers;
using ManagementAPI.Interfaces;
using ManagementAPI.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.Types;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

    [HttpGet("signed")]
    [Authorize]
    [ProducesResponseType(typeof(UserSignedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponseDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserSignedResponseDtoExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UserSignedUnauthorizedResponseDtoExample))]
    public ActionResult<UserSignedDto> GetSignedUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity == null || !identity.IsAuthenticated)
            return Unauthorized();

        var userData = _loginService.GetSignedUser(identity);

        if (userData == null)
            return Unauthorized();

        return Ok(userData);
    }

    [HttpPost("Login")]
    [ProducesResponseType(typeof(LoginOtpResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(LoginBadRequestDtoExample))]
    public async Task<ActionResult<LoginOtpResponseDto>> Login([FromBody] LoginRequestDto User)
    {
        try
        {
            var token = await _loginService.ValidateLoginAsync(User);

            if (token == null) return Unauthorized(new { Message = "Invalid username or password." });

            return Ok(new { token });
        }
        catch (HttpResponseException ex)
        {
            return StatusCode(ex.StatusCode, ex.ErrorObject);
        }
    }

    [HttpPost("login-otp")]
    [ProducesResponseType(typeof(LoginOtpResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(LoginBadRequestDtoExample))]
    public async Task<ActionResult<LoginOtpResponseDto>> LoginOtp([FromBody] LoginOtpRequestDto User)
    {
        try
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
        catch (HttpResponseException ex)
        {
            return StatusCode(ex.StatusCode, ex.ErrorObject);
        }
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
            await _loginService.SendOtp(request.Email!);
            return Ok();
        }
        catch (HttpResponseException ex)
        {
            return StatusCode(ex.StatusCode, ex.ErrorObject);
        }
    }
}