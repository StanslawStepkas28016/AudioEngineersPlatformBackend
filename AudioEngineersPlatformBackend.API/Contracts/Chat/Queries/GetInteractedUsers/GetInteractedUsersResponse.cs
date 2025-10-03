using AudioEngineersPlatformBackend.Application.Dtos;

namespace API.Contracts.Chat.Queries.GetInteractedUsers;

public class GetInteractedUsersResponse
{
    public required List<InteractedUserDto> InteractedUsersList { get; set; }
}