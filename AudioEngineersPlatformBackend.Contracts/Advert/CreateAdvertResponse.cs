namespace AudioEngineersPlatformBackend.Contracts.Advert;

public record CreateAdvertResponse(
    Guid IdAdvert,
    Guid IdUser
);