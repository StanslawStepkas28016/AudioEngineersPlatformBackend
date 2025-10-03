using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAllAdvertsSumarries;

public class GetAllAdvertsSummariesQueryValidator : AbstractValidator<GetAllAdvertsSummariesQuery>
{
    public GetAllAdvertsSummariesQueryValidator()
    {
        RuleFor(exp => exp.Page)
            .Must(exp => exp >= 1)
            .WithMessage("Page must be at least 1.");

        RuleFor(exp => exp.PageSize)
            .Must(exp => exp >= 1)
            .WithMessage("Page size must be at least 1.");
    }
}