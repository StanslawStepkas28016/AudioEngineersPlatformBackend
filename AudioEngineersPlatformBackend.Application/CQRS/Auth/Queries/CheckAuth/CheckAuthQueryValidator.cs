using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Queries.CheckAuth;

public class CheckAuthQueryValidator : AbstractValidator<CheckAuthQuery>
{
    public CheckAuthQueryValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");
    }
}