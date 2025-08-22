using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize(Roles = "Admin, Client, Audio engineer")]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Add current client to its own connection group.
        // await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier!);
        
        // Send information to other online client that the user has connected.
        await Clients.Others.SendAsync
        (
            "ReceiveAvailabilityStatusMessage", "Online"
        );
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Remove current client to its own connection group.
        // await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.UserIdentifier!);

        // Send information to other online client that the user has disconnected.
        await Clients.Others.SendAsync
        (
            "ReceiveAvailabilityStatusMessage", "Offline"
        );
    }
}