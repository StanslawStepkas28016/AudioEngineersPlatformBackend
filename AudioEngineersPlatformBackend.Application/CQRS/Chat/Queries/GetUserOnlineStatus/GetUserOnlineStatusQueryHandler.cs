using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserOnlineStatus;

public class GetUserOnlineStatusQueryHandler : IRequestHandler<GetUserOnlineStatusQuery, GetUserOnlineStatusQueryResult>
{
    private readonly ILogger<GetUserOnlineStatusQueryHandler> _logger;
    private readonly IValidator<GetUserOnlineStatusQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;

    public GetUserOnlineStatusQueryHandler(
        ILogger<GetUserOnlineStatusQueryHandler> logger,
        IValidator<GetUserOnlineStatusQuery> inputValidator,
        IMapper mapper,
        IUserRepository userRepository,
        IChatRepository chatRepository
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }

    public async Task<GetUserOnlineStatusQueryResult> Handle(
        GetUserOnlineStatusQuery getUserOnlineStatusQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (getUserOnlineStatusQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetUserOnlineStatusQueryHandler),
                nameof(Handle),
                getUserOnlineStatusQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // See if the user exists.
        if (!await _userRepository.DoesUserExistByIdUserAsync(getUserOnlineStatusQuery.IdUser, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} which does not point towards any user.",
                nameof(GetUserOnlineStatusQueryHandler),
                nameof(Handle),
                getUserOnlineStatusQuery.IdUser
            );

            throw new BusinessRelatedException("User not found.");
        }

        // Find if the user is online.
        bool isOnline = await _chatRepository
            .IsUserOnlineAsync(getUserOnlineStatusQuery.IdUser, cancellationToken);

        // Map to result.
        GetUserOnlineStatusQueryResult result = _mapper.Map<bool, GetUserOnlineStatusQueryResult>(isOnline);

        return result;
    }
}