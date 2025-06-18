using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Contracts.Advert.CreateAdvert;

public record CreateAdvertRequest(
    Guid IdUser,
    string Title,
    string Description,
    IFormFile CoverImageFile,
    string PortfolioUrl,
    double Price,
    string CategoryName
);