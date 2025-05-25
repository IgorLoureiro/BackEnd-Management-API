using ManagementAPI.Context;
using ManagementAPI.DTO;
using ManagementAPI.Services;
using ManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;

namespace ManagementAPI.Controller;

[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{

    private readonly LoginService _loginService;
    private readonly DbContext _dbContext;

    public AuthController(DbContext dbContext, LoginService loginService) : base()
    {
        _dbContext = dbContext;
        _loginService = loginService;
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] DefaultUser signUp)
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
    public IActionResult Login([FromBody] DefaultUser User)
    {
        var loginUser = _dbContext.User.FirstOrDefault(u => u.Email == User.Email);

        if (loginUser == null)
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        if (!BCrypt.Net.BCrypt.Verify(User.Password, loginUser.Password))
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        return Ok(new { Message = "User logged in successfully.", loginUser });
    }

    [HttpPost("SendRecoverAccount")]
    public IActionResult SendRecoverAccount([FromBody] RequestRecover requestRecover)
    {
        var email = requestRecover.email;

        if (email == null)
        {
            return Unauthorized(new { Message = "Invalid email." });
        }

        var User = _dbContext.User.FirstOrDefault(u => u.Email == email);

        var RecoveryCodeGenerate = LoginService.GenerateRecoverPasswordCode();

        if (User.PasswordRecovery == null)
        {
            var passwordRecoveryObj = new
            {
                Attempts = 3,
                RecoveryCode = RecoveryCodeGenerate
            };

            string jsonPasswordRecoveryObj = JsonConvert.SerializeObject(passwordRecoveryObj);

            User.PasswordRecovery = jsonPasswordRecoveryObj;

            _dbContext.SaveChanges();
        }
        else
        {
            var UserRecovery = JsonConvert.DeserializeObject<RecoverPassword>(User.PasswordRecovery);

            if (UserRecovery.Attempts <= 0)
            {
                return Unauthorized(new { Message = "User has already used all attempts" });
            }

            var passwordRecoveryObj = new
            {
                Attempts = UserRecovery?.Attempts,
                RecoveryCode = RecoveryCodeGenerate
            };

            string jsonPasswordRecoveryObj = JsonConvert.SerializeObject(passwordRecoveryObj);

            User.PasswordRecovery = jsonPasswordRecoveryObj;

            _dbContext.SaveChanges();
        }


        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_loginService.GetEmailSenderName(), _loginService.GetEmailSender()));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Recover Password Code";

        message.Body = new TextPart("plain")
        {
            Text = RecoveryCodeGenerate
        };

        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        client.Authenticate(_loginService.GetEmailSender(), _loginService.GetEmailSenderAppPassword());
        client.Send(message);
        client.Disconnect(true);

        return Ok(new { Message = "Email sent successfully" });
    }

    [HttpPost("VerifyCode")]
    public IActionResult VerifyRecoveryCode([FromBody] RequestRecover requestRecover)
    {
        //Façam uma service pra isso!
        //Controller não lida com lógica!!! <3
        if (requestRecover.code == null)
        {
            return Unauthorized(new { Message = "Invalid code." });
        }

        var User = _dbContext.User.FirstOrDefault(u => u.Email == requestRecover.email);

        var UserRecovery = JsonConvert.DeserializeObject<RecoverPassword>(User.PasswordRecovery);

        if (UserRecovery.Code == requestRecover.code && UserRecovery.Attempts >= 0)
        {

            User.Password = BCrypt.Net.BCrypt.HashPassword(requestRecover.password);

            var passwordRecoveryObject = new
            {
                Attempts = 3,
                RecoveryCode = 0
            };

            string jsonPasswordRecoveryObject = JsonConvert.SerializeObject(passwordRecoveryObject);

            User.PasswordRecovery = jsonPasswordRecoveryObject;

            _dbContext.SaveChanges();

            return Ok(new { Message = "Password changed sucessfully" });
        }
        else if (UserRecovery?.Attempts <= 0)
        {
            return Unauthorized(new { Message = "User has already used all attempts" });
        }

        var passwordRecoveryObj = new
        {
            Attempts = (UserRecovery?.Attempts) - 1,
            RecoveryCode = UserRecovery?.Code
        };

        string jsonPasswordRecoveryObj = JsonConvert.SerializeObject(passwordRecoveryObj);

        User.PasswordRecovery = jsonPasswordRecoveryObj;

        _dbContext.SaveChanges();

        return Conflict(new { Message = $"Wrong code, you have {passwordRecoveryObj?.Attempts} attempts left" });
    }

}