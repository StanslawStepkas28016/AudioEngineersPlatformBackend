namespace AudioEngineersPlatformBackend.Application.Dtos;

public record ReviewDto(
    Guid IdReview,
    string ClientFirstName,
    string ClientLastName,
    string Content,
    byte SatisfactionLevel,
    DateTime DateCreated
);