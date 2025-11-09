using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordCommandResult>
{
    private readonly ILogger<ResetPasswordCommandHandler> _logger;
    private readonly IValidator<ResetPasswordCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUrlGeneratorUtil _urlGeneratorUtil;
    private readonly ISesService _sesService;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordCommandHandler(
        ILogger<ResetPasswordCommandHandler> logger,
        IValidator<ResetPasswordCommand> inputValidator,
        IMapper mapper,
        IAuthRepository authRepository,
        IPasswordHasher<User> passwordHasher,
        IUrlGeneratorUtil urlGeneratorUtil,
        ISesService sesService,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _urlGeneratorUtil = urlGeneratorUtil;
        _sesService = sesService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResetPasswordCommandResult> Handle(
        ResetPasswordCommand resetPasswordCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (resetPasswordCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(ResetPasswordCommandHandler),
                nameof(Handle),
                resetPasswordCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Get the user data.
        User? userAndUserLog = await _authRepository.FindUserAndUserLogByIdUserAsync
            (resetPasswordCommand.IdUser, cancellationToken);

        if (userAndUserLog == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser from the access token {IdUser}, which does not point towards any user.",
                nameof(ResetPasswordCommandHandler),
                nameof(Handle),
                resetPasswordCommand.IdUser
            );

            throw new BusinessRelatedException("User not found.");
        }

        // Ensure the current password is correct.
        PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword
            (userAndUserLog, userAndUserLog.Password, resetPasswordCommand.CurrentPassword);

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a current password which is incorrect.",
                nameof(ResetPasswordCommandHandler),
                nameof(Handle)
            );

            throw new BusinessRelatedException("Invalid current password.");
        }

        // Ensure that new password and the repeated password match.
        if (resetPasswordCommand.NewPassword != resetPasswordCommand.NewPasswordRepeated)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided non-matching new passwords.",
                nameof(ResetPasswordCommandHandler),
                nameof(Handle)
            );

            throw new BusinessRelatedException("New password must match.");
        }

        // Create a token.
        Token resetPasswordToken = Token.Create
        (
            nameof(TokenNames.ResetPasswordToken),
            Guid
                .NewGuid()
                .ToString(),
            Token.ResetPasswordTokenExpirationDate,
            userAndUserLog.IdUser
        );

        await _authRepository.AddTokenAsync(resetPasswordToken, cancellationToken);

        // Set associated data.
        userAndUserLog.SetHashedPassword
            (_passwordHasher.HashPassword(userAndUserLog, resetPasswordCommand.NewPassword));
        userAndUserLog.UserAuthLog.SetIsResettingPassword(true);

        // Send an email with a password rest confirmation link.
        string resetPasswordUrl = await _urlGeneratorUtil.GenerateResetVerificationUrl
            (resetPasswordToken.Value, "verify-reset-password");

        await _sesService.SendPasswordResetEmailAsync(userAndUserLog.Email, userAndUserLog.FirstName, resetPasswordUrl);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ResetPasswordCommandResult { Instructions = "Instructions were sent to your email." };
    }
}