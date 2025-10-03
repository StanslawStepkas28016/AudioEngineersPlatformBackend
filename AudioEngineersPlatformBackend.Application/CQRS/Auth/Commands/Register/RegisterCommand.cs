using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Register;

public class RegisterCommand : IRequest<RegisterCommandResult>
{
    public string FirstName { get; init; } 
    public string LastName { get; init; } 
    public string Email { get; init; } 
    public string PhoneNumber { get; init; } 
    public string Password { get; init; } 
    public string RoleName { get; init; } 
}