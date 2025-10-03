using AudioEngineersPlatformBackend.Application.CQRS.Auth.Queries.CheckAuth;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;

namespace API.Contracts.Auth.Queries.CheckAuth;

public class CheckAuthProfile : Profile
{
    public CheckAuthProfile()
    {
        CreateMap<Guid, CheckAuthQuery>()
            .ForMember(dest => dest.IdUser, exp => exp.MapFrom(src => src));

        CreateMap<CheckAuthDto, CheckAuthQueryResult>();

        CreateMap<CheckAuthQueryResult, CheckAuthResponse>();
    }
}