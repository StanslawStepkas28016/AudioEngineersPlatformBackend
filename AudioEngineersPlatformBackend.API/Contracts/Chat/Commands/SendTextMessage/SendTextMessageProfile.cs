using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendTextMessage;
using AutoMapper;

namespace API.Contracts.Chat.Commands.SendTextMessage;

public class SendTextMessageProfile : Profile
{
    public SendTextMessageProfile()
    {
        CreateMap<SendTextMessageRequest, SendTextMessageCommand>();

        CreateMap<SendTextMessageCommandResult, SendTextMessageResponse>();
    }
}