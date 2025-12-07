using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserData;

public class GetUserDataQueryHandler : IRequestHandler<GetUserDataQuery, GetUserDataQueryResult>
{
    private readonly ILogger<GetUserDataQueryHandler> _logger;
    private readonly IValidator<GetUserDataQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IChatRepository _chatRepository;

    public GetUserDataQueryHandler(
        ILogger<GetUserDataQueryHandler> logger,
        IValidator<GetUserDataQuery> inputValidator,
        IMapper mapper,
        IChatRepository chatRepository
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _chatRepository = chatRepository;
    }

    public async Task<GetUserDataQueryResult> Handle(
        GetUserDataQuery getUserDataQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (getUserDataQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetUserDataQueryHandler),
                nameof(Handle),
                getUserDataQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Fetch the data.
        UserDataDto? userDataDto = await _chatRepository.FindUserDataAsync(getUserDataQuery.IdUser, cancellationToken);

        if (userDataDto == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} which does not point towards any user.",
                nameof(GetUserDataQueryHandler),
                nameof(Handle),
                getUserDataQuery.IdUser
            );

            throw new BusinessRelatedException("User not found.");
        }

        // Map to result.
        GetUserDataQueryResult result = _mapper.Map<UserDataDto, GetUserDataQueryResult>(userDataDto);

        return result;
    }
}