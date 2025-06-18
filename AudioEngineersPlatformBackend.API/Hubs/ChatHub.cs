using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Chat.PersistSessionData;
using AudioEngineersPlatformBackend.Contracts.Chat.ReceiveMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace API.Hubs;

[Authorize(Roles = "Admin, Client, Audio engineer")]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        // Add current client to its own connection group.
        await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier!);

        // Save connection data to the DB.
        await _chatService.PersistConnectionData
        (
            Guid.Parse(Context.UserIdentifier!),
            new PersistConnectionDataRequest
            {
                ConnectionId = Context.ConnectionId,
                IsConnecting = true
            },
            CancellationToken.None
        );
        
        Log.Information($"Connected as {Context.UserIdentifier} with {Context.ConnectionId}");
        

        // Send information to other online client that the user has connected.
        await Clients.Others.SendAsync
        (
            "ReceiveIsOnlineMessage",
            new ReceiveIsOnlineMessage { IdUser = Guid.Parse(Context.UserIdentifier!), IsOnline = true }
        );
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Not removing the user from its group on disconnected as the MS Docs suggest
        // https://learn.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections#groups

        // Save connection data to the DB.
        await _chatService.PersistConnectionData
        (
            Guid.Parse(Context.UserIdentifier!),
            new PersistConnectionDataRequest
            {
                ConnectionId = Context.ConnectionId,
                IsConnecting = false
            },
            CancellationToken.None
        );

        Log.Information($"Disconnected as {Context.UserIdentifier} with {Context.ConnectionId}");

        // Send information to other online client that the user has disconnected.
        await Clients.Others.SendAsync
        (
            "ReceiveIsOnlineMessage",
            new ReceiveIsOnlineMessage { IdUser = Guid.Parse(Context.UserIdentifier!), IsOnline = false }
        );
    }
}