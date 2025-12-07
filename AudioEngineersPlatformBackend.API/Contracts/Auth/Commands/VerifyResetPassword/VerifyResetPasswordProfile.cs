using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetPassword;
using AutoMapper;

namespace API.Contracts.Auth.Commands.VerifyResetPassword;

public class VerifyResetPasswordProfile : Profile
{
    public VerifyResetPasswordProfile()
    {
        CreateMap<Guid, VerifyResetPasswordCommand>()
            .ForMember(dest => dest.ResetPasswordToken, exp => exp.MapFrom(src => src));

        CreateMap<VerifyResetPasswordCommandResult, VerifyResetPasswordResponse>();
    }
}