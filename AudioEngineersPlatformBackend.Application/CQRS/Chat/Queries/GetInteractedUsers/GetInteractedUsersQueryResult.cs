using AudioEngineersPlatformBackend.Application.Dtos;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetInteractedUsers;

public class GetInteractedUsersQueryResult
{
    public required List<InteractedUserDto> InteractedUsersList { get; set; }
}