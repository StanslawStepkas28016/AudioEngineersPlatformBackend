using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.RefreshToken;
using AudioEngineersPlatformBackend.Domain.Entities;
using AutoMapper;

namespace API.Contracts.Auth.Commands.RefreshToken;

public class RefreshTokenProfile : Profile
{
    public RefreshTokenProfile()
    {
        CreateMap<User, RefreshTokenCommandResult>()
            .ForMember(dest => dest.User, exp => exp.MapFrom(src => src));

        CreateMap<Guid, RefreshTokenCommand>()
            .ForMember(dest => dest.RefreshToken, exp => exp.MapFrom(src => src));
    }
}