using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyAccount;
using AudioEngineersPlatformBackend.Domain.Entities;
using AutoMapper;

namespace API.Contracts.Auth.Commands.VerifyAccount;

public class VerifyAccountProfile : Profile
{
    public VerifyAccountProfile()
    {
        CreateMap<VerifyAccountRequest, VerifyAccountCommand>();

        CreateMap<VerifyAccountCommandResult, VerifyAccountResponse>();
    }
}