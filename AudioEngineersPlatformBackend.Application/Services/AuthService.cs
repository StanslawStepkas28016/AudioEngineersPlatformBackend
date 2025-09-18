using System.IdentityModel.Tokens.Jwt;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Application.Util.Cookies;
using AudioEngineersPlatformBackend.Contracts.Auth.CheckAuth;
using AudioEngineersPlatformBackend.Contracts.Auth.Login;
using AudioEngineersPlatformBackend.Contracts.Auth.Register;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetEmail;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetPassword;
using AudioEngineersPlatformBackend.Contracts.Auth.ResetPhoneNumber;
using AudioEngineersPlatformBackend.Contracts.Auth.VerifyAccount;
using AudioEngineersPlatformBackend.Contracts.Auth.VerifyForgotPassword;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AudioEngineersPlatformBackend.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly ICookieUtil _cookieUtil;
    private readonly ICurrentUserUtil _currentUserUtil;
    private readonly ISESService _sesService;
    private readonly ITokenUtil _tokenUtil;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUrlGeneratorUtil _urlGeneratorUtil;
    private readonly IUserRepository _userRepository;

    public AuthService(
        ISESService sesService,
        IAuthRepository authRepository,
        IUnitOfWork unitOfWork,
        ITokenUtil tokenUtil,
        ICookieUtil cookieUtil,
        ICurrentUserUtil currentUserUtil,
        IUserRepository userRepository,
        IUrlGeneratorUtil urlGeneratorUtil
    )
    {
        _sesService = sesService;
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
        _tokenUtil = tokenUtil;
        _cookieUtil = cookieUtil;
        _currentUserUtil = currentUserUtil;
        _userRepository = userRepository;
        _urlGeneratorUtil = urlGeneratorUtil;
    }

    public async Task<RegisterResponse> Register(
        RegisterRequest registerRequest,
        CancellationToken cancellationToken
    )
    {
        // Check if the provided email or phone number is already used.
        if (await _authRepository.FindUserByEmailAsNoTrackingAsync
            (
                new EmailVo(registerRequest.Email).Email,
                cancellationToken
            ) != null
           )
        {
            throw new ArgumentException
            (
                $"Provided {nameof(registerRequest.Email)
                    .ToLower()} is already taken."
            );
        }

        string validPhoneNumber = new PhoneNumberVo(registerRequest.PhoneNumber).PhoneNumber;

        if (await _authRepository.IsPhoneNumberAlreadyTaken(validPhoneNumber, cancellationToken))
        {
            throw new ArgumentException
            (
                $"Provided {nameof(registerRequest.Email)
                    .ToLower()} is already taken."
            );
        }

        // Create a UserAuthLog.
        UserAuthLog userAuthLog = UserAuthLog.Create();

        // Check if the provided role exists.
        Role? role = await _authRepository.FindRoleByNameAsNoTrackingAsync(registerRequest.RoleName, cancellationToken);

        if (role == null)
        {
            throw new ArgumentException($"Invalid {nameof(registerRequest.RoleName)}.");
        }

        // Create a User, then hash its password
        User user = User.Create
        (
            registerRequest.FirstName,
            registerRequest.LastName,
            registerRequest.Email,
            registerRequest.PhoneNumber,
            registerRequest.Password,
            role.IdRole,
            userAuthLog.IdUserAuthLog
        );

        // Hash and set the password
        user.SetHashedPassword(new PasswordHasher<User>().HashPassword(user, registerRequest.Password));

        // Add UserAuthLog and User
        await _authRepository.AddUserLogAsync(userAuthLog, cancellationToken);
        await _authRepository.AddUserAsync(user, cancellationToken);

        // Send a verification email
        await _sesService.SendRegisterVerificationEmailAsync(user.Email, user.FirstName, userAuthLog.VerificationCode);

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new RegisterResponse(user.IdUser, user.Email);
    }

    public async Task<VerifyAccountResponse> VerifyAccount(
        VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken = default
    )
    {
        // Check database invariants - find if user with given id exits, find if verification code is valid
        string validAccountVerificationCode =
            new AccountVerificationCodeVo(verifyAccountRequest.VerificationCode).VerificationCode;

        User? user =
            await _authRepository.FindUserAndUserLogByVerificationCodeAsync
            (
                validAccountVerificationCode,
                cancellationToken
            );

        if (user == null)
        {
            throw new ArgumentException($"Provided {nameof(verifyAccountRequest.VerificationCode)} is invalid.");
        }

        // Verify the users account
        VerificationOutcome verificationOutcome = user.UserAuthLog.VerifyUserAccount();

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        // If the token is expired inform about deletion of the user
        if (verificationOutcome == VerificationOutcome.VerificationCodeExpired)
        {
            throw new Exception($"{nameof(verifyAccountRequest.VerificationCode)} expired, User deleted.");
        }

        return new VerifyAccountResponse(user.IdUser);
    }

    public async Task<LoginResponse> Login(
        LoginRequest loginRequest,
        CancellationToken cancellationToken = default
    )
    {
        // Check database invariants - find if user exists.
        User? user = await _authRepository.FindUserAndUserLogAndRoleByEmailAsync
        (
            new EmailVo(loginRequest.Email).Email,
            cancellationToken
        );

        if (user == null)
        {
            throw new ArgumentException($"User with provided {nameof(loginRequest.Email)} does not exist.");
        }

        // Ensure that the user is neither deleted nor unverified.
        user.UserAuthLog.EnsureCorrectUserStatus();

        // Set login associated data.
        user.UserAuthLog.SetLoginData(_tokenUtil.CreateNonJwtRefreshToken(), DateTime.UtcNow.AddDays(7));

        // Verify the input password against the database.
        PasswordVerificationResult passwordVerificationResult =
            new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, loginRequest.Password);

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new ArgumentException($"Invalid {nameof(loginRequest.Email)} or {nameof(loginRequest.Password)}.");
        }

        // Create an Access Token.
        JwtSecurityToken jwtAccessToken = _tokenUtil.CreateJwtAccessToken(user);

        // Write both tokens as cookies.
        await _cookieUtil.WriteAsCookie
        (
            CookieNames.accessToken,
            new JwtSecurityTokenHandler().WriteToken(jwtAccessToken),
            jwtAccessToken.ValidTo
        );
        await _cookieUtil.WriteAsCookie
        (
            CookieNames.refreshToken,
            user.UserAuthLog.RefreshToken!,
            user.UserAuthLog.RefreshTokenExpiration
        );

        // Persist the changes in the database.
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Send the user data in the response.
        return new LoginResponse
        (
            user.IdUser,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Role.RoleName,
            user.Role.IdRole
        );
    }

    public async Task Logout()
    {
        // Remove both cookies from the clients browser, effectively logging them out. 
        await _cookieUtil.DeleteCookie(CookieNames.accessToken);
        await _cookieUtil.DeleteCookie(CookieNames.refreshToken);
    }

    public async Task RefreshToken(
        CancellationToken cancellationToken = default
    )
    {
        // Get the token from requests cookies.
        string refreshToken = await _cookieUtil.GetCookie(CookieNames.refreshToken);

        // Find a user by the refresh token.
        User? user = await _authRepository.FindUserAndUserLogByRefreshTokenAsync(refreshToken, cancellationToken);

        if (user == null)
        {
            throw new Exception("User does not exist.");
        }

        // Check if the refresh token is expired - this should never happen, because the token is stored in the cookie
        // and the cookies are being deleted after their expiration date from the browser.
        if (user.UserAuthLog.RefreshTokenExpiration < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException($"{nameof(refreshToken)} expired, please login again.");
        }

        // Set new login associated data (token and expiration date).
        user.UserAuthLog.SetLoginData(_tokenUtil.CreateNonJwtRefreshToken(), DateTime.UtcNow.AddDays(7));

        // Generate a new access token.
        JwtSecurityToken accessToken = _tokenUtil.CreateJwtAccessToken(user);

        // Write new access token and refresh token as cookies.
        await _cookieUtil.WriteAsCookie
        (
            CookieNames.accessToken,
            new JwtSecurityTokenHandler().WriteToken(accessToken),
            accessToken.ValidTo
        );
        await _cookieUtil.WriteAsCookie
        (
            CookieNames.refreshToken,
            user.UserAuthLog.RefreshToken!,
            user.UserAuthLog.RefreshTokenExpiration
        );

        // Persist the changes in the database.
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<CheckAuthResponse> CheckAuth(
        Guid idUser,
        CancellationToken cancellationToken = default
    )
    {
        // Validate the idUser
        if (idUser == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(idUser)} cannot be empty.");
        }

        UserAssociatedDataDto? userAssociatedData =
            await _authRepository.GetUserAssociatedDataByIdUserAsync(idUser, cancellationToken);

        // This should never happen, since the idUser is being pulled from the JWT token that resides
        // in an attached cookie, but just in case
        if (userAssociatedData == null)
        {
            throw new ArgumentException($"User with the provided {nameof(idUser)} does not exist.");
        }

        return new CheckAuthResponse
        (
            idUser,
            userAssociatedData.Email!,
            userAssociatedData.FirstName!,
            userAssociatedData.LastName!,
            userAssociatedData.PhoneNumber!,
            userAssociatedData.IdRole,
            userAssociatedData.RoleName!
        );
    }

    public async Task<ResetEmailResponse> ResetEmail(
        Guid idUser,
        ResetEmailRequest resetEmailRequest,
        CancellationToken cancellationToken
    )
    {
        // Ensure the right format of the provided email (will throw an exception if invalid).
        string newValidEmail = new EmailVo(resetEmailRequest.NewEmail).Email;

        // Ensure the provided id is correct.
        Guid idUserValidated = new GuidVo(idUser).Guid;

        // Check if the user is authorized to edit the advert (either the owner or an administrator).
        if (idUserValidated != _currentUserUtil.IdUser && !_currentUserUtil.IsAdministrator)
        {
            throw new UnauthorizedAccessException("Specified data does not belong to you.");
        }

        // Validate if the user exists.
        if (!await _userRepository.DoesUserExistByIdUserAsync(idUserValidated, cancellationToken))
        {
            throw new Exception("User not found.");
        }

        // Fetch the user data.
        User user = (await _userRepository.FindUserByIdUserAsync(idUserValidated, cancellationToken))!;

        // Check if the email is already in use.
        if (await _userRepository.IsEmailAlreadyTakenAsync(newValidEmail, cancellationToken))
        {
            throw new Exception($"{nameof(resetEmailRequest.NewEmail)} is already taken.");
        }

        // Fetch the UserAuthLog data.
        var userLog = (await _userRepository.FindUserLogByIdUserAsync(idUserValidated, cancellationToken))!;

        // Generate a token and generate a reset email url.
        var resetEmailToken = Guid.NewGuid();
        var resetEmailUrl = await _urlGeneratorUtil.GenerateResetVerificationUrl(resetEmailToken, "verify-reset-email");

        // Update the email itself and set email reset data (token, its expiration and user state).
        user.ResetEmail(newValidEmail);
        userLog.SetResetEmailData(resetEmailToken);

        // Logout the user from all sessions, by setting the RefreshToken and RefreshTokenExpiration to null values,
        // as well as removing the authentication cookies from their browser.
        userLog.SetLogoutData();
        await _cookieUtil.DeleteCookie(CookieNames.accessToken);
        await _cookieUtil.DeleteCookie(CookieNames.refreshToken);

        // Send a new verification email.
        await _sesService.SendEmailResetEmailAsync(newValidEmail, user.FirstName, resetEmailUrl);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ResetEmailResponse(resetEmailUrl);
    }

    public async Task VerifyResetEmail(
        Guid resetEmailToken,
        CancellationToken cancellationToken
    )
    {
        // Use a VO to extract the guid and ensure that it is not empty.
        var resetEmailTokenValidated = new GuidVo(resetEmailToken).Guid;

        // Find the user associated UserAuthLog.
        var userLog =
            await _authRepository.FindUserLogByResetEmailTokenAsync(resetEmailTokenValidated, cancellationToken);

        if (userLog == null)
        {
            throw new Exception($"User with the {nameof(resetEmailToken)} was not found.");
        }

        // Verify the token, including its expiration and user flags.
        userLog.VerifyResetEmailData(resetEmailTokenValidated);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task ResetPassword(
        Guid idUser,
        ResetPasswordRequest resetPasswordRequest,
        CancellationToken cancellationToken
    )
    {
        // Ensure the provided id is correct.
        Guid idUserValidated = new GuidVo(idUser).Guid;

        User? userById = await _userRepository.FindUserByIdUserAsync(idUserValidated, cancellationToken);

        // Ensure the user exists.
        if (userById == null)
        {
            throw new ArgumentException($"Provided {nameof(idUser)} is incorrect.");
        }

        // Check if the user is authorized to reset their email (either an admin or the user himself).
        if (idUserValidated != _currentUserUtil.IdUser && !_currentUserUtil.IsAdministrator)
        {
            throw new UnauthorizedAccessException("Specified data does not belong to you.");
        }

        // Change the password, ensuring the logic is correct inside the used method and set the reset password data,
        // so that the user is unable to login until they verify their new password. 
        UserAuthLog? userLog = await _userRepository.FindUserLogByIdUserAsync(idUserValidated, cancellationToken);

        if (userLog == null)
        {
            throw new Exception($"{nameof(userLog)} was not found.");
        }

        Guid resetPasswordToken = Guid.NewGuid();

        // Reset the password, ensuring the current password is correct and that new passwords are correct.
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        PasswordVerificationResult passwordVerificationResult = passwordHasher.VerifyHashedPassword
            (userById, userById.Password, resetPasswordRequest.CurrentPassword);

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new Exception($"{nameof(resetPasswordRequest.CurrentPassword)} is invalid.");
        }

        if (resetPasswordRequest.NewPassword != resetPasswordRequest.NewPasswordRepeated)
        {
            throw new Exception
            (
                $"{nameof(resetPasswordRequest.NewPassword)} and {nameof(resetPasswordRequest.NewPasswordRepeated)
                } do not match."
            );
        }

        userById.SetHashedPassword(passwordHasher.HashPassword(userById, resetPasswordRequest.NewPassword));
        userLog.SetResetPasswordData(resetPasswordToken);

        // Send an email with a confirmation link.
        string resetPasswordUrl = await _urlGeneratorUtil.GenerateResetVerificationUrl
            (resetPasswordToken, "verify-reset-password");

        await _sesService.SendPasswordResetEmailAsync(userById.Email, userById.FirstName, resetPasswordUrl);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task VerifyResetPassword(
        Guid resetPasswordToken,
        CancellationToken cancellationToken
    )
    {
        // Use a VO to extract the guid and ensure that it is not empty.
        Guid resetPasswordTokenValidated = new GuidVo(resetPasswordToken).Guid;

        UserAuthLog? userLog = await _authRepository.FindUserLogByResetPasswordTokenAsync
            (resetPasswordTokenValidated, cancellationToken);

        if (userLog == null)
        {
            throw new Exception($"{nameof(User)} with the provided {nameof(resetPasswordToken)} does not exist.");
        }

        // Verify the token, including its expiration and user flags.
        userLog.VerifyResetPasswordData(resetPasswordTokenValidated);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task ResetPhoneNumber(
        Guid idUser,
        ResetPhoneNumberRequest resetPhoneNumberRequest,
        CancellationToken cancellationToken
    )
    {
        // Ensure the provided input is correct.
        Guid idUserValidated = new GuidVo(idUser).Guid;

        string phoneNumberValidated = new PhoneNumberVo(resetPhoneNumberRequest.NewPhoneNumber).PhoneNumber;

        // Check if the user is authorized to change their phone number (either the user himself or an administrator).
        if (idUserValidated != _currentUserUtil.IdUser && !_currentUserUtil.IsAdministrator)
        {
            throw new UnauthorizedAccessException("Specified data does not belong to you.");
        }

        // Check if the number is already taken.
        if (await _authRepository.IsPhoneNumberAlreadyTaken(phoneNumberValidated, cancellationToken))
        {
            throw new ArgumentException($"Provided {nameof(resetPhoneNumberRequest.NewPhoneNumber)} is already taken.");
        }

        User? userByIdUser = await _userRepository.FindUserByIdUserAsync(idUser, cancellationToken);

        if (userByIdUser == null)
        {
            throw new ArgumentException($"{nameof(User)} with the provided ${nameof(idUser)} was not found.");
        }

        // Change the number.
        userByIdUser.ChangePhoneNumber(phoneNumberValidated);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<Guid> ForgotPassword(
        string email,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("All arguments must be provided.");
        }

        // Find the user and its auth log by email.
        User? userAndUserLogByEmail = await _authRepository.FindUserAndUserLogAndRoleByEmailAsync
            (email, cancellationToken);

        if (userAndUserLogByEmail == null)
        {
            throw new ArgumentException($"User with specified {nameof(email)} not found");
        }

        // Set forgot password data.
        Guid forgotPasswordToken = Guid.NewGuid();
        userAndUserLogByEmail.UserAuthLog.SetForgotPasswordData(forgotPasswordToken);

        // Generate a verification url.
        string url = await _urlGeneratorUtil.GenerateResetVerificationUrl
            (forgotPasswordToken, "verify-forgot-password");

        // Email the user.
        await _sesService.SendForgotPasswordResetEmailAsync
            (userAndUserLogByEmail.Email, userAndUserLogByEmail.FirstName, url);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return forgotPasswordToken;
    }

    public async Task VerifyForgotPassword(
        Guid forgotPasswordToken,
        VerifyForgotPasswordRequest verifyForgotPasswordRequest,
        CancellationToken cancellationToken
    )
    {
        User? userAndUserLog = await _authRepository.FindUserAndUserLogByForgotPasswordToken
            (forgotPasswordToken, cancellationToken);

        throw new NotImplementedException();
    }
}