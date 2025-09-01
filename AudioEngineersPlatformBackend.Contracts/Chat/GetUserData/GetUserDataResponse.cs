namespace AudioEngineersPlatformBackend.Contracts.Chat.GetUserData;

public sealed class GetUserDataResponse
{
    public Guid IdUser { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}