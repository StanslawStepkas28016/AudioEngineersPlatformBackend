using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetPresignedUrlForUpload;

public class GetPresignedUrlForUploadQueryValidator : AbstractValidator<GetPresignedUrlForUploadQuery>
{
    public GetPresignedUrlForUploadQueryValidator()
    {
        RuleFor(exp => exp.FileName)
            .NotEmpty()
            .WithMessage("FileName must be provided.");
        
        RuleFor(exp => exp.Folder)
            .NotEmpty()
            .WithMessage("Folder must be provided.");
    }
}