using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.User;
using AudioEngineersPlatformBackend.Contracts.User.ChangeData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPatch("{idUser:guid}/change-data")]
    public async Task<IActionResult> ChangeData(
        Guid idUser,
        [FromBody] ChangeDataRequest changeDataRequest,
        CancellationToken cancellationToken
    )
    {
        Guid result = await userService.ChangeData(idUser, changeDataRequest, cancellationToken);
        return StatusCode(StatusCodes.Status202Accepted, result);
    }
    
    [AllowAnonymous]
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        await userService.Test();
        return Ok();
    }
}