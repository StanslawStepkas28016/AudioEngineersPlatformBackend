using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetInteractedUsers;

public class GetInteractedUsersQueryValidator : AbstractValidator<GetInteractedUsersQuery>
{
    public GetInteractedUsersQueryValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");
    }
}