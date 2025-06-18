namespace AudioEngineersPlatformBackend.Contracts.Advert;

public record GetAdvertResponse(
    Guid IdAdvert,
    string Title,
    string Description,
    decimal Price,
    string CategoryName,
    string CoverImageUrl,
    string PlaylistUrl,
    string UserFirstName,
    string UserLastName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);