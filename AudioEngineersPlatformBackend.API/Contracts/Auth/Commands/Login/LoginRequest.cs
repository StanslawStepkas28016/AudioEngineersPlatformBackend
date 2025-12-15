namespace API.Contracts.Auth.Commands.Login;

public class LoginRequest
{
    public required string Email { get; init; } 
    public required string Password { get; init; } 
}