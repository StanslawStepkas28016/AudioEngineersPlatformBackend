using System.Security.Claims;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = AudioEngineersPlatformBackend.Contracts.Auth.LoginRequest;
using RegisterRequest = AudioEngineersPlatformBackend.Contracts.Auth.RegisterRequest;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest,
        CancellationToken cancellationToken)
    {
        var registerResponse = await _authService.Register(registerRequest, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, registerResponse);
    }

    [AllowAnonymous]
    [HttpPost("verify-account")]
    public async Task<IActionResult> VerifyAccount([FromBody] VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken)
    {
        var verifyAccountResponse = await _authService.VerifyAccount(verifyAccountRequest, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, verifyAccountResponse);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var loginResponse = await _authService.Login(loginRequest, cancellationToken);
        return StatusCode(StatusCodes.Status202Accepted, loginResponse);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        CancellationToken cancellationToken)
    {
        var refreshTokenResponse = await _authService.RefreshToken(cancellationToken);
        return StatusCode(StatusCodes.Status202Accepted, refreshTokenResponse);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return StatusCode(StatusCodes.Status200OK);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("check-auth")]
    public async Task<IActionResult> CheckAuth(CancellationToken cancellationToken)
    {
        // Get the user ID from the JWT token obtained from the Authorization header (previously obtained from cookies) 
        var idUser = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Call the service to check get the user associated data
        var checkAuthResponse = await _authService.CheckAuth(idUser, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, checkAuthResponse);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword()
    {
        // Change a password by sending an e-mail with instructions
        // return StatusCode(StatusCodes.Status202Accepted);
        throw new NotImplementedException("Reset password functionality is not implemented yet.");
    }
}