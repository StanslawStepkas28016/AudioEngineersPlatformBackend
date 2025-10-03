namespace API.Contracts.Auth.Commands.Login;

public class LoginResponse
{
    public required Guid IdUser { get; init; }
    public required string FirstName { get; init; } 
    public required string LastName { get; init; } 
    public required string Email { get; init; } 
    public required  string PhoneNumber { get; init; } 
    public required Guid IdRole { get; init; }
    public required string RoleName { get; init; } 
}