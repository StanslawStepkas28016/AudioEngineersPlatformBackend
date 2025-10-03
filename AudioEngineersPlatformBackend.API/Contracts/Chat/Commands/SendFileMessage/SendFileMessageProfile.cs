using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendFileMessage;
using AutoMapper;

namespace API.Contracts.Chat.Commands.SendFileMessage;

public class SendFileMessageProfile : Profile
{
    public SendFileMessageProfile()
    {
        CreateMap<SendFileMessageRequest, SendFileMessageCommand>();

        CreateMap<SendFileMessageCommandResult, SendFileMessageResponse>();
    }
}