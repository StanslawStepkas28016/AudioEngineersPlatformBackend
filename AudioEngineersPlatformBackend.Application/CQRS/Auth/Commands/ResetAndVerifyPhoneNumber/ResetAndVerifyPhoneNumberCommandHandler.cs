using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetAndVerifyPhoneNumber;

public class
    ResetAndVerifyPhoneNumberCommandHandler : IRequestHandler<ResetAndVerifyPhoneNumberCommand,
    ResetAndVerifyPhoneNumberCommandResult>
{
    private readonly ILogger<ResetAndVerifyPhoneNumberCommandHandler> _logger;
    private readonly IValidator<ResetAndVerifyPhoneNumberCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResetAndVerifyPhoneNumberCommandHandler(
        ILogger<ResetAndVerifyPhoneNumberCommandHandler> logger,
        IValidator<ResetAndVerifyPhoneNumberCommand> inputValidator,
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

    public async Task<ResetAndVerifyPhoneNumberCommandResult> Handle(
        ResetAndVerifyPhoneNumberCommand resetAndVerifyPhoneNumberCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (resetAndVerifyPhoneNumberCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(ResetAndVerifyPhoneNumberCommandHandler),
                nameof(Handle),
                resetAndVerifyPhoneNumberCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Find the user and its associated data.
        User? userAndUserLog = await _authRepository.FindUserAndUserLogByIdUserAsync
            (resetAndVerifyPhoneNumberCommand.IdUser, cancellationToken);

        if (userAndUserLog == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser from the access token {Id} which does not point towards any user.",
                nameof(ResetAndVerifyPhoneNumberCommandHandler),
                nameof(Handle),
                resetAndVerifyPhoneNumberCommand.IdUser
            );

            throw new BusinessRelatedException("User not found");
        }

        // Attempt resetting the number, will throw an exception if the number is the same.
        userAndUserLog.ResetPhoneNumber(new PhoneNumberVo(resetAndVerifyPhoneNumberCommand.NewPhoneNumber).PhoneNumber);

        // Persist all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ResetAndVerifyPhoneNumberCommandResult { Message = "Phone reset successful." };
    }
}