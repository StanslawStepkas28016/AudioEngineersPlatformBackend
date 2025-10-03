namespace API.Contracts.Chat.Queries.GetUserData;

public class GetUserDataResponse
{
    public Guid IdUser { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}