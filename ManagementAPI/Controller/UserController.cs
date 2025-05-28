using ManagementAPI.DTO;
using ManagementAPI.Helpers;
using ManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementAPI.Controller
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        { 
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] DefaultUserResponse User)
        {
           var result = await _userService.CreateUserAsync(User);
          return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if(result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("UserList")]
        public async Task<IActionResult> GetUsersList(int usersPerPage, int page)
        {
            var result = await _userService.GetListUserAsync(usersPerPage, page);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] DefaultUserRequest user)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, user);
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deletedUser = await _userService.DeleteUserByIdAsync(id);
            if (deletedUser == null) return NotFound();
            return Ok(deletedUser);
        }
    }
}
