using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordCommandResult>
{
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;
    private readonly IValidator<ForgotPasswordCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IUrlGeneratorUtil _urlGeneratorUtil;
    private readonly ISesService _sesService;
    private readonly IUnitOfWork _unitOfWork;

    public ForgotPasswordCommandHandler(
        ILogger<ForgotPasswordCommandHandler> logger,
        IValidator<ForgotPasswordCommand> inputValidator,
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
        _authRepository = authRepository;
        _urlGeneratorUtil = urlGeneratorUtil;
        _sesService = sesService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ForgotPasswordCommandResult> Handle(
        ForgotPasswordCommand forgotPasswordCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (forgotPasswordCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(ForgotPasswordCommandHandler),
                nameof(Handle),
                forgotPasswordCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Find the user and its auth log by email.
        User? userAndUserLogByEmail = await _authRepository.FindUserAndUserLogAndRoleByEmailAsync
            (forgotPasswordCommand.Email, cancellationToken);

        if (userAndUserLogByEmail == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided and email address {EmailAddress} which des not point to any user.",
                nameof(ForgotPasswordCommandHandler),
                nameof(Handle),
                forgotPasswordCommand.Email
            );

            throw new BusinessRelatedException("User does not exist.");
        }

        // Ensure the user has a correct status.
        userAndUserLogByEmail.UserAuthLog.EnsureCorrectUserStatus();

        // Set forgot password data and add a resetting token.
        userAndUserLogByEmail.UserAuthLog.SetIsRemindingPasswordStatus(true);

        string forgotPasswordToken = Guid
            .NewGuid()
            .ToString();

        Token token = Token.Create
        (
            nameof(TokenNames.ForgotPasswordToken),
            forgotPasswordToken,
            Token.ForgotPasswordTokenExpirationDate,
            userAndUserLogByEmail.IdUser
        );

        await _authRepository.AddTokenAsync(token, cancellationToken);

        // Generate a verification url.
        string url = await _urlGeneratorUtil.GenerateResetVerificationUrl
            (forgotPasswordToken, "verify-forgot-password");

        // Email the user.
        await _sesService.SendForgotPasswordResetEmailAsync
            (userAndUserLogByEmail.Email, userAndUserLogByEmail.FirstName, url);

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ForgotPasswordCommandResult
            { Instructions = "Instructions for resetting your password were sent to your email address." };
    }
}