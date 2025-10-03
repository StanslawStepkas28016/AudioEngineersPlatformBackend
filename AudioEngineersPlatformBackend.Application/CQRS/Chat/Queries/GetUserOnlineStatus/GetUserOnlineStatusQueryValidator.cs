using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserOnlineStatus;

public class GetUserOnlineStatusQueryValidator : AbstractValidator<GetUserOnlineStatusQuery>
{
    public GetUserOnlineStatusQueryValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");
    }
}