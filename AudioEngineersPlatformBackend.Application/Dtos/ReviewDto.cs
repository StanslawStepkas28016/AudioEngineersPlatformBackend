namespace AudioEngineersPlatformBackend.Application.Dtos;

public class ReviewDto
{
    public required Guid IdReview { get; init; }
    public required string ClientFirstName { get; init; }
    public required string ClientLastName { get; init; }
    public required string Content { get; init; }
    public required byte SatisfactionLevel { get; init; }
    public required DateTime DateCreated { get; init; }
}