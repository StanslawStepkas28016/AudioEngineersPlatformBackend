using API.Contracts.Advert.Queries.GetAdvertReviews;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertReviews;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Advert.Queries;

[TestSubject(typeof(GetAdvertReviewsQueryHandler))]
public class GetAdvertReviewsQueryHandlerTests
{
    private readonly Mock<ILogger<GetAdvertReviewsQueryHandler>> _loggerMock;
    private readonly GetAdvertReviewsQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAdvertRepository> _advertRepositoryMock;

    public GetAdvertReviewsQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAdvertReviewsQueryHandler>>();
        _concreteValidator = new GetAdvertReviewsQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new GetAdvertReviewsProfile()), new NullLoggerFactory())
        );
        _advertRepositoryMock = new Mock<IAdvertRepository>();
    }

    private Task<PagedListDto<ReviewDto>> GeneratePagedList()
    {
        List<ReviewDto> list = new List<ReviewDto>();

        list
            .Add
            (
                new ReviewDto
                {
                    IdReview = Guid.Parse("31ba89aa-f10f-40e7-b4b0-7375da567997"),
                    ClientFirstName = "John",
                    ClientLastName = "Doe",
                    Content = "Some content",
                    DateCreated = new DateTime(2025, 12, 12),
                    SatisfactionLevel = 4
                }
            );

        return Task.FromResult
        (
            new PagedListDto<ReviewDto>
            (
                list,
                1,
                1,
                1
            )
        );
    }

    [Fact]
    public async Task GetAdvertReviews_Should_Return_Non_Empty_Data_When_There_Is_Data_Present()
    {
        // Arrange
        GetAdvertReviewsQuery query = new GetAdvertReviewsQuery
        {
            IdAdvert = Guid.Parse("9490F061-E97B-4F69-96D0-36E1D181B232"),
            Page = 1,
            PageSize = 1
        };

        GetAdvertReviewsQueryHandler handler = new GetAdvertReviewsQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object
        );

        _advertRepositoryMock
            .Setup(exp => exp.DoesAdvertExistByIdAdvertAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup
            (exp => exp.FindAdvertReviewsAsync
                (
                    It.IsAny<Guid>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(await GeneratePagedList());

        // Act
        GetAdvertReviewsQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .PagedAdvertReviews
            .Items
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task GetAdvertReviews_Should_Throw_Exception_When_Advert_Does_Not_Exist()
    {
        // Arrange
        GetAdvertReviewsQuery query = new GetAdvertReviewsQuery
        {
            IdAdvert = Guid.Parse("9490F061-E97B-4F69-96D0-36E1D181B232"),
            Page = 1,
            PageSize = 1
        };

        GetAdvertReviewsQueryHandler handler = new GetAdvertReviewsQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object
        );

        _advertRepositoryMock
            .Setup(exp => exp.DoesAdvertExistByIdAdvertAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("Advert not found.");
    }
}