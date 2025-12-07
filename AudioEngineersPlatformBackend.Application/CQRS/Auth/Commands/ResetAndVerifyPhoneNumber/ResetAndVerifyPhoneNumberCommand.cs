using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetAndVerifyPhoneNumber;

public class ResetAndVerifyPhoneNumberCommand : IRequest<ResetAndVerifyPhoneNumberCommandResult>
{
    public required Guid IdUser { get; set; }
    public required string NewPhoneNumber { get; set; } 
}