using AudioEngineersPlatformBackend.Domain.Entities;
using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendTextMessage;

public class SendTextMessageCommandValidator : AbstractValidator<SendTextMessageCommand>
{
    public SendTextMessageCommandValidator()
    {
        RuleFor(exp => exp.IdUserSender)
            .NotEmpty()
            .WithMessage("IdUserSender must be provided.");

        RuleFor(exp => exp.IdUserRecipient)
            .NotEmpty()
            .WithMessage("IdUserRecipient must be provided.");

        RuleFor(exp => exp.TextContent)
            .NotEmpty()
            .WithMessage("TextContent must be provided.");

        RuleFor(exp => exp.TextContent)
            .Length(1, Message.MaxLength)
            .WithMessage($"TextContent can be {Message.MaxLength} characters maximum.");
    }
}