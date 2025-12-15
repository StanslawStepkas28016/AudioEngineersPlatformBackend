using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResult>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IValidator<RegisterCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ISesService _sesService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger,
        IValidator<RegisterCommand> inputValidator,
        IMapper mapper,
        IAuthRepository authRepository,
        IPasswordHasher<User> passwordHasher,
        ISesService sesService,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _sesService = sesService;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisterCommandResult> Handle(
        RegisterCommand registerCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (registerCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(RegisterCommandHandler),
                nameof(Handle),
                registerCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Check if the provided email or phone number is already used.
        if (await _authRepository.FindUserByEmailAsNoTrackingAsync
            (
                new EmailVo(registerCommand.Email).Email,
                cancellationToken
            ) != null
           )
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an email address {Email} which is already taken.",
                nameof(RegisterCommandHandler),
                nameof(Handle),
                registerCommand.Email
            );

            throw new BusinessRelatedException
            (
                $"Provided {nameof(registerCommand.Email)
                    .ToLower()} is already taken."
            );
        }

        string validPhoneNumber = new PhoneNumberVo(registerCommand.PhoneNumber).PhoneNumber;

        if (await _authRepository.IsPhoneNumberAlreadyTakenAsync(validPhoneNumber, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided phone number {PhoneNumber} which is already taken.",
                nameof(RegisterCommandHandler),
                nameof(Handle),
                registerCommand.PhoneNumber
            );

            throw new BusinessRelatedException
            (
                $"Provided {nameof(registerCommand.PhoneNumber)
                    .ToLower()} is already taken."
            );
        }

        // Create a UserAuthLog, by default the users IsVerified flag is set to false.
        UserAuthLog userAuthLog = UserAuthLog.Create();

        // Check if the provided role exists.
        Role? role = await _authRepository.FindRoleByNameAsNoTrackingAsync(registerCommand.RoleName, cancellationToken);

        if (role == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided incorrect role name {RoleName}.",
                nameof(RegisterCommandHandler),
                nameof(Handle),
                registerCommand.RoleName
            );

            throw new ArgumentException($"Invalid {nameof(registerCommand.RoleName)}.");
        }

        // Create a User, then hash their password.
        User user = User.Create
        (
            registerCommand.FirstName,
            registerCommand.LastName,
            registerCommand.Email,
            registerCommand.PhoneNumber,
            registerCommand.Password,
            role.IdRole,
            userAuthLog.IdUserAuthLog
        );

        // Hash and set the password.
        user.SetHashedPassword(_passwordHasher.HashPassword(user, registerCommand.Password));

        // Create a token for email verification.
        Token token = Token.Create
        (
            nameof(TokenNames.VerifyAccountToken),
            VerificationCodeVo.Generate(),
            Token.VerifyAccountTokenExpirationDate,
            user.IdUser
        );

        // Add UserAuthLog and User and Token.
        await _authRepository.AddUserLogAsync(userAuthLog, cancellationToken);
        await _authRepository.AddUserAsync(user, cancellationToken);
        await _authRepository.AddTokenAsync(token, cancellationToken);

        // Send a verification email
        await _sesService.SendRegisterVerificationEmailAsync(user.Email, user.FirstName, token.Value);

        // Map to result.
        RegisterCommandResult registerCommandResult = _mapper.Map<User, RegisterCommandResult>(user);

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return registerCommandResult;
    }
}