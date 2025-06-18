namespace AudioEngineersPlatformBackend.Contracts.Auth.Register;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string RoleName
);