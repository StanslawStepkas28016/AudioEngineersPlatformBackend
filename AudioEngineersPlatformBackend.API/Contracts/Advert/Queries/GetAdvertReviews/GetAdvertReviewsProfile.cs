using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertReviews;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;

namespace API.Contracts.Advert.Queries.GetAdvertReviews;

public class GetAdvertReviewsProfile : Profile
{
    public GetAdvertReviewsProfile()
    {
        CreateMap<GetAdvertReviewsRequest, GetAdvertReviewsQuery>()
            .ForMember(exp => exp.IdAdvert, opt => opt.Ignore());

        CreateMap<PagedListDto<ReviewDto>, GetAdvertReviewsQueryResult>()
            .ForMember
            (
                dest => dest.PagedAdvertReviews,
                opt => opt.MapFrom
                ((
                        src,
                        dest
                    ) => dest.PagedAdvertReviews = src
                )
            );

        CreateMap<GetAdvertReviewsQueryResult, GetAdvertReviewsResponse>();
    }
}