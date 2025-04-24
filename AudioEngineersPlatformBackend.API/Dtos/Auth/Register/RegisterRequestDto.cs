namespace AudioEngineersPlatformBackend.Dtos.Auth.Register;

public class RegisterRequestDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string RoleName { get; set; }
    public string? AdminSecret { get; set; }
}