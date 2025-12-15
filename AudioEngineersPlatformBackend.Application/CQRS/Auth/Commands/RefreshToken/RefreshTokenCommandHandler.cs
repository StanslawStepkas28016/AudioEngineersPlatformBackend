using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResult>
{
    private readonly ILogger<RefreshTokenCommandHandler> _logger;
    private readonly IValidator<RefreshTokenCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
        ILogger<RefreshTokenCommandHandler> logger,
        IValidator<RefreshTokenCommand> inputValidator,
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

    public async Task<RefreshTokenCommandResult> Handle(
        RefreshTokenCommand refreshTokenCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (refreshTokenCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(RefreshTokenCommandHandler),
                nameof(Handle),
                refreshTokenCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Find a user by the refresh token.
        User? user = await _authRepository.FindUserAndUserLogAndTokenByTokenAsync
            (refreshTokenCommand.RefreshToken.ToString(), cancellationToken);

        if (user == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a refresh token {RefreshToken} which does not point to any existing user.",
                nameof(RefreshTokenCommandHandler),
                nameof(Handle),
                refreshTokenCommand.RefreshToken
            );

            throw new BusinessRelatedException("User does not exist.");
        }

        // Check if the refresh token is expired - this should never happen, because the token is stored in the cookie
        // and the cookies are being deleted after their expiration date from the browser.
        Token extractedRefreshToken = user.Tokens.First();
        if (extractedRefreshToken
                .ExpirationDate < DateTime.UtcNow)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a correct refresh token {RefreshToken} but it is expired.",
                nameof(RefreshTokenCommandHandler),
                nameof(Handle),
                refreshTokenCommand.RefreshToken
            );

            throw new UnauthorizedAccessException($"{nameof(TokenNames.RefreshToken)} expired, please login again.");
        }

        // Generate a new token.
        Guid newRefreshToken = Guid.NewGuid();

        extractedRefreshToken
            .UpdateTokenData(newRefreshToken.ToString(), Token.RefreshTokenExpirationDate);

        // Map to result.
        RefreshTokenCommandResult result = _mapper.Map<User, RefreshTokenCommandResult>(user);

        // Persist the changes in the database.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return result;
    }
}