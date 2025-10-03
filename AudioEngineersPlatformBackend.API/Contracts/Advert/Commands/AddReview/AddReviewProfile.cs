using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.AddReview;
using AutoMapper;

namespace API.Contracts.Advert.Commands.AddReview;

public class AddReviewProfile : Profile
{
    public AddReviewProfile()
    {
        CreateMap<AddReviewRequest, AddReviewCommand>()
            .ForMember(exp => exp.IdAdvert, opt => opt.Ignore());

        CreateMap<AddReviewCommandResult, AddReviewResponse>();
    }
}