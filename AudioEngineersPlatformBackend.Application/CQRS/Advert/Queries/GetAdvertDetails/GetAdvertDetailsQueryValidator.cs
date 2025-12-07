using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertDetails;

public class GetAdvertDetailsQueryValidator : AbstractValidator<GetAdvertDetailsQuery>
{
    public GetAdvertDetailsQueryValidator()
    {
        RuleFor(exp => exp.IdAdvert)
            .NotEmpty()
            .WithMessage("IdAdvert must be provided.");
    }
}