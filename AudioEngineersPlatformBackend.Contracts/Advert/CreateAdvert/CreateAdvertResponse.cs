namespace AudioEngineersPlatformBackend.Contracts.Advert.CreateAdvert;

public record CreateAdvertResponse(
    Guid IdAdvert,
    Guid IdUser
);