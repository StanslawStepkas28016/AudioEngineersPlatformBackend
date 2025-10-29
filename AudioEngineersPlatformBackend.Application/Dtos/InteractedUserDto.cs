namespace AudioEngineersPlatformBackend.Application.Dtos;

public class InteractedUserDto
{
    public required Guid IdUser { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int UnreadCount { get; set; }

    private bool Equals(
        InteractedUserDto other
    )
    {
        return IdUser == other.IdUser
               && FirstName == other.FirstName
               && LastName == other.LastName;
    }

    public override bool Equals(
        object? obj
    )
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((InteractedUserDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdUser, FirstName, LastName);
    }
}