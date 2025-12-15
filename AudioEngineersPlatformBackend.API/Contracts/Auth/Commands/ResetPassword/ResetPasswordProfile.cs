using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetPassword;
using AutoMapper;

namespace API.Contracts.Auth.Commands.ResetPassword;

public class ResetPasswordProfile : Profile
{
    public ResetPasswordProfile()
    {
        CreateMap<ResetPasswordRequest, ResetPasswordCommand>()
            .ForMember(dest => dest.IdUser, exp => exp.Ignore());

        CreateMap<ResetPasswordCommandResult, ResetPasswordResponse>();
    }
}