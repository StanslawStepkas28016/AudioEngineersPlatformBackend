using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetIdAdvertByIdUser;

public class GetIdAdvertByIdUserQueryValidator : AbstractValidator<GetIdAdvertByIdUserQuery>
{
    public GetIdAdvertByIdUserQueryValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");
    }
}