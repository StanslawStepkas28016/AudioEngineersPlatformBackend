using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize(Roles = "Admin, Client, Audio engineer")]
public class ChatHub : Hub
{
    // Methods on your hub are for clients to call.
    // Think of them the same way you do subscribe calls on the client.
    // Those are your hooks for the server, just like the methods on your hub are your hooks for the client.

    public static Dictionary<string, List<string>> ConnectedUsers = new();
    
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var idUser = Context.UserIdentifier;

        lock (ConnectedUsers)
        {
            if (!ConnectedUsers.ContainsKey(idUser))
                ConnectedUsers[idUser] = new();
            ConnectedUsers[idUser].Add(connectionId);
        }
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        var idUser = Context.UserIdentifier;

        lock (ConnectedUsers)
        {
            if (ConnectedUsers.ContainsKey(idUser))
            {
                ConnectedUsers[idUser].Remove(connectionId);
                if (ConnectedUsers[idUser].Count == 0)
                    ConnectedUsers.Remove(idUser);
            }
        }
    }
}