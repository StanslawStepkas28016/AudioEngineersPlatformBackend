namespace AudioEngineersPlatformBackend.Contracts.Advert;

public record GetAdvertResponse(
    Guid IdUser,
    Guid IdAdvert,
    string Title,
    string Description,
    double Price,
    string CategoryName,
    string CoverImageUrl,
    string PortfolioUrl,
    string UserFirstName,
    string UserLastName,
    DateTime DateCreated,
    DateTime? DateModified
);