using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.PersistConnectionData;
using AutoMapper;

namespace API.Contracts.Chat.Commands.PersistConnectionData;

public class PersistConnectionDataProfile : Profile
{
    public PersistConnectionDataProfile()
    {
        CreateMap<PersistConnectionDataRequest, PersistConnectionDataCommand>();
        
        CreateMap<PersistConnectionDataCommandResult, PersistConnectionDataResponse>();
    }
}