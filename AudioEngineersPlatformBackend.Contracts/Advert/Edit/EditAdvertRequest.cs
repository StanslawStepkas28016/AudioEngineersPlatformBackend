namespace AudioEngineersPlatformBackend.Contracts.Advert.Edit;

public record EditAdvertRequest(
    string? Title,
    string? Description,
    string? PortfolioUrl,
    double Price
);