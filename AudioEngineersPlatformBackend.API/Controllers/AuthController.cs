using System.Security.Claims;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Auth.CheckAuth;
using AudioEngineersPlatformBackend.Contracts.Auth.Login;
using AudioEngineersPlatformBackend.Contracts.Auth.Register;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetEmail;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetPassword;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetPhoneNumber;
using AudioEngineersPlatformBackend.Contracts.Auth.VerifyAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = AudioEngineersPlatformBackend.Contracts.Auth.Login.LoginRequest;
using RegisterRequest = AudioEngineersPlatformBackend.Contracts.Auth.Register.RegisterRequest;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    ///     Endpoint used for registering a user.
    /// </summary>
    /// <param name="registerRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest,
        CancellationToken cancellationToken)
    {
        RegisterResponse registerResponse = await authService.Register(registerRequest, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, registerResponse);
    }

    /// <summary>
    ///     Endpoint used for verifying a newly registered user.  
    /// </summary>
    /// <param name="verifyAccountRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("verify-account")]
    public async Task<IActionResult> VerifyAccount([FromBody] VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken)
    {
        VerifyAccountResponse verifyAccountResponse =
            await authService.VerifyAccount(verifyAccountRequest, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, verifyAccountResponse);
    }

    /// <summary>
    ///     Endpoint used for logging in.
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        LoginResponse loginResponse = await authService.Login(loginRequest, cancellationToken);
        return StatusCode(StatusCodes.Status202Accepted, loginResponse);
    }

    /// <summary>
    ///     Endpoint used for refreshing tokens.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        CancellationToken cancellationToken)
    {
        await authService.RefreshToken(cancellationToken);
        return StatusCode(StatusCodes.Status202Accepted);
    }

    /// <summary>
    ///     Endpoint used for logging out.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await authService.Logout();
        return StatusCode(StatusCodes.Status200OK);
    }

    /// <summary>
    ///     Endpoint used for checking the authentication state of a user
    ///     and retrieving their crucial information.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("check-auth")]
    public async Task<IActionResult> CheckAuth(CancellationToken cancellationToken)
    {
        // Get the user ID from the JWT token obtained from the Authorization header (previously obtained from cookies) 
        Guid idUser = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Call the service to check get the user associated data
        CheckAuthResponse checkAuthResponse = await authService.CheckAuth(idUser, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, checkAuthResponse);
    }

    /// <summary>
    ///     Endpoint used for resetting the email address of a specified user. 
    /// </summary>
    /// <param name="idUser"></param>
    /// <param name="resetEmailRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPatch("{idUser:guid}/reset-email")]
    public async Task<IActionResult> ResetEmail(
        Guid idUser,
        [FromBody] ResetEmailRequest resetEmailRequest,
        CancellationToken cancellationToken
    )
    {
        ResetEmailResponse resetEmailResponse =
            await authService.ResetEmail(idUser, resetEmailRequest, cancellationToken);
        return StatusCode(StatusCodes.Status202Accepted, resetEmailResponse);
    }

    /// <summary>
    ///     Endpoint used for verifying a newly set email. 
    /// </summary>
    /// <param name="resetEmailToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("{resetEmailToken:guid}/verify-reset-email")]
    public async Task<IActionResult> VerifyResetEmail(Guid resetEmailToken, CancellationToken cancellationToken)
    {
        await authService.VerifyResetEmail(resetEmailToken, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }


    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPatch("{idUser:guid}/reset-password")]
    public async Task<IActionResult> ResetPassword(Guid idUser, [FromBody] ResetPasswordRequest resetPasswordRequest,
        CancellationToken cancellationToken)
    {
        await authService.ResetPassword(idUser, resetPasswordRequest, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [AllowAnonymous]
    [HttpPost("{resetPasswordToken:guid}/verify-reset-password")]
    public async Task<IActionResult> VerifyResetPassword(Guid resetPasswordToken, CancellationToken cancellationToken)
    {
        await authService.VerifyResetPassword(resetPasswordToken, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPatch("{idUser:guid}/reset-phone-number")]
    public async Task<IActionResult> ResetPhoneNumber(Guid idUser,
        [FromBody] ResetPhoneNumberRequest resetPhoneNumberRequest, CancellationToken cancellationToken)
    {
        await authService.ResetPhoneNumber(idUser, resetPhoneNumberRequest, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}