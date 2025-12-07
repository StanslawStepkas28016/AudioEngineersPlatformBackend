namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserData;

public class GetUserDataQueryResult
{
    public Guid IdUser { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}