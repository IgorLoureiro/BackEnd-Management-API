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
        public IActionResult User(DefaultUser User)
        {
            return Created();
        }
    }
}
