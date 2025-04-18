using System.Threading;
using System.Threading.Tasks;
using AudioEngineersPlatformBackend.Dtos.Auth.Login;
using AudioEngineersPlatformBackend.Dtos.Auth.Register;
using AudioEngineersPlatformBackend.Dtos.Auth.VerifyEmail;
using AudioEngineersPlatformBackend.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AudioEngineersPlatformBackend.Controllers;

[Route("/api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto,
        CancellationToken cancellationToken)
    {
        var registerResponseDto = await _authService.Register(registerRequestDto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, registerResponseDto);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailRequestDto verifyEmailRequestDto,
        CancellationToken cancellationToken)
    {
        var verifyEmailResponseDto = await _authService.VerifyEmail(verifyEmailRequestDto, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, verifyEmailResponseDto);
    }

    // Add send cookies
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var loginResponseDto = await _authService.Login(loginRequestDto, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, loginResponseDto);
    }
}