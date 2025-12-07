using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.DeleteAdvert;
using AutoMapper;

namespace API.Contracts.Advert.Commands.DeleteAdvert;

public class DeleteAdvertProfile : Profile
{
    public DeleteAdvertProfile()
    {
        CreateMap<DeleteAdvertRequest, DeleteAdvertCommand>()
            .ForMember(exp => exp.IdAdvert, opt => opt.Ignore());

        CreateMap<DeleteAdvertCommandResult, DeleteAdvertResponse>();
    }
}