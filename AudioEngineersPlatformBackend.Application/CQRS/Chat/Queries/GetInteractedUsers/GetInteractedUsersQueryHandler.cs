using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetInteractedUsers;

public class GetInteractedUsersQueryHandler : IRequestHandler<GetInteractedUsersQuery, GetInteractedUsersQueryResult>
{
    private readonly ILogger<GetInteractedUsersQueryHandler> _logger;
    private readonly IValidator<GetInteractedUsersQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;

    public GetInteractedUsersQueryHandler(
        ILogger<GetInteractedUsersQueryHandler> logger,
        IValidator<GetInteractedUsersQuery> inputValidator,
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

    public async Task<GetInteractedUsersQueryResult> Handle(
        GetInteractedUsersQuery getInteractedUsersQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (getInteractedUsersQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetInteractedUsersQueryHandler),
                nameof(Handle),
                getInteractedUsersQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // See if the user exists.
        if (!await _userRepository.DoesUserExistByIdUserAsync(getInteractedUsersQuery.IdUser, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} which does not point towards any user.",
                nameof(GetInteractedUsersQueryHandler),
                nameof(Handle),
                getInteractedUsersQuery.IdUser
            );

            throw new BusinessRelatedException("User not found.");
        }

        // Fetch the data.
        List<InteractedUserDto> interactedUsersList = await _chatRepository.FindInteractedUsersAsync
            (getInteractedUsersQuery.IdUser, cancellationToken);

        // Map to result.
        GetInteractedUsersQueryResult result = _mapper.Map<List<InteractedUserDto>, GetInteractedUsersQueryResult>
            (interactedUsersList);

        return result;
    }
}