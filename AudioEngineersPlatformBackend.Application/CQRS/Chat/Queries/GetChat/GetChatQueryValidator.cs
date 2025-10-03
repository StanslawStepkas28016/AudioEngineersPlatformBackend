using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetChat;

public class GetChatQueryValidator : AbstractValidator<GetChatQuery>
{
    public GetChatQueryValidator()
    {
        RuleFor(exp => exp.IdUserSender)
            .NotEmpty()
            .WithMessage("IdUserSender must be provided.");

        RuleFor(exp => exp.IdUserRecipient)
            .NotEmpty()
            .WithMessage("IdUserRecipient must be provided.");

        RuleFor(exp => exp.Page)
            .Must(val => val >= 1)
            .WithMessage("Page must be at least 1.");

        RuleFor(exp => exp.PageSize)
            .Must(val => val >= 1)
            .WithMessage("PageSize must be at least 1.");
    }
}