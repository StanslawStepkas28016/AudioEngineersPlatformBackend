using API.Hubs;
using AudioEngineersPlatformBackend.Application.Abstractions;
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
    public async Task<IActionResult> SendAndSaveTextMessage([FromBody] TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken)
    {
        // Persist the message data.
        await _messageService.SaveTextMessage(textMessageRequest, cancellationToken);

        // Send the message using signalR. 
        string idUserRecipient = textMessageRequest.IdUserRecipient.ToString();

        await _chatHubContext.Clients.User(ChatHub.ConnectedUsers[idUserRecipient].First())
            .SendAsync
            (
                "ReceiveMessageFromSender", textMessageRequest.TextContent, cancellationToken: cancellationToken
            );

        return StatusCode(StatusCodes.Status204NoContent);
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
    
    // TODO: Add saving file messages, Fetching conversations list (not conversations).
}