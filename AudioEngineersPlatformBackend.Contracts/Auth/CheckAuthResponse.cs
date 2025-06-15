namespace AudioEngineersPlatformBackend.Contracts.Auth;

public record CheckAuthResponse(
    Guid IdUser,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    Guid IdRole,
    string RoleName
);