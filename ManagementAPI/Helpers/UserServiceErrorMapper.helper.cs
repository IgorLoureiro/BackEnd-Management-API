using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Enums;
using ManagementAPI.DTO;

namespace ManagementAPI.Helpers;

public static class UserServiceErrorResultMapper
{
    public static IActionResult ToActionResult(UserServiceResult result)
    {
        return result switch
        {
            UserServiceResult.Success =>
                new StatusCodeResult(StatusCodes.Status201Created),

            UserServiceResult.UsernameAlreadyExists =>
                Conflict(ErrorResponseMappingHelper.Create(409, "Username", "Username already exists. Please select another username!")),

            UserServiceResult.EmailAlreadyExists =>
                Conflict(ErrorResponseMappingHelper.Create(409, "Email", "Email already exists. Please select another email!")),

            UserServiceResult.GenerationFailed =>
                BadRequest(ErrorResponseMappingHelper.Create(400, "User", "User cannot be generated due to an unexpected error.")),

            UserServiceResult.CreationFailed =>
                BadRequest(ErrorResponseMappingHelper.Create(400, "User", "User cannot be created due to an unexpected error.")),

            _ =>
                new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }

    private static IActionResult Conflict(ValidationStyleErrorDto error) =>
        new ObjectResult(error) { StatusCode = StatusCodes.Status409Conflict };

    private static IActionResult BadRequest(ValidationStyleErrorDto error) =>
        new BadRequestObjectResult(error);
}
