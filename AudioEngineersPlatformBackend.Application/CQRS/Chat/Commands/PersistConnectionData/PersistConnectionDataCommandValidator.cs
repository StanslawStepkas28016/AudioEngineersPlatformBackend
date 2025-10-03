using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.PersistConnectionData;

public class PersistConnectionDataCommandValidator : AbstractValidator<PersistConnectionDataCommand>
{
    public PersistConnectionDataCommandValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");

        RuleFor(exp => exp.ConnectionId)
            .NotEmpty()
            .WithMessage("ConnectionId must be provided.");
    }
}