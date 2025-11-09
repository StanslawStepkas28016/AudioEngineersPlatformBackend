using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertDetails;

public class GetAdvertDetailsQueryHandler : IRequestHandler<GetAdvertDetailsQuery, GetAdvertDetailsQueryResult>
{
    private readonly ILogger<GetAdvertDetailsQueryHandler> _logger;
    private readonly IValidator<GetAdvertDetailsQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IS3Service _s3Service;

    public GetAdvertDetailsQueryHandler(
        ILogger<GetAdvertDetailsQueryHandler> logger,
        IValidator<GetAdvertDetailsQuery> inputValidator,
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

    public async Task<GetAdvertDetailsQueryResult> Handle(
        GetAdvertDetailsQuery getAdvertDetailsQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (getAdvertDetailsQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetAdvertDetailsQueryHandler),
                nameof(Handle),
                getAdvertDetailsQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Check if the advert exists.
        if (!await _advertRepository.DoesAdvertExistByIdAdvertAsync(getAdvertDetailsQuery.IdAdvert, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdAdvert {IdAdvert} which does not point to any existing advert.",
                nameof(GetAdvertDetailsQueryHandler),
                nameof(Handle),
                getAdvertDetailsQuery.IdAdvert
            );

            throw new BusinessRelatedException("Advert does not exist.");
        }

        // Fetch the advert data.
        AdvertDetailsDto? advertDetailsDto =
            await _advertRepository.FindAdvertDetailsByIdAdvertAsync(getAdvertDetailsQuery.IdAdvert, cancellationToken);
        
        // Generate a presigned URL for the cover image.
        advertDetailsDto!.CoverImageUrl = await _s3Service.GetPreSignedUrlForReadAsync
            ("images", "", advertDetailsDto!.CoverImageKey, cancellationToken);

        // Map to result.
        GetAdvertDetailsQueryResult result = _mapper.Map<AdvertDetailsDto, GetAdvertDetailsQueryResult>
            (advertDetailsDto);

        return result;
    }
}