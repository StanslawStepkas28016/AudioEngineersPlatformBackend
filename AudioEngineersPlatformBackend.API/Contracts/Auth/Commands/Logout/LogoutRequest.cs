namespace API.Contracts.Auth.Commands.Logout;

public class LogoutRequest
{
    public required Guid IdUser { get; set; }
    public required string RefreshToken { get; set; } 
}