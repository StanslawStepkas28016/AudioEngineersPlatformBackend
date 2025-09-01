namespace AudioEngineersPlatformBackend.Contracts.Chat.GetMessagedUsers;

public sealed class InteractedUsersResponse
{
    public Guid IdUser { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int UnreadCount { get; set; }

    private bool Equals(InteractedUsersResponse? other)
    {
        return
            (IdUser == other?.IdUser) &&
            (FirstName == other?.FirstName) &&
            (LastName == other.LastName);
    }

    public override bool Equals(object? obj)
    {
        return Equals((InteractedUsersResponse)obj!);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdUser, FirstName, LastName);
    }
};