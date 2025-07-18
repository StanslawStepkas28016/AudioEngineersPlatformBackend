using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Application.Util.Cookies;
using AudioEngineersPlatformBackend.Contracts.Auth.CheckAuth;
using AudioEngineersPlatformBackend.Contracts.Auth.Login;
using AudioEngineersPlatformBackend.Contracts.Auth.Register;
using AudioEngineersPlatformBackend.Contracts.Auth.VerifyAccount;
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
        UserLog userLog = UserLog.Create();

        // Check database invariants - find a specified role by its name
        Role? role = await _authRepository.FindRoleByName(registerRequest.RoleName, cancellationToken);

        if (role == null)
        {
            throw new ArgumentException("Invalid role", nameof(registerRequest.RoleName));
        }

        // Create a User, then hash its password
        User user = User.Create(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email,
            registerRequest.PhoneNumber, registerRequest.Password, role!.IdRole, userLog.IdUserLog);

        user.SetHashedPassword(new PasswordHasher<User>().HashPassword(user, registerRequest.Password));

        // Add UserLog and User
        await _authRepository.AddUserLog(userLog, cancellationToken);
        await _authRepository.AddUser(user, cancellationToken);

        // Send a verification email
        await _emailService.TrySendVerificationEmailAsync(user.Email, user.FirstName, userLog.VerificationCode);

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new RegisterResponse(user.IdUser, user.Email);
    }

    public async Task<VerifyAccountResponse> VerifyAccount(VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken = default)
    {
        // Check database invariants - find if user with given id exits, find if verification code is valid
        User? user = await _authRepository.FindUserAndUserLogByVerificationCode(
            new VerificationCodeVo(verifyAccountRequest.VerificationCode).GetValidVerificationCode(),
            cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("Provided verification code is invalid",
                nameof(verifyAccountRequest.VerificationCode));
        }

        // Business logic - verify users account
        VerificationOutcome verificationOutcome = user.UserLog.VerifyUserAccount();

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        // If the token is expired inform about deletion of the user
        if (verificationOutcome == VerificationOutcome.VerificationCodeExpired)
        {
            throw new Exception("VerificationCode expired, User deleted");
        }

        return new VerifyAccountResponse(user!.IdUser);
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        // Check database invariants - find if user exists
        User? user = await _authRepository.FindUserAndUserLogAndRoleByEmail(
            new EmailVo(loginRequest.Email).GetValidEmail(),
            cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("User with provided email does not exist");
        }

        // Business logic - check if user is deleted or unverified
        user.UserLog.TryCheckUserStatus();

        // Business logic - set login associated data
        user.UserLog.SetLoginData(_tokenUtil.CreateNonJwtRefreshToken(), DateTime.UtcNow.AddDays(7));

        // Verify hashed password
        PasswordVerificationResult passwordVerificationResult =
            new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, loginRequest.Password);

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new ArgumentException("Invalid email or password"); //
        }

        // Create the Access Token
        JwtSecurityToken jwtAccessToken = _tokenUtil.CreateJwtAccessToken(user);

        // Write both tokens as cookies
        _cookieUtil.WriteAsCookie(CookieName.accessToken, new JwtSecurityTokenHandler().WriteToken(jwtAccessToken),
            jwtAccessToken.ValidTo);
        _cookieUtil.WriteAsCookie(CookieName.refreshToken, user.UserLog.RefreshToken!, user.UserLog.RefreshTokenExp);

        // Persist the changes in the database
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Send the user data in the response
        return new LoginResponse(
            user.IdUser,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Role.RoleName,
            user.Role.IdRole
        );
    }

    public Task Logout()
    {
        _cookieUtil.DeleteCookie(CookieName.accessToken);
        _cookieUtil.DeleteCookie(CookieName.refreshToken);
        return Task.CompletedTask;
    }

    public async Task RefreshToken(CancellationToken cancellationToken = default)
    {
        // Get the token from the cookie, method will throw an exception if the cookie is not present
        string refreshToken = _cookieUtil.TryGetCookie(CookieName.refreshToken);

        // Find a user by the refresh token
        User? user = await _authRepository.FindUserAndUserLogByRefreshToken(refreshToken, cancellationToken);

        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        // Check if the refresh token is expired - this should never happen, because the token is stored in the cookie
        // and the cookies are being deleted after their expiration date from the browser
        if (user.UserLog.RefreshTokenExp < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token expired, please login again");
        }

        // Set new login associated data (token and expiration date)
        user.UserLog.SetLoginData(_tokenUtil.CreateNonJwtRefreshToken(), DateTime.UtcNow.AddDays(7));

        // Generate a new access token
        JwtSecurityToken accessToken = _tokenUtil.CreateJwtAccessToken(user);

        // Write new access token and refresh token as cookies
        _cookieUtil.WriteAsCookie(CookieName.accessToken, new JwtSecurityTokenHandler().WriteToken(accessToken),
            accessToken.ValidTo);
        _cookieUtil.WriteAsCookie(CookieName.refreshToken, user.UserLog.RefreshToken!, user.UserLog.RefreshTokenExp);

        // Persist the changes in the database
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<CheckAuthResponse> CheckAuth(Guid idUser, CancellationToken cancellationToken = default)
    {
        // Validate the idUser
        if (idUser == Guid.Empty)
        {
            throw new ArgumentException("Provided idUser is empty");
        }

        UserAssociatedDataDto? userAssociatedData = await _authRepository.GetUserAssociatedDataByIdUser(idUser, cancellationToken);

        // This should never happen, since the idUser is being pulled from the JWT token that resides
        // in an attached cookie, but just in case
        if (userAssociatedData == null)
        {
            throw new ArgumentException("User does not exist", nameof(idUser));
        }

        return new CheckAuthResponse(
            idUser,
            userAssociatedData.Email!,
            userAssociatedData.FirstName!,
            userAssociatedData.LastName!,
            userAssociatedData.PhoneNumber!,
            userAssociatedData.IdRole,
            userAssociatedData.RoleName!
        );
    }
}