using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Queries.CheckAuth;

public class CheckAuthQueryHandler : IRequestHandler<CheckAuthQuery, CheckAuthQueryResult>
{
    private readonly ILogger<CheckAuthQueryHandler> _logger;
    private readonly IValidator<CheckAuthQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;

    public CheckAuthQueryHandler(
        ILogger<CheckAuthQueryHandler> logger,
        IValidator<CheckAuthQuery> inputValidator,
        IMapper mapper,
        IAuthRepository authRepository
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _authRepository = authRepository;
    }

    public async Task<CheckAuthQueryResult> Handle(
        CheckAuthQuery checkAuthQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (checkAuthQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(CheckAuthQueryHandler),
                nameof(Handle),
                checkAuthQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Get the user associated data.
        CheckAuthDto? userAssociatedData =
            await _authRepository.GetCheckAuthDataAsync(checkAuthQuery.IdUser, cancellationToken);

        // This should never happen, since the idUser is being pulled from the JWT token that resides in the request cookie.
        // Despite the following, it is still being checked and logged as this is a data which is fetched often.
        if (userAssociatedData == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} which does not point toward any user. This error is unexpected and should never happen.",
                nameof(CheckAuthQueryHandler),
                nameof(Handle),
                checkAuthQuery.IdUser
            );

            throw new Exception("User does not exist.");
        }

        // Map to result.
        CheckAuthQueryResult result = _mapper.Map<CheckAuthDto, CheckAuthQueryResult>(userAssociatedData);

        return result;
    }
}