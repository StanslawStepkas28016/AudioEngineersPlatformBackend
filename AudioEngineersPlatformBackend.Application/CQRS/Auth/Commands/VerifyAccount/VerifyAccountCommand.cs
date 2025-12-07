using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyAccount;

public class VerifyAccountCommand : IRequest<VerifyAccountCommandResult>
{
    public required string VerificationCode { get; init; } 
}