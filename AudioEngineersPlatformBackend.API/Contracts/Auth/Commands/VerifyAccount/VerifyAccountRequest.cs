namespace API.Contracts.Auth.Commands.VerifyAccount;

public class VerifyAccountRequest
{
    public required string VerificationCode { get; init; } 
}