using API.Contracts.Advert.Queries.GetAllAdvertsSummaries;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAllAdvertsSumarries;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Advert.Queries;

[TestSubject(typeof(GetAllAdvertsSummariesQueryHandler))]
public class GetAllAdvertsSummariesQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllAdvertsSummariesQueryHandler>> _loggerMock;
    private readonly GetAllAdvertsSummariesQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAdvertRepository> _advertRepositoryMock;
    private readonly Mock<IS3Service> _s3ServiceMock;

    public GetAllAdvertsSummariesQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllAdvertsSummariesQueryHandler>>();
        _concreteValidator = new GetAllAdvertsSummariesQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new GetAllAdvertsSummariesProfile()), new NullLoggerFactory())
        );
        _advertRepositoryMock = new Mock<IAdvertRepository>();
        _s3ServiceMock = new Mock<IS3Service>();
    }

    private Task<PagedListDto<AdvertSummaryDto>> GeneratePagedList()
    {
        List<AdvertSummaryDto> list = new List<AdvertSummaryDto>();

        list.Add
        (
            new AdvertSummaryDto
            {
                IdAdvert = Guid.Parse("31ba89aa-f10f-40e7-b4b0-7375da567997"),
                Title = "I will mix your song professionally!",
                IdUser = Guid.Parse("828daa53-9a49-40ad-97b3-31b0349bc08d"),
                UserFirstName = "Anna",
                UserLastName = "Kowalska",
                DateCreated = new DateTime(2025, 12, 12),
                Description = "Some description",
                Price = 350,
                CategoryName = "Mixing",
                CoverImageKey = Guid.Parse("df0f7b35-b8c2-4246-b7f7-ccc82d4a3a7e"),
                CoverImageUrl =
                    "https://sound-best-bucket.s3.eu-north-1.amazonaws.com/images/df0f7b35-b8c2-4246-b7f7-ccc82d4a3a7e?X-Amz-Expires=900&response-content-disposition=attachment%3Bfilename%2A%3DUTF-8%27%27cover-image.jpg&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAYF6PKJ4U4K22EQKJ%2F20251021%2Feu-north-1%2Fs3%2Faws4_request&X-Amz-Date=20251021T112418Z&X-Amz-SignedHeaders=host&X-Amz-Signature=20fb8921c5a7dc7ad31599228dee8a76ea13855aa916eebb06ef234feb23f82d"
            }
        );

        return Task.FromResult
        (
            new PagedListDto<AdvertSummaryDto>
            (
                list,
                1,
                1,
                1
            )
        );
    }

    [Fact]
    public async Task GetAllAdvertsSummaries_Should_Return_Non_Empty_Data_When_There_Is_Data_Present()
    {
        // Arrange
        GetAllAdvertsSummariesQuery query = new GetAllAdvertsSummariesQuery
        {
            SortOrder = It.IsAny<string>(),
            Page = 1,
            PageSize = 1,
            SearchTerm = It.IsAny<string>()
        };

        GetAllAdvertsSummariesQueryHandler handler = new GetAllAdvertsSummariesQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _s3ServiceMock.Object
        );

        _advertRepositoryMock
            .Setup
            (exp => exp.FindAdvertSummariesAsync
                (
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(await GeneratePagedList());

        _s3ServiceMock
            .Setup
            (exp => exp.GetPreSignedUrlForReadAsync
                (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(It.IsAny<string>());

        // Act
        GetAllAdvertsSummariesQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .PagedAdvertSummaries
            .Items
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task GetAllAdvertsSummaries_Should_Throw_Exception_When_Input_Values_Are_Incorrect()
    {
        // Arrange
        GetAllAdvertsSummariesQuery query = new GetAllAdvertsSummariesQuery
        {
            SortOrder = It.IsAny<string>(),
            Page = 0,
            PageSize = 1,
            SearchTerm = It.IsAny<string>()
        };

        GetAllAdvertsSummariesQueryHandler handler = new GetAllAdvertsSummariesQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _s3ServiceMock.Object
        );

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<ArgumentException>()
            .WithMessage("Page must be at least 1.");
    }
}