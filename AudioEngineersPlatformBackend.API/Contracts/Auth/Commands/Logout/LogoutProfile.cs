using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Logout;
using AutoMapper;

namespace API.Contracts.Auth.Commands.Logout;

public class LogoutProfile : Profile
{
    public LogoutProfile()
    {
        CreateMap<LogoutRequest, LogoutCommand>();
        
        CreateMap<LogoutCommandResult, LogoutResponse>();
    }
}