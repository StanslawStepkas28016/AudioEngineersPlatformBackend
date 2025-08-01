using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{

    
    // [AllowAnonymous]
    // [HttpPatch("{emailResetToken:guid}/validate-reset-email")]
    // public async Task<IActionResult> ValidateResetEmail(Guid emailResetToken)
    // {
    //     throw new NotImplementedException();
    // }

    // [AllowAnonymous]
    // [HttpPatch("{idUser:guid}/reset-password")]
    // public async Task<IActionResult> ResetPassword(Guid idUser)
    // {
    //     throw new NotImplementedException();
    // }
}