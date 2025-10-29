using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ForgotPassword;
using AutoMapper;

namespace API.Contracts.Auth.Commands.ForgotPassword;

public class ForgotPasswordProfile : Profile
{
    public ForgotPasswordProfile()
    {
        CreateMap<ForgotPasswordRequest, ForgotPasswordCommand>();
        
        CreateMap<ForgotPasswordCommandResult, ForgotPasswordResponse>();
    }
}