using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Enums;

namespace ManagementAPI.Helpers
{
    public static class UserControllerHelper
    {
        public static IActionResult ToActionResult(this UserServiceResult result)
        {
            return result switch
            {
                UserServiceResult.Success => new StatusCodeResult(201),
                UserServiceResult.InvalidUser => new BadRequestObjectResult("Invalid user data."),
                UserServiceResult.UsernameAlreadyExists => new ConflictObjectResult("Username already exists."),
                UserServiceResult.EmailAlreadyExists => new ConflictObjectResult("Email already exists."),
                UserServiceResult.GenerationFailed or UserServiceResult.CreationFailed => new ObjectResult("User creation failed.") { StatusCode = 500 },
                _ => new ObjectResult("Unknown error.") { StatusCode = 500 }
            };
        }
    }
}
