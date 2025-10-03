using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyForgotPassword;

public class
    VerifyForgotPasswordCommandHandler : IRequestHandler<VerifyForgotPasswordCommand, VerifyForgotPasswordCommandResult>
{
    private readonly ILogger<VerifyForgotPasswordCommandHandler> _logger;
    private readonly IValidator<VerifyForgotPasswordCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyForgotPasswordCommandHandler(
        ILogger<VerifyForgotPasswordCommandHandler> logger,
        IValidator<VerifyForgotPasswordCommand> inputValidator,
        IMapper mapper,
        IAuthRepository authRepository,
        IPasswordHasher<User> passwordHasher,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _mapper = mapper;
        _inputValidator = inputValidator;
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<VerifyForgotPasswordCommandResult> Handle(
        VerifyForgotPasswordCommand verifyForgotPasswordCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (verifyForgotPasswordCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(VerifyForgotPasswordCommandHandler),
                nameof(Handle),
                verifyForgotPasswordCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Find the user based on the token.
        User? userAndUserLog = await _authRepository.FindUserAndUserLogAndTokenByTokenAsync
            (verifyForgotPasswordCommand.ForgotPasswordToken.ToString(), cancellationToken);

        if (userAndUserLog == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a token {Token} which does not point towards any user.",
                nameof(VerifyForgotPasswordCommandHandler),
                nameof(Handle),
                verifyForgotPasswordCommand.ForgotPasswordToken
            );

            throw new BusinessRelatedException("User not found.");
        }

        // See if the persisted password is identical to the newly provided password.
        if (_passwordHasher.VerifyHashedPassword
                (userAndUserLog, userAndUserLog.Password, verifyForgotPasswordCommand.NewPassword) ==
            PasswordVerificationResult.Success)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a new password which is identical to the stored password.",
                nameof(VerifyForgotPasswordCommandHandler),
                nameof(Handle)
            );
            throw new BusinessRelatedException("New password must be differ from the old password.");
        }

        // Hash the new password.
        string newHashedPassword = new PasswordHasher<User>().HashPassword
            (userAndUserLog, verifyForgotPasswordCommand.NewPassword);

        userAndUserLog.ResetHashedPassword(newHashedPassword);

        // Set all related data.
        userAndUserLog.UserAuthLog.SetIsRemindingPasswordStatus(false);

        // Delete the old token.
        await _authRepository.DeleteTokenByValueAsync
        (
            userAndUserLog.Tokens.First()
                .Value,
            cancellationToken
        );

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new VerifyForgotPasswordCommandResult { Message = "Password reset completed." };
    }
}