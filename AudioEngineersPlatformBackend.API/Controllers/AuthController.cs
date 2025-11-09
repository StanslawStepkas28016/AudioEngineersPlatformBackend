using System.IdentityModel.Tokens.Jwt;
using API.Abstractions;
using API.Contracts.Auth.Commands.ForgotPassword;
using API.Contracts.Auth.Commands.Login;
using API.Contracts.Auth.Commands.Logout;
using API.Contracts.Auth.Commands.Register;
using API.Contracts.Auth.Commands.ResetAndVerifyPhoneNumber;
using API.Contracts.Auth.Commands.ResetEmail;
using API.Contracts.Auth.Commands.ResetPassword;
using API.Contracts.Auth.Commands.VerifyAccount;
using API.Contracts.Auth.Commands.VerifyForgotPassword;
using API.Contracts.Auth.Commands.VerifyResetEmail;
using API.Contracts.Auth.Commands.VerifyResetPassword;
using API.Contracts.Auth.Queries.CheckAuth;
using API.Util.CookieUtil;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ForgotPassword;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Login;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Logout;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.RefreshToken;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Register;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetAndVerifyPhoneNumber;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetEmail;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetPassword;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyAccount;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyForgotPassword;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetEmail;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetPassword;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Queries.CheckAuth;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;
    private readonly IJwtUtil _jwtUtil;
    private readonly ICookieUtil _cookieUtil;
    private readonly IClaimsUtil _claimsUtil;

    public AuthController(
        IMapper mapper,
        ISender sender,
        IJwtUtil jwtUtil,
        ICookieUtil cookieUtil,
        IClaimsUtil claimsUtil
    )
    {
        _mapper = mapper;
        _sender = sender;
        _jwtUtil = jwtUtil;
        _cookieUtil = cookieUtil;
        _claimsUtil = claimsUtil;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        // Map request to command.
        RegisterCommand command = _mapper.Map<RegisterRequest, RegisterCommand>(request);

        // Send to mediator.
        RegisterCommandResult result = await _sender.Send(command, cancellationToken);

        // Map to response.
        RegisterResponse response = _mapper.Map<RegisterCommandResult, RegisterResponse>(result);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("verify-account")]
    public async Task<IActionResult> VerifyAccount(
        [FromBody] VerifyAccountRequest request,
        CancellationToken cancellationToken
    )
    {
        // Map request to command.
        VerifyAccountCommand command = _mapper.Map<VerifyAccountRequest, VerifyAccountCommand>(request);

        // Send to mediator.
        VerifyAccountCommandResult result = await _sender.Send(command, cancellationToken);

        // Map to response.
        VerifyAccountResponse response = _mapper.Map<VerifyAccountCommandResult, VerifyAccountResponse>(result);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken
    )
    {
        // Map to request to command.
        LoginCommand command = _mapper.Map<LoginRequest, LoginCommand>(loginRequest);

        // Send to mediator.
        LoginCommandResult result = await _sender.Send(command, cancellationToken);

        // Send appropriate cookies.
        JwtSecurityToken jwtAccessToken = await _jwtUtil.CreateJwtAccessToken(result.User);

        // Write both tokens as cookies.
        await _cookieUtil.WriteAsCookie
        (
            CookieNames.AccessToken,
            new JwtSecurityTokenHandler().WriteToken(jwtAccessToken),
            jwtAccessToken.ValidTo
        );

        await _cookieUtil.WriteAsCookie
        (
            CookieNames.RefreshToken,
            result
                .User
                .Tokens.First()
                .Value,
            result
                .User
                .Tokens.First()
                .ExpirationDate
        );

        // Map result to response.
        LoginResponse loginResponse = _mapper.Map<LoginCommandResult, LoginResponse>(result);

        return Ok(loginResponse);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        CancellationToken cancellationToken
    )
    {
        // Obtain the refreshToken from request cookies.
        Guid refreshToken = Guid.Parse(await _cookieUtil.GetCookie(CookieNames.RefreshToken));

        // Map context extracted request data to command.
        RefreshTokenCommand command = _mapper.Map<Guid, RefreshTokenCommand>(refreshToken);

        // Send to mediator.
        RefreshTokenCommandResult result = await _sender.Send(command, cancellationToken);

        // Write a new access token and a new refresh token as cookies.
        JwtSecurityToken accessToken = await _jwtUtil.CreateJwtAccessToken(result.User);

        await _cookieUtil.WriteAsCookie
        (
            CookieNames.AccessToken,
            new JwtSecurityTokenHandler().WriteToken(accessToken),
            accessToken.ValidTo
        );

        await _cookieUtil.WriteAsCookie
        (
            CookieNames.RefreshToken,
            result.User.Tokens.First()
                .Value,
            result.User.Tokens.First()
                .ExpirationDate
        );

        return Ok();
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser from the context and the refreshToken from the request cookie.
        Guid idUser = await _claimsUtil.ExtractIdUserFromClaims();

        string refreshToken = await _cookieUtil.GetCookie(CookieNames.RefreshToken);

        LogoutRequest request = new LogoutRequest { IdUser = idUser, RefreshToken = refreshToken };

        // Map from request to command.
        LogoutCommand command = _mapper.Map<LogoutRequest, LogoutCommand>(request);

        // Send to mediator, to remove the current refreshToken from the db.
        LogoutCommandResult result = await _sender.Send(command, cancellationToken);

        // Map from result to response.
        LogoutResponse response = _mapper.Map<LogoutCommandResult, LogoutResponse>(result);

        // Delete the cookies from the clients browser.
        await _cookieUtil.DeleteCookie(CookieNames.AccessToken);
        await _cookieUtil.DeleteCookie(CookieNames.RefreshToken);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpGet("check-auth")]
    public async Task<IActionResult> CheckAuth(
        CancellationToken cancellationToken
    )
    {
        // Get the user ID from the JWT token obtained from the Authorization header (previously obtained from cookies).
        Guid idUser = await _claimsUtil.ExtractIdUserFromClaims();

        // Map to query.
        CheckAuthQuery query = _mapper.Map<Guid, CheckAuthQuery>(idUser);

        // Send query to mediator.
        CheckAuthQueryResult result = await _sender.Send(query, cancellationToken);

        // Map result to response.
        CheckAuthResponse response = _mapper.Map<CheckAuthQueryResult, CheckAuthResponse>(result);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest forgotPasswordRequest,
        CancellationToken cancellationToken
    )
    {
        // Map request to command.
        ForgotPasswordCommand command = _mapper.Map<ForgotPasswordRequest, ForgotPasswordCommand>
            (forgotPasswordRequest);

        // Send to mediator.
        ForgotPasswordCommandResult result = await _sender.Send(command, cancellationToken);

        // Map result to response.
        ForgotPasswordResponse response = _mapper.Map<ForgotPasswordCommandResult, ForgotPasswordResponse>(result);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("{forgotPasswordToken:guid}/verify-forgot-password")]
    public async Task<IActionResult> VerifyForgotPassword(
        [FromRoute] Guid forgotPasswordToken,
        [FromBody] VerifyForgotPasswordRequest verifyForgotPasswordRequest,
        CancellationToken cancellationToken
    )
    {
        // Map from request to command.
        VerifyForgotPasswordCommand command = _mapper.Map<VerifyForgotPasswordRequest, VerifyForgotPasswordCommand>
        (
            verifyForgotPasswordRequest,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) => dest.ForgotPasswordToken = forgotPasswordToken
            )
        );

        // Send to mediator.
        VerifyForgotPasswordCommandResult result = await _sender
            .Send(command, cancellationToken);

        // Map the result to response.
        VerifyForgotPasswordResponse response =
            _mapper.Map<VerifyForgotPasswordCommandResult, VerifyForgotPasswordResponse>(result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpPatch("reset-email")]
    public async Task<IActionResult> ResetEmail(
        [FromBody] ResetEmailRequest resetEmailRequest,
        CancellationToken cancellationToken
    )
    {
        // Map request command.
        Guid idUser = await _claimsUtil.ExtractIdUserFromClaims();

        ResetEmailCommand command = _mapper.Map<ResetEmailRequest, ResetEmailCommand>
        (
            resetEmailRequest,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) => dest.IdUser = idUser
            )
        );

        // Send to mediator.
        ResetEmailCommandResult result = await _sender.Send(command, cancellationToken);

        // Map from result to response.
        ResetEmailResponse response = _mapper.Map<ResetEmailCommandResult, ResetEmailResponse>(result);

        // Logout the user.
        await _cookieUtil.DeleteCookie(CookieNames.AccessToken);
        await _cookieUtil.DeleteCookie(CookieNames.RefreshToken);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("{resetEmailToken:guid}/verify-reset-email")]
    public async Task<IActionResult> VerifyResetEmail(
        [FromRoute] Guid resetEmailToken,
        CancellationToken cancellationToken
    )
    {
        // Map request to command.
        VerifyResetEmailCommand resetEmailCommand = _mapper.Map<Guid, VerifyResetEmailCommand>
        (
            resetEmailToken,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) => dest.ResetEmailToken = resetEmailToken
            )
        );

        // Send to mediator.
        VerifyResetEmailCommandResult result = await _sender.Send(resetEmailCommand, cancellationToken);

        // Map to response.
        VerifyResetEmailResponse response = _mapper.Map<VerifyResetEmailCommandResult, VerifyResetEmailResponse>
            (result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpPatch("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest resetPasswordRequest,
        CancellationToken cancellationToken
    )
    {
        // Map request to command.
        Guid idUser = await _claimsUtil.ExtractIdUserFromClaims();

        ResetPasswordCommand command = _mapper.Map<ResetPasswordRequest, ResetPasswordCommand>
        (
            resetPasswordRequest,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) => dest.IdUser = idUser
            )
        );

        // Send to mediator.
        ResetPasswordCommandResult result = await _sender.Send(command, cancellationToken);

        // Map to response.
        ResetPasswordResponse response = _mapper.Map<ResetPasswordCommandResult, ResetPasswordResponse>(result);

        // Logout the user.
        await _cookieUtil.DeleteCookie(CookieNames.AccessToken);
        await _cookieUtil.DeleteCookie(CookieNames.RefreshToken);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("{resetPasswordToken:guid}/verify-reset-password")]
    public async Task<IActionResult> VerifyResetPassword(
        [FromRoute] Guid resetPasswordToken,
        CancellationToken cancellationToken
    )
    {
        // Map to request data to command.
        VerifyResetPasswordCommand command = _mapper.Map<Guid, VerifyResetPasswordCommand>(resetPasswordToken);

        // Send to mediator.
        VerifyResetPasswordCommandResult result = await _sender.Send(command, cancellationToken);

        // Map result to response.
        VerifyResetPasswordResponse response =
            _mapper.Map<VerifyResetPasswordCommandResult, VerifyResetPasswordResponse>(result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpPatch("reset-phone-number")]
    public async Task<IActionResult> ResetAndVerifyPhoneNumber(
        [FromBody] ResetAndVerifyPhoneNumberRequest resetAndVerifyPhoneNumberRequest,
        CancellationToken cancellationToken
    )
    {
        // Map the request to a command.
        Guid idUser = await _claimsUtil.ExtractIdUserFromClaims();

        ResetAndVerifyPhoneNumberCommand command =
            _mapper.Map<ResetAndVerifyPhoneNumberRequest, ResetAndVerifyPhoneNumberCommand>
            (
                resetAndVerifyPhoneNumberRequest,
                opt => opt.AfterMap
                ((
                        src,
                        dest
                    ) => dest.IdUser = idUser
                )
            );

        // Send to mediator.
        ResetAndVerifyPhoneNumberCommandResult result = await _sender.Send(command, cancellationToken);

        // Map the result to response.
        ResetAndVerifyPhoneNumberResponse response =
            _mapper.Map<ResetAndVerifyPhoneNumberCommandResult, ResetAndVerifyPhoneNumberResponse>(result);

        return Ok(response);
    }
}