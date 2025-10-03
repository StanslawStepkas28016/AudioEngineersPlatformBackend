using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetEmail;

public class VerifyResetEmailCommandHandler : IRequestHandler<VerifyResetEmailCommand, VerifyResetEmailCommandResult>
{
    private readonly ILogger<VerifyResetEmailCommandHandler> _logger;
    private readonly IValidator<VerifyResetEmailCommand> _inputValidator;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyResetEmailCommandHandler(
        ILogger<VerifyResetEmailCommandHandler> logger,
        IValidator<VerifyResetEmailCommand> inputValidator,
        IMapper mapper,
        IAuthRepository authRepository,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VerifyResetEmailCommandResult> Handle(
        VerifyResetEmailCommand verifyResetEmailCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (verifyResetEmailCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(VerifyResetEmailCommandHandler),
                nameof(Handle),
                verifyResetEmailCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Find the user by token.
        var userAndUserLogWithToken =
            await _authRepository.FindUserAndUserLogAndTokenByTokenAsync
                (verifyResetEmailCommand.ResetEmailToken.ToString(), cancellationToken);

        if (userAndUserLogWithToken == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a ResetEmailToken {Token} which does not exist.",
                nameof(VerifyResetEmailCommandHandler),
                nameof(Handle),
                verifyResetEmailCommand.ResetEmailToken
            );

            throw new BusinessRelatedException("User not found.");
        }

        // Try to verify the token.
        Token token = userAndUserLogWithToken
            .Tokens.First();

        if (!token
                .TryVerify())
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a ResetEmailToken {Token} which exists but is no longer valid.",
                nameof(VerifyResetEmailCommandHandler),
                nameof(Handle),
                verifyResetEmailCommand.ResetEmailToken
            );

            throw new BusinessRelatedException("Token expired.");
        }

        // Remove associated data and the no-longer valid token.
        await _authRepository.DeleteTokenByValueAsync
            (verifyResetEmailCommand.ResetEmailToken.ToString(), cancellationToken);

        userAndUserLogWithToken.UserAuthLog.SetIsResettingEmail(false);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new VerifyResetEmailCommandResult { Message = "Email reset successful." };
    }
}