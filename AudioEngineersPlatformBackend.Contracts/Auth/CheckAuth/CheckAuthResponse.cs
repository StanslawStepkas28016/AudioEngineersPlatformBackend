namespace AudioEngineersPlatformBackend.Contracts.Auth.CheckAuth;

public record CheckAuthResponse
(
    Guid IdUser,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    Guid IdRole,
    string RoleName
);