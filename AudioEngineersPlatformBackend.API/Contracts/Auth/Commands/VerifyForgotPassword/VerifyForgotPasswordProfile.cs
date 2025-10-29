using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyForgotPassword;
using AutoMapper;

namespace API.Contracts.Auth.Commands.VerifyForgotPassword;

public class VerifyForgotPasswordProfile : Profile
{
    public VerifyForgotPasswordProfile()
    {
        CreateMap<VerifyForgotPasswordRequest, VerifyForgotPasswordCommand>()
            .ForMember(dest => dest.ForgotPasswordToken, src => src.Ignore());

        CreateMap<VerifyForgotPasswordCommandResult, VerifyForgotPasswordResponse>();
    }
}