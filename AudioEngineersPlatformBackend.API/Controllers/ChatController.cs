using API.Abstractions;
using API.Contracts.Chat.Commands.GetPreSignedUrlForUpload;
using API.Contracts.Chat.Commands.SendFileMessage;
using API.Contracts.Chat.Commands.SendTextMessage;
using API.Contracts.Chat.Queries.GetChat;
using API.Contracts.Chat.Queries.GetInteractedUsers;
using API.Contracts.Chat.Queries.GetUserData;
using API.Contracts.Chat.Queries.GetUserOnlineStatus;
using API.Hubs;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendFileMessage;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendTextMessage;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetChat;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetInteractedUsers;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetPresignedUrlForUpload;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserData;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserOnlineStatus;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;
    private readonly IClaimsUtil _claimsUtil;
    private readonly IHubContext<ChatHub> _chatHubContext;

    public ChatController(
        IMapper mapper,
        ISender sender,
        IClaimsUtil claimsUtil,
        IHubContext<ChatHub> chatHubContext
    )
    {
        _mapper = mapper;
        _sender = sender;
        _claimsUtil = claimsUtil;
        _chatHubContext = chatHubContext;
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpPost("text-message")]
    public async Task<IActionResult> SendTextMessage(
        [FromBody] SendTextMessageRequest sendTextMessageRequest,
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser with roleName and perform authorization checks.
        Guid idUserFromClaims = await _claimsUtil.ExtractIdUserFromClaims();
        string roleNameFromClaims = await _claimsUtil.ExtractRoleNameFromClaims();

        if (idUserFromClaims != sendTextMessageRequest.IdUserSender && roleNameFromClaims != "Administrator")
        {
            throw new UnauthorizedAccessException("You cannot create this resource.");
        }

        // Map to command.
        SendTextMessageCommand command = _mapper.Map<SendTextMessageRequest, SendTextMessageCommand>
            (sendTextMessageRequest);

        // Send to mediator.
        SendTextMessageCommandResult result = await _sender.Send(command, cancellationToken);

        // Map to response.
        SendTextMessageResponse response = _mapper.Map<SendTextMessageCommandResult, SendTextMessageResponse>(result);

        // Send the message using signalR if the user is online. 
        string idUserRecipient = sendTextMessageRequest.IdUserRecipient.ToString();

        // Send a message to the recipients group (meaning send a message to all users current connections).
        await _chatHubContext
            .Clients.Group(idUserRecipient)
            .SendAsync
            (
                "ReceiveMessageFromSender",
                response,
                cancellationToken
            );

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpGet("presigned-url-for-file-upload")]
    public async Task<IActionResult> GetPresignedUrlForUpload(
        [FromQuery] GetPreSignedUrlForUploadRequest getPreSignedUrlForUploadRequest,
        CancellationToken cancellationToken
    )
    {
        // Map to query.
        GetPresignedUrlForUploadQuery query =
            _mapper.Map<GetPreSignedUrlForUploadRequest, GetPresignedUrlForUploadQuery>
                (getPreSignedUrlForUploadRequest);

        // Send to mediator.
        GetPresignedUrlForUploadQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to response.
        GetPresignedUrlForUploadResponse response =
            _mapper.Map<GetPresignedUrlForUploadQueryResult, GetPresignedUrlForUploadResponse>(result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpPost("file-message")]
    public async Task<IActionResult> SendFileMessage(
        [FromBody] SendFileMessageRequest sendFileMessageRequest,
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser with roleName and perform authorization checks.
        Guid idUserFromClaims = await _claimsUtil.ExtractIdUserFromClaims();
        string roleNameFromClaims = await _claimsUtil.ExtractRoleNameFromClaims();

        if (idUserFromClaims != sendFileMessageRequest.IdUserSender && roleNameFromClaims != "Administrator")
        {
            throw new UnauthorizedAccessException("You cannot create this resource.");
        }

        // Map to command.
        SendFileMessageCommand command = _mapper.Map<SendFileMessageRequest, SendFileMessageCommand>
            (sendFileMessageRequest);

        // Send to mediator.
        SendFileMessageCommandResult result = await _sender.Send(command, cancellationToken);

        // Map to response.
        SendFileMessageResponse response = _mapper.Map<SendFileMessageCommandResult, SendFileMessageResponse>(result);

        // Send the message using signalR if the user is online.
        string idUserRecipient = sendFileMessageRequest.IdUserRecipient.ToString();

        // Send a message to the recipients group (meaning send a message to all its available connections).
        await _chatHubContext
            .Clients.Group(idUserRecipient)
            .SendAsync
            (
                "ReceiveMessageFromSender",
                response,
                cancellationToken
            );

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpGet("{idUser:guid}/status")]
    public async Task<IActionResult> GetUserOnlineStatus(
        [FromRoute] Guid idUser,
        CancellationToken cancellationToken
    )
    {
        // Map to query.
        GetUserOnlineStatusQuery query = _mapper.Map<Guid, GetUserOnlineStatusQuery>
            (idUser);

        // Send to mediator.
        GetUserOnlineStatusQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to response.
        GetUserOnlineStatusResponse response = _mapper.Map<GetUserOnlineStatusQueryResult, GetUserOnlineStatusResponse>
            (result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpGet("{idUserSender:guid}/{idUserRecipient:guid}")]
    public async Task<IActionResult> GetChat(
        [FromRoute] Guid idUserSender,
        [FromRoute] Guid idUserRecipient,
        [FromQuery] GetChatRequest getChatRequest,
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser with roleName and perform authorization checks.
        Guid idUserFromClaims = await _claimsUtil.ExtractIdUserFromClaims();
        string roleNameFromClaims = await _claimsUtil.ExtractRoleNameFromClaims();

        if (idUserFromClaims != idUserSender && roleNameFromClaims != "Administrator")
        {
            throw new UnauthorizedAccessException("You cannot access this resource.");
        }

        GetChatQuery query = _mapper.Map<GetChatRequest, GetChatQuery>
        (
            getChatRequest,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) =>
                {
                    dest.IdUserSender = idUserSender;
                    dest.IdUserRecipient = idUserRecipient;
                }
            )
        );

        // Send to mediator.
        GetChatQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to response.
        GetChatResponse response = _mapper.Map<GetChatQueryResult, GetChatResponse>(result);

        return Ok(response.PagedChatMessages);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpGet("{idUser:guid}/user-data")]
    public async Task<IActionResult> GetUserData(
        [FromRoute] Guid idUser,
        CancellationToken cancellationToken
    )
    {
        // Map to query.
        GetUserDataQuery query = _mapper.Map<Guid, GetUserDataQuery>(idUser);

        // Send to mediator.
        GetUserDataQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to response.
        GetUserDataResponse response = _mapper.Map<GetUserDataQueryResult, GetUserDataResponse>(result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client, Audio engineer")]
    [HttpGet("{idUser:guid}/interacted")]
    public async Task<IActionResult> GetInteractedUsers(
        [FromRoute] Guid idUser,
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser with roleName and perform authorization checks.
        Guid idUserFromClaims = await _claimsUtil.ExtractIdUserFromClaims();
        string roleNameFromClaims = await _claimsUtil.ExtractRoleNameFromClaims();

        if (idUserFromClaims != idUser && roleNameFromClaims != "Administrator")
        {
            throw new UnauthorizedAccessException("You cannot access this resource.");
        }

        // Map to query.
        GetInteractedUsersQuery query = _mapper.Map<Guid, GetInteractedUsersQuery>(idUser);

        // Send to mediator.
        GetInteractedUsersQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to result.
        GetInteractedUsersResponse response = _mapper.Map<GetInteractedUsersQueryResult, GetInteractedUsersResponse>
            (result);

        return Ok(response);
    }
}