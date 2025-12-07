namespace API.Contracts.Auth.Commands.Register;

public class RegisterRequest
{
    public required  string FirstName { get; init; } 
    public required string LastName { get; init; } 
    public required string Email { get; init; } 
    public required string PhoneNumber { get; init; } 
    public required string Password { get; init; } 
    public required string RoleName { get; init; } 
}