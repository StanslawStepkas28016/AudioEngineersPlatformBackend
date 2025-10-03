using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(exp => exp.Email)
            .EmailAddress()
            .WithMessage("Valid email address must be provided.");

        RuleFor(exp => exp.FirstName)
            .NotEmpty()
            .WithMessage("Valid first name must be provided.");

        RuleFor(exp => exp.LastName)
            .NotEmpty()
            .WithMessage("Valid last name must be provided.");

        RuleFor(exp => exp.PhoneNumber)
            .NotEmpty()
            .WithMessage("Valid phone number must be provided.");

        RuleFor(exp => exp.Password)
            .NotEmpty()
            .WithMessage("Valid password must be provided.");

        RuleFor(exp => exp.RoleName)
            .NotEmpty()
            .WithMessage("Valid role name must be provided.");
    }
}