using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserData;

public class GetUserDataQueryValidator : AbstractValidator<GetUserDataQuery>
{
    public GetUserDataQueryValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");
    }
}