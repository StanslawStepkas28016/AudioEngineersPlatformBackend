using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetPassword;

public class
    VerifyResetPasswordCommandHandler : IRequestHandler<VerifyResetPasswordCommand, VerifyResetPasswordCommandResult>
{
    private readonly ILogger<VerifyResetPasswordCommandHandler> _logger;
    private readonly IValidator<VerifyResetPasswordCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyResetPasswordCommandHandler(
        ILogger<VerifyResetPasswordCommandHandler> logger,
        IValidator<VerifyResetPasswordCommand> inputValidator,
        IMapper mapper,
        IAuthRepository authRepository,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VerifyResetPasswordCommandResult> Handle(
        VerifyResetPasswordCommand verifyResetPasswordCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (verifyResetPasswordCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(VerifyResetPasswordCommandHandler),
                nameof(Handle),
                verifyResetPasswordCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // See if user exits by token.
        User? userAndUserLogWithToken = await _authRepository
            .FindUserAndUserLogAndTokenByTokenAsync
                (verifyResetPasswordCommand.ResetPasswordToken.ToString(), cancellationToken);

        if (userAndUserLogWithToken == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a ResetPasswordToken {Token} which does not point to any user.",
                nameof(VerifyResetPasswordCommandHandler),
                nameof(Handle),
                verifyResetPasswordCommand.ResetPasswordToken
            );

            throw new BusinessRelatedException("User does not exist.");
        }

        // Try to verify the token.
        Token token = userAndUserLogWithToken.Tokens.First();

        if (!token.TryVerify())
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a ResetPasswordToken {Token} which exists but is no longer valid.",
                nameof(VerifyResetPasswordCommandHandler),
                nameof(Handle),
                verifyResetPasswordCommand.ResetPasswordToken
            );

            throw new BusinessRelatedException("Token expired.");
        }

        // Remove associated data and the no-longer valid token.
        await _authRepository.DeleteTokenByValueAsync
        (
            userAndUserLogWithToken.Tokens.First()
                .Value,
            cancellationToken
        );

        userAndUserLogWithToken.UserAuthLog.SetIsResettingPassword(false);

        // Persist all data.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new VerifyResetPasswordCommandResult { Message = "Password reset successful." };
    }
}