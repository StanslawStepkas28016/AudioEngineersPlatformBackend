using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyAccount;

public class VerifyAccountCommandHandler : IRequestHandler<VerifyAccountCommand, VerifyAccountCommandResult>
{
    private readonly ILogger<VerifyAccountCommandHandler> _logger;
    private readonly IValidator<VerifyAccountCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyAccountCommandHandler(
        ILogger<VerifyAccountCommandHandler> logger,
        IValidator<VerifyAccountCommand> inputValidator,
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

    public async Task<VerifyAccountCommandResult> Handle(
        VerifyAccountCommand verifyAccountCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (verifyAccountCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(VerifyAccountCommandHandler),
                nameof(Handle),
                verifyAccountCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Check database invariants - find if user with given id exits, find if verification code is valid.
        string verifyAccountCode =
            new VerificationCodeVo(verifyAccountCommand.VerificationCode).VerificationCode;

        User? user =
            await _authRepository.FindUserAndUserLogAndTokenByTokenAsync
            (
                verifyAccountCode,
                cancellationToken
            );

        if (user == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a non existent verification code {Code}.",
                nameof(VerifyAccountCommandHandler),
                nameof(Handle),
                verifyAccountCommand.VerificationCode
            );

            throw new BusinessRelatedException($"Provided {nameof(verifyAccountCommand.VerificationCode)} is invalid.");
        }

        // Try to verify the users account.
        Token token = user.Tokens.First();

        // If the verification fails, remove the user.
        if (!token.TryVerify())
        {
            // Mark the user as delete
            user.UserAuthLog.SetIsDeletedStatusAndData(true);

            // Persist the data.
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a correct verification code {Code}, but it is expired.",
                nameof(VerifyAccountCommandHandler),
                nameof(Handle),
                verifyAccountCommand.VerificationCode
            );

            throw new BusinessRelatedException("Your email verification code has expired. Please contact the support.");
        }

        // Mark the user as verified.
        user.UserAuthLog.SetIsVerifiedStatus(true);

        // Remove the no longer valid token.
        await _authRepository.DeleteTokenByValueAsync(verifyAccountCode, cancellationToken);

        // Persist the data.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new VerifyAccountCommandResult { Message = "Successfully verified your account." };
    }
}