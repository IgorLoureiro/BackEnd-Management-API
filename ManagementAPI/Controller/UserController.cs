using ManagementAPI.DTO;
using ManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManagementAPI.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser(DefaultUser User)
        {
            //var hashedPassword = BCrypt.Net.BCrypt.HashPassword(User.Password);

            //var newUser = new UserTable
            //{
            //    Username = User.Username,
            //    Password = hashedPassword,
            //    Email = User.Email
            //};

            //_dbContext.User.Add(newUser);
            //_dbContext.SaveChanges();

            return Created();
        }

        [HttpPost]
        public IActionResult User(DefaultUser User)
        {
            return Created();
        }
    }
}
