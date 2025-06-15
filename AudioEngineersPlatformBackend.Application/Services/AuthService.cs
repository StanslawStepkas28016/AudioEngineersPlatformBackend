using System.IdentityModel.Tokens.Jwt;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Auth;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AudioEngineersPlatformBackend.Application.Services;

public class AuthService : IAuthService
{
    private readonly IEmailService _emailService;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenUtil _tokenUtil;
    private readonly ICookieUtil _cookieUtil;

    public AuthService(IEmailService emailService, IAuthRepository authRepository,
        IUnitOfWork unitOfWork, ITokenUtil tokenUtil, ICookieUtil cookieUtil)
    {
        _emailService = emailService;
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
        _tokenUtil = tokenUtil;
        _cookieUtil = cookieUtil;
    }

    public async Task<RegisterResponse> Register(RegisterRequest registerRequest,
        CancellationToken cancellationToken)
    {
        // Check database invariants - find if email or phone number is already used
        if (await _authRepository.FindUserByEmail(new EmailVo(registerRequest.Email).GetValidEmail(),
                cancellationToken) != null)
        {
            throw new ArgumentException("Provided email is already taken");
        }

        if (await _authRepository.FindUserByPhoneNumber(
                new PhoneNumberVo(registerRequest.PhoneNumber).GetValidPhoneNumber(), cancellationToken) !=
            null)
        {
            throw new ArgumentException("Provided phone number is already taken");
        }

        // Create a UserLog
        var userLog = UserLog.Create();

        // Check database invariants - find a specified role by its name
        var role = await _authRepository.FindRoleByName(registerRequest.RoleName, cancellationToken);

        if (role == null)
        {
            throw new ArgumentException("Invalid role", nameof(registerRequest.RoleName));
        }

        // Create a User, then hash its password
        var user = new User(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email,
            registerRequest.PhoneNumber, registerRequest.Password, role!.IdRole, userLog.IdUserLog);

        user.SetHashedPassword(new PasswordHasher<User>().HashPassword(user, registerRequest.Password));

        // Add UserLog and User
        await _authRepository.AddUserLog(userLog, cancellationToken);
        await _authRepository.AddUser(user, cancellationToken);

        // Send a verification email
        await _emailService.TrySendVerificationEmailAsync(user.Email, user.FirstName, userLog.VerificationCode);

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new RegisterResponse(user.IdUser, user.Email, "");
    }

    public async Task<VerifyAccountResponse> VerifyAccount(VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken = default)
    {
        // Check database invariants - find if user with given id exits, find if verification code is valid
        var user = await _authRepository.FindUserAndUserLogByVerificationCode(
            new VerificationCodeVo(verifyAccountRequest.VerificationCode).GetValidVerificationCode(),
            cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("Provided verification code is invalid",
                nameof(verifyAccountRequest.VerificationCode));
        }

        // Business logic - verify users account
        var verificationOutcome = user.UserLog.VerifyUserAccount();

        // Exception rzucać w klasie biznesowej!!!!

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        // If the token is expired inform about deletion of the user
        if (verificationOutcome == VerificationOutcome.VerificationCodeExpired)
        {
            throw new Exception("Verification code expired, user deleted");
        }

        return new VerifyAccountResponse(user!.IdUser);
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        // Check database invariants - find if user exists
        var user = await _authRepository.FindUserAndUserLogAndRoleByEmail(
            new EmailVo(loginRequest.Email).GetValidEmail(),
            cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("User with provided email does not exist", nameof(loginRequest.Email));
        }

        // Business logic - check if user is deleted or unverified
        user.UserLog.TryCheckUserStatus();

        // Business logic - set login associated data
        user.UserLog.SetLoginData(_tokenUtil.CreateNonJwtRefreshToken(), DateTime.UtcNow.AddDays(7));

        // Verify hashed password
        var passwordVerificationResult =
            new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, loginRequest.Password);

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new ArgumentException("Invalid password", nameof(loginRequest.Email));
        }

        // Create the Access Token
        var jwtAccessToken = _tokenUtil.CreateJwtAccessToken(user);

        // Write both tokens as cookies
        _cookieUtil.WriteAsCookie("accessToken", new JwtSecurityTokenHandler().WriteToken(jwtAccessToken),
            jwtAccessToken.ValidTo);
        _cookieUtil.WriteAsCookie("refreshToken", user.UserLog.RefreshToken!, user.UserLog.RefreshTokenExp);

        // Persist the changes in the database
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Send the access token in the normal repose
        return new LoginResponse(user.IdUser);
    }

    public Task Logout()
    {
        _cookieUtil.DeleteCookie("refreshToken");
        _cookieUtil.DeleteCookie("accessToken");
        return Task.CompletedTask;
    }

    public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest,
        CancellationToken cancellationToken = default)
    {
        // Get the token from the cookie
        var refreshToken = _cookieUtil.TryGetCookie("refreshToken");

        // See if the refresh token is valid
        var user = await _authRepository.FindUserAndUserLogByIdUser(refreshTokenRequest.IdUser, cancellationToken);

        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        if (user.UserLog.RefreshToken != refreshToken)
        {
            throw new Exception("Invalid refresh token");
        }

        if (user.UserLog.RefreshTokenExp < DateTime.UtcNow)
        {
            throw new Exception("Refresh token expired, please login again");
        }

        // Persists new refresh token and its expiration date in the database
        user.UserLog.SetLoginData(_tokenUtil.CreateNonJwtRefreshToken(), DateTime.UtcNow.AddDays(7));

        // Generate a new access token
        var accessToken = _tokenUtil.CreateJwtAccessToken(user);

        // Write new access token and refresh token as cookies
        _cookieUtil.WriteAsCookie("accessToken", new JwtSecurityTokenHandler().WriteToken(accessToken),
            accessToken.ValidTo);
        _cookieUtil.WriteAsCookie("refreshToken", user.UserLog.RefreshToken, user.UserLog.RefreshTokenExp);

        // Persist the changes in the database
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Send the access token in the normal response
        return new RefreshTokenResponse(user.IdUser);
    }

    public async Task<CheckAuthResponse> CheckAuth(Guid idUser, CancellationToken cancellationToken = default)
    {
        var userAssociatedData = await _authRepository.GetUserAssociatedData(idUser, cancellationToken);

        // This should never happen, but just in case
        if (userAssociatedData == null)
        {
            throw new ArgumentException("User does not exist", nameof(idUser));
        }

        return new CheckAuthResponse(
            idUser,
            userAssociatedData.Email,
            userAssociatedData.FirstName,
            userAssociatedData.LastName,
            userAssociatedData.PhoneNumber,
            userAssociatedData.IdRole,
            userAssociatedData.RoleName
        );
    }
}