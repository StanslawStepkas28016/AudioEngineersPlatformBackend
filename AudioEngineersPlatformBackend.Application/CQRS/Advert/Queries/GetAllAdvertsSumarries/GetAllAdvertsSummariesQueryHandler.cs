using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAllAdvertsSumarries;

public class
    GetAllAdvertsSummariesQueryHandler : IRequestHandler<GetAllAdvertsSummariesQuery, GetAllAdvertsSummariesQueryResult>
{
    private readonly ILogger<GetAllAdvertsSummariesQueryHandler> _logger;
    private readonly IValidator<GetAllAdvertsSummariesQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IS3Service _s3Service;

    public GetAllAdvertsSummariesQueryHandler(
        ILogger<GetAllAdvertsSummariesQueryHandler> logger,
        IValidator<GetAllAdvertsSummariesQuery> inputValidator,
        IMapper mapper,
        IAdvertRepository advertRepository,
        IS3Service s3Service
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _advertRepository = advertRepository;
        _s3Service = s3Service;
    }

    public async Task<GetAllAdvertsSummariesQueryResult> Handle(
        GetAllAdvertsSummariesQuery getAllAdvertsSummariesQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (getAllAdvertsSummariesQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetAllAdvertsSummariesQueryHandler),
                nameof(Handle),
                getAllAdvertsSummariesQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Fetch all adverts summaries.
        PagedListDto<AdvertSummaryDto> pagedListDto =
            await _advertRepository.FindAdvertSummariesAsync
            (
                getAllAdvertsSummariesQuery.CategoryFilterTerm,
                getAllAdvertsSummariesQuery.SortOrder,
                getAllAdvertsSummariesQuery.SearchTerm,
                getAllAdvertsSummariesQuery.Page,
                getAllAdvertsSummariesQuery.PageSize,
                cancellationToken
            );

        // Generate presigned URLs for cover images.
        foreach (AdvertSummaryDto advert in pagedListDto.Items)
        {
            advert.CoverImageUrl =
                await _s3Service.GetPreSignedUrlForReadAsync
                    ("images", advert.CoverImageKey.ToString(), advert.CoverImageKey, cancellationToken);
        }

        // Map to result.
        GetAllAdvertsSummariesQueryResult result =
            _mapper.Map<PagedListDto<AdvertSummaryDto>, GetAllAdvertsSummariesQueryResult>(pagedListDto);

        return result;
    }
}