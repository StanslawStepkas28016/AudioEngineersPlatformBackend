using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertReviews;

public class GetAdvertReviewsQueryValidator : AbstractValidator<GetAdvertReviewsQuery>
{
    public GetAdvertReviewsQueryValidator()
    {
        RuleFor(exp => exp.IdAdvert)
            .NotEmpty()
            .WithMessage("IdAdvert must be provided.");

        RuleFor(exp => exp.Page)
            .Must(exp => exp >= 1)
            .WithMessage("Page must be at least 1.");

        RuleFor(exp => exp.PageSize)
            .Must(exp => exp >= 1)
            .WithMessage("Page size must be at least 1.");
    }
}