using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementAPI.Controller
{


    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("protected")]
        [Authorize]
        public IActionResult ProtectedRoute()
        {
            return Ok(new { message = "Você está autenticado!" });
        }
    }

}
