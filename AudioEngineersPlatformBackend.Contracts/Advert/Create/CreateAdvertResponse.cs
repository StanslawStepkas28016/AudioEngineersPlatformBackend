namespace AudioEngineersPlatformBackend.Contracts.Advert.Create;

public record CreateAdvertResponse(
    Guid IdAdvert,
    Guid IdUser
);