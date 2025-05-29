using ManagementAPI.DTO;
using ManagementAPI.Helpers;
using ManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ManagementAPI.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CreateUserBadRequestDtoExample))]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto User)
        {
            var result = await _userService.CreateUserAsync(User);
            return UserServiceErrorResultMapper.ToActionResult(result);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserListResponseDtoExample))]
        public async Task<IActionResult> GetUsersList(int limit = 10, int page = 1)
        {
            var result = await _userService.GetListUserAsync(limit, page);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserResponseDtoExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundErrorDtoExample))]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result == null) return NotFound(new { message = $"User with id '{id}' Not Found" });
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserResponseDtoExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(UpdateUserBadRequestDtoExample))]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestDto user)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, user);
            if (updatedUser == null) return NotFound(new { message = $"User with id '{id}' Not Found" });
            return Ok(updatedUser);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(InternalServerErrorDto), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundErrorDtoExample))]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deletedUser = await _userService.DeleteUserByIdAsync(id);
            if (deletedUser == null) return NotFound(new { message = $"User with id '{id}' Not Found" });

            return Ok();
        }
    }
}
