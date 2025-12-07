namespace API.Contracts.Auth.Commands.ResetPassword;

public class ResetPasswordRequest
{
    public required string CurrentPassword { get; init; } 
    public required string NewPassword { get; init; } 
    public required string NewPasswordRepeated { get; init; } 
}