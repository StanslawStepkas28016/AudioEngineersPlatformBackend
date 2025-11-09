using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetIdAdvertByIdUser;

public class GetIdAdvertByIdUserQueryHandler : IRequestHandler<GetIdAdvertByIdUserQuery, GetIdAdvertByIdUserQueryResult>
{
    private readonly ILogger<GetIdAdvertByIdUserQueryHandler> _logger;
    private readonly IValidator<GetIdAdvertByIdUserQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;

    public GetIdAdvertByIdUserQueryHandler(
        ILogger<GetIdAdvertByIdUserQueryHandler> logger,
        IValidator<GetIdAdvertByIdUserQuery> inputValidator,
        IMapper mapper,
        IAdvertRepository advertRepository
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _advertRepository = advertRepository;
    }

    public async Task<GetIdAdvertByIdUserQueryResult> Handle(
        GetIdAdvertByIdUserQuery getIdAdvertByIdUserQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (getIdAdvertByIdUserQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetIdAdvertByIdUserQuery),
                nameof(Handle),
                getIdAdvertByIdUserQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Fetch data.
        Guid idAdvert = await _advertRepository.FindIdAdvertByIdUser
            (getIdAdvertByIdUserQuery.IdUser, cancellationToken);

        if (idAdvert == Guid.Empty)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} which does not have any advert assigned..",
                nameof(GetIdAdvertByIdUserQuery),
                nameof(Handle),
                getIdAdvertByIdUserQuery.IdUser
            );

            throw new BusinessRelatedException("You have not posted an advert yet.");
        }

        // Map to result.
        GetIdAdvertByIdUserQueryResult result = _mapper.Map<Guid, GetIdAdvertByIdUserQueryResult>(idAdvert);

        return result;
    }
}