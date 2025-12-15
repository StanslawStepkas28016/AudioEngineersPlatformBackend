using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendFileMessage;

public class SendFileMessageCommandValidator : AbstractValidator<SendFileMessageCommand>
{
    public SendFileMessageCommandValidator()
    {
        RuleFor(exp => exp.IdUserSender)
            .NotEmpty()
            .WithMessage("IdUserSender must be provided.");

        RuleFor(exp => exp.IdUserRecipient)
            .NotEmpty()
            .WithMessage("IdUserRecipient must be provided.");

        RuleFor(exp => exp.FileKey)
            .NotEmpty()
            .WithMessage("FileKey must be provided.");

        RuleFor(exp => exp.FileName)
            .NotEmpty()
            .WithMessage("FileName must be provided.");
    }
}