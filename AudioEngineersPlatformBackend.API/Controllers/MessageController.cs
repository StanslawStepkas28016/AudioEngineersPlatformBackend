using API.Hubs;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Message.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserData;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserMessages;
using AudioEngineersPlatformBackend.Contracts.Message.SaveAndSendTextMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("api/message")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IHubContext<ChatHub> _chatHubContext;

    public MessageController(IMessageService messageService, IHubContext<ChatHub> chatHubContext)
    {
        _messageService = messageService;
        _chatHubContext = chatHubContext;
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpPost]
    public async Task<IActionResult> SaveAndSendTextMessage([FromBody] TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken)
    {
        // Persist the message data.
        TextMessageResponse textMessageResponse = await _messageService.SaveTextMessage
            (textMessageRequest, cancellationToken);

        // Send the message using signalR if the user is online. 
        string idUserRecipient = textMessageRequest.IdUserRecipient.ToString();

        // Send a message to recipient.
        await _chatHubContext.Clients.User(idUserRecipient).SendAsync
        (
            "ReceiveMessageFromSender",
            textMessageResponse,
            cancellationToken: cancellationToken
        );
        
        return StatusCode(StatusCodes.Status200OK, textMessageResponse);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("{idUserSender:guid}/{idUserRecipient:guid}")]
    public async Task<IActionResult> GetUserMessages(Guid idUserSender, Guid idUserRecipient,
        CancellationToken cancellationToken)
    {
        List<GetMessageResponse> messages = await _messageService.GetUserMessages
            (idUserSender, idUserRecipient, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, messages);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("{idUser:guid}/interacted")]
    public async Task<IActionResult> GetInteractedUsers(Guid idUser, CancellationToken cancellationToken)
    {
        List<InteractedUsersResponse> messagedUsers = await _messageService.GetInteractedUsers
            (idUser, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, messagedUsers);
    }

    [Authorize(Roles = "Admin, Client, Audio engineer")]
    [HttpGet("{idUser:guid}/user-data")]
    public async Task<IActionResult> GetUserData(Guid idUser, CancellationToken cancellationToken)
    {
        GetUserDataResponse userData = await _messageService.GetUserData(idUser, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, userData);
    }


    // TODO: Add saving file messages.
}