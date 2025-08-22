namespace AudioEngineersPlatformBackend.Contracts.Message.GetMessagedUsers;

public sealed class InteractedUsersResponse
{
    public Guid IdUser { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
};