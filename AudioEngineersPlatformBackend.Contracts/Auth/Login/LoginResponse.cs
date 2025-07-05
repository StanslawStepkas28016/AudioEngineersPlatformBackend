namespace AudioEngineersPlatformBackend.Contracts.Auth;

public record LoginResponse(
    Guid IdUser,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string RoleName,
    Guid IdRole
);