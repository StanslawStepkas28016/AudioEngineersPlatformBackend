using API.Contracts.Chat.Commands.PersistConnectionData;
using API.Dtos;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.PersistConnectionData;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize(Roles = "Administrator, Client, Audio engineer")]
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    private const int CancellationTokenDelay = 1500;

    public ChatHub(
        ILogger<ChatHub> logger,
        IMapper mapper,
        ISender sender
    )
    {
        _logger = logger;
        _mapper = mapper;
        _sender = sender;
    }

    private async Task<PersistConnectionDataResponse> PersistConnectionDataToDatabaseAsync(
        PersistConnectionDataRequest persistConnectionDataRequest,
        CancellationToken cancellationToken
    )
    {
        // Map to command.
        PersistConnectionDataCommand command = _mapper.Map<PersistConnectionDataRequest, PersistConnectionDataCommand>
            (persistConnectionDataRequest);

        // Send to mediator.
        PersistConnectionDataCommandResult result = await _sender.Send(command, cancellationToken);

        // Map to response.
        PersistConnectionDataResponse response =
            _mapper.Map<PersistConnectionDataCommandResult, PersistConnectionDataResponse>(result);

        return response;
    }

    public override async Task OnConnectedAsync()
    {
        // Add current client to its own connection group.
        await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier!);

        // Persist connection data to the DB.
        await PersistConnectionDataToDatabaseAsync
        (
            new PersistConnectionDataRequest
            {
                IdUser = Guid.Parse(Context.UserIdentifier!), ConnectionId = Context.ConnectionId, IsConnecting = true
            },
            new CancellationTokenSource(CancellationTokenDelay).Token
        );

        // Log information.
        _logger.LogInformation
        (
            "Info from Class {ClassName}, Method {MethodName}: Connected as {ContextUserIdentifier} with {ContextConnectionId}.",
            nameof(ChatHub),
            nameof(OnConnectedAsync),
            Context.UserIdentifier,
            Context.ConnectionId
        );

        // Send information to other online client that the user has connected.
        await Clients.Others.SendAsync
        (
            "ReceiveIsOnlineMessage",
            new IsOnlineMessageDto { IdUser = Guid.Parse(Context.UserIdentifier!), IsOnline = true }
        );
    }

    public override async Task OnDisconnectedAsync(
        Exception? exception
    )
    {
        // Not removing the user from its group on disconnected as the MS Docs suggest.
        // Source: https://learn.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections#groups

        // Persist connection data to the DB.
        await PersistConnectionDataToDatabaseAsync
        (
            new PersistConnectionDataRequest
            {
                IdUser = Guid.Parse(Context.UserIdentifier!), ConnectionId = Context.ConnectionId, IsConnecting = false
            },
            new CancellationTokenSource(CancellationTokenDelay).Token
        );

        // Log information.
        _logger.LogInformation
        (
            "Info from Class {ClassName}, Method {MethodName}: Disconnected as {ContextUserIdentifier} with {ContextConnectionId}.",
            nameof(ChatHub),
            nameof(OnDisconnectedAsync),
            Context.UserIdentifier,
            Context.ConnectionId
        );

        // Send information to other online client that the user has disconnected.
        await Clients.Others.SendAsync
        (
            "ReceiveIsOnlineMessage",
            new IsOnlineMessageDto { IdUser = Guid.Parse(Context.UserIdentifier!), IsOnline = false }
        );
    }
}