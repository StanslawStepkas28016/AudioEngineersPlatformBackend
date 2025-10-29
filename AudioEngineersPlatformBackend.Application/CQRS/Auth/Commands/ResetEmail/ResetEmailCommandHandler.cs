using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetEmail;

public class ResetEmailCommandHandler : IRequestHandler<ResetEmailCommand, ResetEmailCommandResult>
{
    private readonly ILogger<ResetEmailCommandHandler> _logger;
    private readonly IValidator<ResetEmailCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IUrlGeneratorUtil _urlGeneratorUtil;
    private readonly ISesService _sesService;
    private readonly IUnitOfWork _unitOfWork;

    public ResetEmailCommandHandler(
        ILogger<ResetEmailCommandHandler> logger,
        IValidator<ResetEmailCommand> inputValidator,
        IMapper mapper,
        IAuthRepository authRepository,
        IUrlGeneratorUtil urlGeneratorUtil,
        ISesService sesService,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _urlGeneratorUtil = urlGeneratorUtil;
        _sesService = sesService;
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResetEmailCommandResult> Handle(
        ResetEmailCommand resetEmailCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (resetEmailCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(ResetEmailCommandHandler),
                nameof(Handle),
                resetEmailCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Validate if the user exists.
        User? userWithUserLog = await _authRepository.FindUserAndUserLogByIdUserAsync
            (resetEmailCommand.IdUser, cancellationToken);

        if (userWithUserLog == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} from an access token which does not exist.",
                nameof(ResetEmailCommandHandler),
                nameof(Handle),
                resetEmailCommand.IdUser
            );

            throw new BusinessRelatedException("User not found.");
        }

        // Ensure users status is correct.
        userWithUserLog.UserAuthLog.EnsureCorrectUserStatus();

        // Check if the email is equivalent to the current mail.
        if (resetEmailCommand.NewEmail == userWithUserLog.Email)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a new email address {Email} which is equivalent to their current email address.",
                nameof(ResetEmailCommandHandler),
                nameof(Handle),
                resetEmailCommand.NewEmail
            );

            throw new BusinessRelatedException("New email must differ from the old one.");
        }

        // Check if the new email is already in use.
        if (await _authRepository.IsEmailAlreadyTakenAsync(resetEmailCommand.NewEmail, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an email address {Email} which is already taken.",
                nameof(ResetEmailCommandHandler),
                nameof(Handle),
                resetEmailCommand.NewEmail
            );

            throw new BusinessRelatedException("Email already taken.");
        }

        // Generate a ResetEmailToken and generate a reset email url.
        string resetEmailTokenValue = Guid
            .NewGuid()
            .ToString();

        // Update the email itself and set email reset data (token, its expiration and user state).
        userWithUserLog.ResetEmail(resetEmailCommand.NewEmail);

        userWithUserLog.UserAuthLog.SetIsResettingEmail(true);

        Token token = Token.Create
        (
            nameof(TokenNames.ResetEmailToken),
            resetEmailTokenValue,
            Token.ResetEmailTokenExpirationDate,
            userWithUserLog.IdUser
        );

        await _authRepository.AddTokenAsync(token, cancellationToken);

        // Log the user out from all other sessions, by deleting all refresh tokens.
        // The current (request cookie) refresh token will be deleted from the API layer.
        await _authRepository.DeleteAllTokensWithSpecificNameByIdUserAsync
            (userWithUserLog.IdUser, nameof(TokenNames.RefreshToken), cancellationToken);

        // Send a new verification email containing a link to verify new email address.
        string resetEmailUrl = await _urlGeneratorUtil
            .GenerateResetVerificationUrl
                (resetEmailTokenValue, "verify-reset-email");

        await _sesService.SendEmailResetEmailAsync
            (resetEmailCommand.NewEmail, userWithUserLog.FirstName, resetEmailUrl);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ResetEmailCommandResult { Instructions = "Instructions were sent to your new email inbox." };
    }
}