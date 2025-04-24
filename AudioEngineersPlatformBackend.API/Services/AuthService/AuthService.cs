using System;
using System.Threading;
using System.Threading.Tasks;
using AudioEngineersPlatformBackend.Dtos.Auth.Login;
using AudioEngineersPlatformBackend.Dtos.Auth.Register;
using AudioEngineersPlatformBackend.Dtos.Auth.Utilities;
using AudioEngineersPlatformBackend.Dtos.Auth.VerifyEmail;
using AudioEngineersPlatformBackend.Exceptions;
using AudioEngineersPlatformBackend.Helpers;
using AudioEngineersPlatformBackend.Models;
using AudioEngineersPlatformBackend.Repositories.AuthRepository;
using AudioEngineersPlatformBackend.Resources;
using AudioEngineersPlatformBackend.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AudioEngineersPlatformBackend.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IEmailService _emailService;
    private readonly AuthVariables _authVariables;
    private readonly MasterContext _context;

    public class AuthVariables
    {
        public string AdminSecret { get; set; }
    }

    public AuthService(IAuthRepository authRepository, IOptions<AuthVariables> authVariables,
        IEmailService emailService, MasterContext context)
    {
        _authRepository = authRepository;
        _emailService = emailService;
        _authVariables = authVariables.Value;
        _context = context;
    }

    public async Task<RegisterResponseDto> Register(RegisterRequestDto registerRequestDto,
        CancellationToken cancellationToken)
    {
        // Check for nulls or empty strings
        if (string.IsNullOrEmpty(registerRequestDto.FirstName)
            || string.IsNullOrEmpty(registerRequestDto.LastName)
            || string.IsNullOrEmpty(registerRequestDto.Email)
            || string.IsNullOrEmpty(registerRequestDto.Username)
            || string.IsNullOrEmpty(registerRequestDto.PhoneNumber)
            || string.IsNullOrEmpty(registerRequestDto.Password)
            || string.IsNullOrEmpty(registerRequestDto.RoleName))
        {
            throw new LocalizedGeneralException(ErrorMessages.ProvidedDataIsNullOrEmpty);
        }

        // Check if username is already taken
        var userByUsername = await _authRepository.FindUserByUsername(registerRequestDto.Username, cancellationToken);

        if (userByUsername is not null)
        {
            throw new LocalizedGeneralException(ErrorMessages.UsernameAlreadyTaken, registerRequestDto.Username);
        }

        // Validate email
        var isValidEmail = RegexValidators.IsValidEmail(registerRequestDto.Email);

        if (!isValidEmail)
        {
            throw new LocalizedGeneralException(ErrorMessages.EmailIncorrect);
        }

        // Check if email is already taken
        var userByEmail = await _authRepository.FindUserByEmail(registerRequestDto.Email, cancellationToken);

        if (userByEmail is not null)
        {
            throw new LocalizedGeneralException(ErrorMessages.EmailAlreadyTaken);
        }

        // Validate phoneNumber (needs to be provided with a code ex. +48 to be correct)
        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        var parsedNumber = phoneNumberUtil.Parse(registerRequestDto.PhoneNumber, null);

        if (!phoneNumberUtil.IsValidNumber(parsedNumber))
        {
            throw new LocalizedGeneralException(ErrorMessages.PhoneNumberIncorrect);
        }

        // Check if phoneNumber is already taken
        var userByPhoneNumber =
            await _authRepository.FindUserByPhoneNumber(registerRequestDto.PhoneNumber, cancellationToken);

        if (userByPhoneNumber is not null)
        {
            throw new LocalizedGeneralException(ErrorMessages.PhoneNumberAlreadyTaken);
        }

        // Find a role by name
        var findRoleByName = await _authRepository.FindRoleByName(registerRequestDto.RoleName, cancellationToken);

        if (findRoleByName is null)
        {
            throw new LocalizedGeneralException(ErrorMessages.RoleNotFound, registerRequestDto.RoleName);
        }

        // See if an admin tries to register
        if (!string.IsNullOrEmpty(registerRequestDto.AdminSecret) &&
            _authVariables.AdminSecret != registerRequestDto.AdminSecret)
        {
            throw new LocalizedGeneralException(ErrorMessages.AdminSecretInvalid);
        }

        // Create a userLog
        var userLog = new UserLog
        {
            DateCreated = DateTime.Now,
            DateDeleted = null,
            IsDeleted = false,
            VerificationCode = SecurityHelpers.GenerateVerificationCode(),
            VerificationCodeExpiration = DateTime.Now.AddHours(24),
            IsVerified = false,
            DateLastLogin = null,
        };

        // Create a new user
        var user = new User
        {
            FirstName = registerRequestDto.FirstName,
            LastName = registerRequestDto.LastName,
            Username = registerRequestDto.Username,
            Email = registerRequestDto.Email,
            PhoneNumber = registerRequestDto.PhoneNumber,
            Password = "",
            IdRole = findRoleByName.IdRole,
        };

        // Hash the password
        var hashedPassword = new PasswordHasher<User>().HashPassword(user, registerRequestDto.Password);
        user.Password = hashedPassword;

        // Store user and userLog in database
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        User storedUser;
        try
        {
            var storedUserLog = await _authRepository.StoreUserLog(userLog, cancellationToken);
            user.IdUserLog = storedUserLog!.IdUserLog;
            storedUser = (await _authRepository.StoreUser(user, cancellationToken))!;
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        // Send a register verification email
        await _emailService.SendVerificationEmailAsync(
            new VerifyEmailUserDataDto
            {
                Email = user.Email,
                Username = user.Username,
                VerificationCode = userLog.VerificationCode
            },
            cancellationToken);

        return new RegisterResponseDto
        {
            IdUser = storedUser!.IdUser,
            FirstName = storedUser.FirstName,
            LastName = storedUser.LastName,
            Username = storedUser.Username
        };
    }

    public async Task<VerifyEmailResponseDto> VerifyEmail(VerifyEmailRequestDto verifyEmailRequestDto,
        CancellationToken cancellationToken)
    {
        // Check for nulls
        if (string.IsNullOrEmpty(verifyEmailRequestDto.VerificationCode))
        {
            throw new LocalizedGeneralException(ErrorMessages.ProvidedDataIsNullOrEmpty);
        }

        // Check if the verificationCode is structured right (6 digits)
        if (verifyEmailRequestDto.VerificationCode.Length != 6)
        {
            throw new LocalizedGeneralException(ErrorMessages.VerificationCodeInvalidFormat);
        }

        // Check if the verificationCode is valid (meaning, it is stored the database and not being null)
        var userLog =
            await _authRepository.FindUserLogByVerificationCode(verifyEmailRequestDto.VerificationCode,
                cancellationToken);

        if (userLog is null)
        {
            throw new LocalizedGeneralException(ErrorMessages.VerificationCodeInvalid);
        }

        // Check if the verificationCode is expired
        if (userLog.VerificationCodeExpiration < DateTime.Now)
        {
            throw new LocalizedGeneralException(ErrorMessages.VerificationCodeExpired);
        }

        // Adjust all the data associated (IsVerified = true, VerificationCode = null, VerificationCodeExpiration = null)  
        await _authRepository.SetUserLogToVerifiedAndAdjustAssociatedData(userLog.IdUserLog, cancellationToken);

        // Send a welcome email...

        return new VerifyEmailResponseDto()
        {
            Message = "Successfully verified your email address",
        };
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        // Check for nulls or empty strings
        if (string.IsNullOrEmpty(loginRequestDto.Email) || string.IsNullOrEmpty(loginRequestDto.Password))
        {
            throw new LocalizedArgumentException(ErrorMessages.ProvidedDataIsNullOrEmpty);
        }

        // Check if the user with the specified email exists
        var userByEmail = await _authRepository.FindUserByEmail(loginRequestDto.Email, cancellationToken);

        if (userByEmail is null)
        {
            throw new LocalizedArgumentException(ErrorMessages.UserWithSpecifiedEmailNotFound, loginRequestDto.Email);
        }

        // Check if the user is unverified 
        var userLogByIdUserLog =
            await _authRepository.FindUserLogByIdUserLog(userByEmail.IdUserLog, cancellationToken);

        if (!userLogByIdUserLog!.IsVerified)
        {
            throw new LocalizedGeneralException($"User is not verified");
        }

        // Check if the user is deleted 
        if (userLogByIdUserLog.IsDeleted)
        {
            throw new LocalizedGeneralException($"User is deleted");
        }

        // See if the provided password is correct
        var verificationResult =
            new PasswordHasher<User>().VerifyHashedPassword(userByEmail, userByEmail.Password,
                loginRequestDto.Password);

        if (verificationResult != PasswordVerificationResult.Success)
        {
            throw new LocalizedGeneralException($"Password {loginRequestDto.Password} is invalid");
        }

        // Set the LastLoginDate in UserLog Table
        await _authRepository.SetUserLogLastLoginDateByIdUserLog(userByEmail.IdUserLog, cancellationToken);

        return new LoginResponseDto
        {
            Token = "Logged in!"
        };
    }
}