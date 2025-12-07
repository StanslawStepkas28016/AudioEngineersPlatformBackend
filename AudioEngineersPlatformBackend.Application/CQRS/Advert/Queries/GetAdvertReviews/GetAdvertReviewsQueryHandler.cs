using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertReviews;

public class GetAdvertReviewsQueryHandler : IRequestHandler<GetAdvertReviewsQuery, GetAdvertReviewsQueryResult>
{
    private readonly ILogger<GetAdvertReviewsQueryHandler> _logger;
    private readonly IValidator<GetAdvertReviewsQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;

    public GetAdvertReviewsQueryHandler(
        ILogger<GetAdvertReviewsQueryHandler> logger,
        IValidator<GetAdvertReviewsQuery> inputValidator,
        IMapper mapper,
        IAdvertRepository advertRepository
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _advertRepository = advertRepository;
    }

    public async Task<GetAdvertReviewsQueryResult> Handle(
        GetAdvertReviewsQuery getAdvertReviewsQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator
            .ValidateAsync(getAdvertReviewsQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetAdvertReviewsQueryResult),
                nameof(Handle),
                getAdvertReviewsQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Check if the advert exists.
        if (!await _advertRepository
                .DoesAdvertExistByIdAdvertAsync(getAdvertReviewsQuery.IdAdvert, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdAdvert {IdAdvert} which does not exist.",
                nameof(GetAdvertReviewsQuery),
                nameof(Handle),
                getAdvertReviewsQuery.IdAdvert
            );

            throw new BusinessRelatedException("Advert not found.");
        }

        // Fetch the data.
        PagedListDto<ReviewDto> reviewsForAdvertPaginated =
            await _advertRepository.FindAdvertReviewsAsync
            (
                getAdvertReviewsQuery.IdAdvert,
                getAdvertReviewsQuery.Page,
                getAdvertReviewsQuery.PageSize,
                cancellationToken
            );

        // Map to result.
        GetAdvertReviewsQueryResult result = _mapper.Map<PagedListDto<ReviewDto>, GetAdvertReviewsQueryResult>
            (reviewsForAdvertPaginated);

        return result;
    }
}