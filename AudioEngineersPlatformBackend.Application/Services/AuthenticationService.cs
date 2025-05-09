using System.Security.Cryptography;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Authentication;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;

namespace AudioEngineersPlatformBackend.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IEmailService _emailService;
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(IEmailService emailService, IAuthenticationRepository authenticationRepository,
        IUnitOfWork unitOfWork)
    {
        _emailService = emailService;
        _authenticationRepository = authenticationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisterResponse> Register(RegisterRequest registerRequest,
        CancellationToken cancellationToken)
    {
        await _emailService.Send();

        return new RegisterResponse(Guid.Empty, "", "");

        /*// Check database invariants - find if email or phone number is already used
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

        // Create a User
        var user = new User(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email,
            registerRequest.PhoneNumber, registerRequest.Password, role!.IdRole, userLog.IdUserLog);

        // Add UserLog and User
        await _authenticationRepository.AddUserLog(userLog, cancellationToken);
        await _authenticationRepository.AddUser(user, cancellationToken);

        // Send a verification email


        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new RegisterResponse(user.IdUser, user.Email, "");*/
    }
}