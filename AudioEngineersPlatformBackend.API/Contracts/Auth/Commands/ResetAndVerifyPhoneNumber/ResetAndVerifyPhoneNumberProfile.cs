using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetAndVerifyPhoneNumber;
using AutoMapper;

namespace API.Contracts.Auth.Commands.ResetAndVerifyPhoneNumber;

public class ResetAndVerifyPhoneNumberProfile : Profile
{
    public ResetAndVerifyPhoneNumberProfile()
    {
        CreateMap<ResetAndVerifyPhoneNumberRequest, ResetAndVerifyPhoneNumberCommand>()
            .ForMember(dest => dest.IdUser, exp => exp.Ignore());

        CreateMap<ResetAndVerifyPhoneNumberCommandResult, ResetAndVerifyPhoneNumberResponse>();
    }
}