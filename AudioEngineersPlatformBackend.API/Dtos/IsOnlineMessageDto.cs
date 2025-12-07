namespace API.Dtos;

public class IsOnlineMessageDto
{
    public required Guid IdUser { get; set; }
    public required bool IsOnline { get; set; }
}