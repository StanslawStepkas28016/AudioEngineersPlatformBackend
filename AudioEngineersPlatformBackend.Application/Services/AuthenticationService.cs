using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Authentication;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AudioEngineersPlatformBackend.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IEmailService _emailService;
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJWTFactory _jwtFactory;

    public AuthenticationService(IEmailService emailService, IAuthenticationRepository authenticationRepository,
        IUnitOfWork unitOfWork, IJWTFactory jwtFactory)
    {
        _emailService = emailService;
        _authenticationRepository = authenticationRepository;
        _unitOfWork = unitOfWork;
        _jwtFactory = jwtFactory;
    }

    public async Task<RegisterResponse> Register(RegisterRequest registerRequest,
        CancellationToken cancellationToken)
    {
        // Check database invariants - find if email or phone number is already used
        if (await _authenticationRepository.FindUserByEmail(new EmailVO(registerRequest.Email).GetValidEmail(),
                cancellationToken) != null)
        {
            throw new ArgumentException("Provided email is already taken");
        }

        if (await _authenticationRepository.FindUserByPhoneNumber(
                new PhoneNumberVO(registerRequest.PhoneNumber).GetValidPhoneNumber(), cancellationToken) !=
            null)
        {
            throw new ArgumentException("Provided phone number is already taken");
        }

        // Create a UserLog
        var userLog = new UserLog();

        // Check database invariants - find a specified role by its name
        var role = await _authenticationRepository.FindRoleByName(registerRequest.RoleName, cancellationToken);

        if (role == null)
        {
            throw new ArgumentException("Invalid role", nameof(registerRequest.RoleName));
        }

        // Create a User, then hash its password
        var user = new User(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email,
            registerRequest.PhoneNumber, registerRequest.Password, role!.IdRole, userLog.IdUserLog);

        user.SetHashedPassword(new PasswordHasher<User>().HashPassword(user, registerRequest.Password));

        // Add UserLog and User
        await _authenticationRepository.AddUserLog(userLog, cancellationToken);
        await _authenticationRepository.AddUser(user, cancellationToken);

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
        var user = await _authenticationRepository.FindUserAndUserLogByIdAndVerificationCode(
            new GeneralIdVO(verifyAccountRequest.IdUser).GetValidId(),
            new VerificationCodeVO(verifyAccountRequest.VerificationCode).GetValidVerificationCode(),
            cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("Provided verification code is invalid",
                nameof(verifyAccountRequest.VerificationCode));
        }

        // Business logic - verify users account
        var verificationOutcome = user.UserLog.VerifyUserAccount();

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
        var user = await _authenticationRepository.FindUserAndUserLogAndRoleByEmail(new EmailVO(loginRequest.Email).GetValidEmail(),
            cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("Invalid email", nameof(loginRequest.Email));
        }

        // Business logic - check if user is deleted or unverified
        user.UserLog.TryCheckUserStatus();
        
        // Business logic - set login associated data
        user.UserLog.SetDateLastLogin();
        
        // Verify hashed password
        var passwordVerificationResult =
            new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, loginRequest.Password);

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new ArgumentException("Invalid password", nameof(loginRequest.Email));
        }

        // Create a JWT
        var token = _jwtFactory.CreateJWT(user);

        return new LoginResponse(user.IdUser, token);
    }
}