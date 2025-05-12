using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = AudioEngineersPlatformBackend.Contracts.Authentication.LoginRequest;
using RegisterRequest = AudioEngineersPlatformBackend.Contracts.Authentication.RegisterRequest;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        var registerResponse = await _authenticationService.Register(registerRequest, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, registerResponse);
    }

    [AllowAnonymous]
    [HttpPost("verify-account")]
    public async Task<IActionResult> VerifyAccount(VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken)
    {
        var verifyAccountResponse = await _authenticationService.VerifyAccount(verifyAccountRequest, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, verifyAccountResponse);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var loginResponse = await _authenticationService.Login(loginRequest, cancellationToken);

        // Return a cookie to the browser
        Response.Cookies.Append("token",
            loginResponse.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(24)
            }
        );

        return StatusCode(StatusCodes.Status202Accepted, loginResponse);
    }

    [HttpGet("test")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
    
}