namespace API.Contracts.Auth.Commands.VerifyForgotPassword;

public class VerifyForgotPasswordRequest
{
    public required  string NewPassword { get; set; } 
    public required string NewPasswordRepeated { get; set; } 
}