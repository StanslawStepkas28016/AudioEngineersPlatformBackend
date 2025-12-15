namespace API.Contracts.Auth.Commands.ResetEmail;

public class ResetEmailRequest
{
    public required string NewEmail { get; init; } 
}