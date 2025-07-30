namespace AudioEngineersPlatformBackend.Contracts.User.ChangeData;

public record ChangeDataRequest(
    string PhoneNumber,
    string Email
);