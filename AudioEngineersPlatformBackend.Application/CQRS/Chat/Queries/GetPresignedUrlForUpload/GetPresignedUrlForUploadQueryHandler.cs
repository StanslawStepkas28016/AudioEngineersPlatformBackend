using AudioEngineersPlatformBackend.Application.Abstractions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetPresignedUrlForUpload;

public class
    GetPresignedUrlForUploadQueryHandler : IRequestHandler<GetPresignedUrlForUploadQuery,
    GetPresignedUrlForUploadQueryResult>
{
    private readonly ILogger<GetPresignedUrlForUploadQueryHandler> _logger;
    private readonly IValidator<GetPresignedUrlForUploadQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IS3Service _s3Service;

    public GetPresignedUrlForUploadQueryHandler(
        ILogger<GetPresignedUrlForUploadQueryHandler> logger,
        IValidator<GetPresignedUrlForUploadQuery> inputValidator,
        IMapper mapper,
        IS3Service s3Service
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _s3Service = s3Service;
    }

    public async Task<GetPresignedUrlForUploadQueryResult> Handle(
        GetPresignedUrlForUploadQuery getPresignedUrlForUploadQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (getPresignedUrlForUploadQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetPresignedUrlForUploadQueryResult),
                nameof(Handle),
                getPresignedUrlForUploadQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Generate the url for upload.
        Guid fileKey = Guid.NewGuid();

        string preSignedUrl = await _s3Service.GetPreSignedUrlForUploadAsync
        (
            getPresignedUrlForUploadQuery.Folder,
            fileKey,
            getPresignedUrlForUploadQuery.FileName,
            cancellationToken
        );

        return new GetPresignedUrlForUploadQueryResult
        {
            FileKey = fileKey,
            PreSignedUrlForUpload = preSignedUrl
        };
    }
}