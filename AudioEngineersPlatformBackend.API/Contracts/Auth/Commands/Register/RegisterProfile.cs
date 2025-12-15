using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Register;
using AudioEngineersPlatformBackend.Domain.Entities;
using AutoMapper;

namespace API.Contracts.Auth.Commands.Register;

public class RegisterProfile : Profile
{
    public RegisterProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        
        CreateMap<User, RegisterCommandResult>();
        
        CreateMap<RegisterCommandResult, RegisterResponse>();
    }
}