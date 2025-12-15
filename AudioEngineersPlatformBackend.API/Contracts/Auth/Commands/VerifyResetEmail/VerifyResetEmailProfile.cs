using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetEmail;
using AutoMapper;

namespace API.Contracts.Auth.Commands.VerifyResetEmail;

public class VerifyResetEmailProfile : Profile
{
    public VerifyResetEmailProfile()
    {
        CreateMap<Guid, VerifyResetEmailCommand>()
            .ForMember
            (
                dest => dest.ResetEmailToken,
                exp => exp.MapFrom(src => src)
            );

        CreateMap<VerifyResetEmailCommandResult, VerifyResetEmailResponse>();
    }
}