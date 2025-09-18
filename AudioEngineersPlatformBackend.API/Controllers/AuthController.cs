using System.Security.Claims;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Auth.CheckAuth;
using AudioEngineersPlatformBackend.Contracts.Auth.Login;
using AudioEngineersPlatformBackend.Contracts.Auth.Register;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetEmail;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetPassword;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetPhoneNumber;
using AudioEngineersPlatformBackend.Contracts.Auth.VerifyAccount;
using AudioEngineersPlatformBackend.Contracts.Auth.VerifyForgotPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = AudioEngineersPlatformBackend.Contracts.Auth.Login.LoginRequest;
using RegisterRequest = AudioEngineersPlatformBackend.Contracts.Auth.Register.RegisterRequest;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService
    )
    {
        _authService = authService;
    }

    /// <summary>
    ///     Endpoint used for registering a user.
    /// </summary>
    /// <param name="registerRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest registerRequest,
        CancellationToken cancellationToken
    )
    {
        RegisterResponse registerResponse = await _authService.Register(registerRequest, cancellationToken);
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
    public async Task<IActionResult> VerifyAccount(
        [FromBody] VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken
    )
    {
        VerifyAccountResponse verifyAccountResponse =
            await _authService.VerifyAccount(verifyAccountRequest, cancellationToken);
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
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken
    )
    {
        LoginResponse loginResponse = await _authService.Login(loginRequest, cancellationToken);
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
        CancellationToken cancellationToken
    )
    {
        await _authService.RefreshToken(cancellationToken);
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
        await _authService.Logout();
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
    public async Task<IActionResult> CheckAuth(
        CancellationToken cancellationToken
    )
    {
        // Get the user ID from the JWT token obtained from the Authorization header (previously obtained from cookies) 
        Guid idUser = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Call the service to check get the user associated data
        CheckAuthResponse checkAuthResponse = await _authService.CheckAuth(idUser, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, checkAuthResponse);
    }

    [AllowAnonymous]
    [HttpPost("{email}/forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        string email,
        CancellationToken cancellationToken
    )
    {
        Guid forgotPasswordToken = await _authService.ForgotPassword(email, cancellationToken);
        return StatusCode(StatusCodes.Status202Accepted, forgotPasswordToken);
    }

    [AllowAnonymous]
    [HttpPost("{forgotPasswordToken:guid}/verify-forgot-password")]
    public async Task<IActionResult> VerifyForgotPassword(
        Guid forgotPasswordToken,
        [FromBody] VerifyForgotPasswordRequest verifyForgotPasswordRequest,
        CancellationToken cancellationToken
    )
    {
        await _authService.VerifyForgotPassword(forgotPasswordToken, verifyForgotPasswordRequest, cancellationToken);
        return StatusCode(StatusCodes.Status202Accepted);
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
            await _authService.ResetEmail(idUser, resetEmailRequest, cancellationToken);
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
    public async Task<IActionResult> VerifyResetEmail(
        Guid resetEmailToken,
        CancellationToken cancellationToken
    )
    {
        await _authService.VerifyResetEmail(resetEmailToken, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPatch("{idUser:guid}/reset-password")]
    public async Task<IActionResult> ResetPassword(
        Guid idUser,
        [FromBody] ResetPasswordRequest resetPasswordRequest,
        CancellationToken cancellationToken
    )
    {
        await _authService.ResetPassword(idUser, resetPasswordRequest, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [AllowAnonymous]
    [HttpPost("{resetPasswordToken:guid}/verify-reset-password")]
    public async Task<IActionResult> VerifyResetPassword(
        Guid resetPasswordToken,
        CancellationToken cancellationToken
    )
    {
        await _authService.VerifyResetPassword(resetPasswordToken, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPatch("{idUser:guid}/reset-phone-number")]
    public async Task<IActionResult> ResetPhoneNumber(
        Guid idUser,
        [FromBody] ResetPhoneNumberRequest resetPhoneNumberRequest,
        CancellationToken cancellationToken
    )
    {
        await _authService.ResetPhoneNumber(idUser, resetPhoneNumberRequest, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}