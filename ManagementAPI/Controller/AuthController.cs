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


    private readonly ILoginService _loginService;
    private readonly DbContext _dbContext;

    public AuthController(DbContext dbContext, ILoginService loginService) : base()
    {
        _loginService = loginService;
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] DefaultUserResponse signUp)
    {
        if (signUp == null)
        {
            return BadRequest(new { Message = "Invalid sign-up data." });
        }

        var existingUserByUsername = _dbContext.User.FirstOrDefault(u => u.Username == signUp.Username);
        if (existingUserByUsername != null)
        {
            return Conflict(new { Message = "Username is already taken.", Code = 1 });
        }

        var existingUserByEmail = _dbContext.User.FirstOrDefault(u => u.Email == signUp.Email);
        if (existingUserByEmail != null)
        {
            return Conflict(new { Message = "Email is already in use.", Code = 2 });
        }

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest User)
    {
        var token = await _loginService.ValidateLoginAsync(User);

        if (string.IsNullOrEmpty(token)) 
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        return Ok(new { Message = "User logged in successfully.", token});
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