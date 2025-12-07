using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetEmail;
using AutoMapper;

namespace API.Contracts.Auth.Commands.ResetEmail;

public class ResetEmailProfile : Profile
{
    public ResetEmailProfile()
    {
        CreateMap<ResetEmailRequest, ResetEmailCommand>()
            .ForMember(dest => dest.IdUser, exp => exp.Ignore());

        CreateMap<ResetEmailCommandResult, ResetEmailResponse>();
    }
}