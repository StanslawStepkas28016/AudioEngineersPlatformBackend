using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Login;
using AudioEngineersPlatformBackend.Domain.Entities;
using AutoMapper;

namespace API.Contracts.Auth.Commands.Login;

public class LoginProfile : Profile
{
    public LoginProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();

        CreateMap<User, LoginCommandResult>()
            .ForMember
            (
                dest => dest.User,
                exp => exp.MapFrom(src => src)
            );

        // Mapping explicitly, as I don't want to follow a naming convention.
        // This could be omitted by specifying: IdUser => UserIdUser etc. 
        CreateMap<LoginCommandResult, LoginResponse>()
            .ForMember(dest => dest.IdUser, exp => exp.MapFrom(src => src.User.IdUser))
            .ForMember(dest => dest.FirstName, exp => exp.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, exp => exp.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, exp => exp.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PhoneNumber, exp => exp.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.IdRole, exp => exp.MapFrom(src => src.User.Role.IdRole))
            .ForMember(dest => dest.RoleName, exp => exp.MapFrom(src => src.User.Role.RoleName));
    }
}