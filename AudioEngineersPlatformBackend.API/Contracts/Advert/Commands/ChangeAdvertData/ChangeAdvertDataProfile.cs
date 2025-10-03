using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.ChangeAdvertData;
using AutoMapper;

namespace API.Contracts.Advert.Commands.ChangeAdvertData;

public class ChangeAdvertDataProfile : Profile
{
    public ChangeAdvertDataProfile()
    {
        CreateMap<ChangeAdvertDataRequest, ChangeAdvertDataCommand>()
            .ForMember(exp => exp.IdAdvert, opt => opt.Ignore());

        CreateMap<ChangeAdvertDataCommandResult, ChangeAdvertDataResponse>();
    }
}