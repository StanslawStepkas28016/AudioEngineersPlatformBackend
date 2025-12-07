using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.CreateAdvert;
using AutoMapper;

namespace API.Contracts.Advert.Commands.CreateAdvert;

public class CreateAdvertProfile : Profile
{
    public CreateAdvertProfile()
    {
        CreateMap<CreateAdvertRequest, CreateAdvertCommand>();

        CreateMap<CreateAdvertCommandResult, CreateAdvertResponse>();
    }
}