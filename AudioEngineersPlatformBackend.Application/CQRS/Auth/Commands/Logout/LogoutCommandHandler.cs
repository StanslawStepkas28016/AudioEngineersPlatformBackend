using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutCommandResult>
{
    private readonly ILogger<LogoutCommandHandler> _logger;
    private readonly IValidator<LogoutCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(
        ILogger<LogoutCommandHandler> logger,
        IValidator<LogoutCommand> inputValidator,
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

    public async Task<LogoutCommandResult> Handle(
        LogoutCommand logoutCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync(logoutCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(LogoutCommandHandler),
                nameof(Handle),
                logoutCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Get the refreshToken from the database.
        User? userWithToken = await _authRepository.FindUserAndUserLogAndTokenByTokenAsync
            (logoutCommand.RefreshToken, cancellationToken);

        if (userWithToken == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a refresh token {Token} which is not present in the database.",
                nameof(LogoutCommandHandler),
                nameof(Handle),
                logoutCommand.RefreshToken
            );

            throw new BusinessRelatedException("User does not exist.");
        }

        // Delete the token.
        await _authRepository.DeleteTokenByValueAsync
        (
            userWithToken.Tokens.First()
                .Value,
            cancellationToken
        );
        
        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new LogoutCommandResult { Message = "Successfully logged out" };
    }
}