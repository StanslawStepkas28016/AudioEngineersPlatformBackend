using API.Hubs;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Chat.GetChat;
using AudioEngineersPlatformBackend.Contracts.Chat.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Chat.GetPresignedUrlForUpload;
using AudioEngineersPlatformBackend.Contracts.Chat.GetUserData;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendFileMessage;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendGeneralMessage;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendTextMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _chatHubContext;

    public ChatController(IChatService chatService, IHubContext<ChatHub> chatHubContext)
    {
        _chatService = chatService;
        _chatHubContext = chatHubContext;
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPost("{idUserSender:guid}/text")]
    public async Task<IActionResult> SaveAndSendTextMessage(Guid idUserSender,
        [FromBody] TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken)
    {
        // Persist the message data.
        MessageResponse messageResponse = await _chatService.SaveTextMessage
            (idUserSender, textMessageRequest, cancellationToken);

        // Send the message using signalR if the user is online. 
        string idUserRecipient = textMessageRequest.IdUserRecipient.ToString();

        // Send a message to the recipients group (meaning send a message to all its available connections).
        await _chatHubContext.Clients.Group(idUserRecipient).SendAsync
        (
            "ReceiveMessageFromSender",
            messageResponse,
            cancellationToken: cancellationToken
        );

        return StatusCode(StatusCodes.Status200OK, messageResponse);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("presigned-upload")]
    public async Task<IActionResult> GetPresignedUrlForUpload(string folder, string fileName,
        CancellationToken cancellationToken)
    {
        GetPresignedUrlForUploadResponse getPresignedUrlForUploadResponse = await _chatService.GetPresignedUrlForUpload
            (folder, fileName, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, getPresignedUrlForUploadResponse);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPost("{idUserSender:guid}/file")]
    public async Task<IActionResult> SaveAndSendFileMessage(Guid idUserSender,
        [FromBody] FileMessageRequest fileMessageRequest,
        CancellationToken cancellationToken)
    {
        // Persist the message data.
        MessageResponse fileMessagesResponse = await _chatService.SaveFileMessage
            (idUserSender, fileMessageRequest, cancellationToken);

        // Send the message using signalR if the user is online. 
        string idUserRecipient = fileMessageRequest.IdUserRecipient.ToString();

        // Send a message to the recipients group (meaning send a message to all its available connections).
        await _chatHubContext.Clients.Group(idUserRecipient).SendAsync
        (
            "ReceiveMessageFromSender",
            fileMessagesResponse,
            cancellationToken: cancellationToken
        );

        return StatusCode(StatusCodes.Status200OK, fileMessagesResponse);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("{idUser:guid}/status")]
    public async Task<IActionResult> GetUserOnlineStatus(Guid idUser, CancellationToken cancellationToken)
    {
        bool isOnline = await _chatService.GetUserOnlineStatus(idUser, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, isOnline);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("{idUserSender:guid}/{idUserRecipient:guid}")]
    public async Task<IActionResult> GetChat(Guid idUserSender, Guid idUserRecipient,
        CancellationToken cancellationToken)
    {
        List<GetChatResponse> messages = await _chatService.GetChat
            (idUserSender, idUserRecipient, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, messages);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("{idUser:guid}/interacted")]
    public async Task<IActionResult> GetInteractedUsers(Guid idUser, CancellationToken cancellationToken)
    {
        List<InteractedUsersResponse> messagedUsers = await _chatService.GetInteractedUsers
            (idUser, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, messagedUsers);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("{idUser:guid}/user-data")]
    public async Task<IActionResult> GetUserData(Guid idUser, CancellationToken cancellationToken)
    {
        GetUserDataResponse userData = await _chatService.GetUserData(idUser, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, userData);
    }
}