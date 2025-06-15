using Amazon.Util.Internal;

namespace AudioEngineersPlatformBackend.Contracts.Advert;

public record CreateAdvertRequest(
    string Title,
    string Description,
    IFile CoverImage,
    string PortfolioUrl,
    double Price,
    string Category
);