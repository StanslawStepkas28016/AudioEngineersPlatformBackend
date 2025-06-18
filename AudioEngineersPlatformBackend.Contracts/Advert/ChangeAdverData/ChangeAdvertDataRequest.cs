namespace AudioEngineersPlatformBackend.Contracts.Advert.ChangeAdverData;

public record ChangeAdvertDataRequest(
    string? Title,
    string? Description,
    string? PortfolioUrl,
    double Price
);