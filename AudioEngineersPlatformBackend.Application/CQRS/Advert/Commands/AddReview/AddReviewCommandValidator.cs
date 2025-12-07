using AudioEngineersPlatformBackend.Domain.Entities;
using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.AddReview;

public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(exp => exp.IdUserReviewer)
            .NotEmpty()
            .WithMessage("IdUserReviewer must be provided.");

        RuleFor(exp => exp.IdAdvert)
            .NotEmpty()
            .WithMessage("IdAdvert must be provided.");

        RuleFor(exp => exp.Content)
            .NotEmpty()
            .WithMessage("Content must be provided.");

        RuleFor(exp => exp.SatisfactionLevel)
            .InclusiveBetween(Review.MinSatisfactionLevel, Review.MaxSatisfactionLevel)
            .WithMessage
                ($"SatisfactionLevel must be between {Review.MinSatisfactionLevel} and {Review.MaxSatisfactionLevel}.");
    }
}