using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResult>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<LoginCommand> _inputValidator;
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        ILogger<LoginCommandHandler> logger,
        IValidator<LoginCommand> inputValidator,
        IMapper mapper,
        IAuthRepository authRepository,
        IPasswordHasher<User> passwordHasher,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginCommandResult> Handle(
        LoginCommand loginCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync(loginCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(LoginCommandHandler),
                nameof(Handle),
                loginCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Check database invariants - find if user exists.
        User? user = await _authRepository.FindUserAndUserLogAndRoleByEmailAsync
        (
            new EmailVo(loginCommand.Email).Email,
            cancellationToken
        );

        if (user == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User with provided email address {EmailAddress} does not exist.",
                nameof(LoginCommandHandler),
                nameof(Handle), 
                loginCommand.Email
            );

            throw new ArgumentException
                ("Invalid credentials.");
        }

        // Ensure that the user is neither deleted nor unverified.
        user.UserAuthLog.EnsureCorrectUserStatus();

        // Set login associated data.
        user.UserAuthLog.SetLoginData();

        // Add a refresh token.
        Guid refreshToken = Guid.NewGuid();

        Token token = Token.Create
            (nameof(TokenNames.RefreshToken), refreshToken.ToString(), Token.RefreshTokenExpirationDate, user.IdUser);

        await _authRepository.AddTokenAsync(token, cancellationToken);

        // Verify the input password against the database.
        PasswordVerificationResult passwordVerificationResult =
            _passwordHasher.VerifyHashedPassword(user, user.Password, loginCommand.Password);

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User with provided Email {EmailAddress} provided wrong password.",
                nameof(LoginCommandHandler),
                nameof(Handle),
                loginCommand.Email
            );

            throw new ArgumentException
                ("Invalid credentials.");
        }

        // Map to result.
        LoginCommandResult result = _mapper.Map<User, LoginCommandResult>(user);

        // Persist the changes in the database.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return result;
    }
}