namespace API.Contracts.Auth.Commands.Register;

public class RegisterResponse
{
    public required  Guid IdUser { get; init; }
    public required string Email { get; init; } 
}