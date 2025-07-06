using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.User;
using AudioEngineersPlatformBackend.Contracts.User.ChangeUserProfileData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [Authorize("Admin, Client, Audio engineer")]
    [HttpGet("{idUser:guid}")]
    public async Task<IActionResult> GetUserProfileData(Guid idUser, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [Authorize("Admin, Client, Audio engineer")]
    [HttpPatch("{idUser:guid}/change-profile")]
    public async Task<IActionResult> ChangeUserProfileData(
        Guid idUser,
        [FromForm] ChangeUserProfileDataRequest changeUserProfileDataRequest,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}