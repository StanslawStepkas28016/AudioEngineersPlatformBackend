using API.Contracts.Advert.Queries.GetAdvertDetails;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertDetails;
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

[TestSubject(typeof(GetAdvertDetailsQueryHandler))]
public class GetAdvertDetailsQueryHandlerTests
{
    private readonly Mock<ILogger<GetAdvertDetailsQueryHandler>> _loggerMock;
    private readonly GetAdvertDetailsQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAdvertRepository> _advertRepositoryMock;
    private readonly Mock<IS3Service> _s3ServiceMock;

    public GetAdvertDetailsQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAdvertDetailsQueryHandler>>();
        _concreteValidator = new GetAdvertDetailsQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new GetAdvertDetailsProfile()), new NullLoggerFactory())
        );
        _advertRepositoryMock = new Mock<IAdvertRepository>();
        _s3ServiceMock = new Mock<IS3Service>();
    }

    private Task<AdvertDetailsDto> GenerateAdvertDetailsDto()
    {
        return Task.FromResult
        (
            new AdvertDetailsDto
            {
                IdAdvert = Guid.Parse("31ba89aa-f10f-40e7-b4b0-7375da567997"),
                Title = "I will mix your song professionally!",
                IdUser = Guid.Parse("828daa53-9a49-40ad-97b3-31b0349bc08d"),
                UserFirstName = "Anna",
                UserLastName = "Kowalska",
                DateCreated = new DateTime(2025, 12, 12),
                DateModified = null,
                Description = "Some description.",
                Price = 350,
                CategoryName = "Mixing",
                CoverImageKey = Guid.Parse("df0f7b35-b8c2-4246-b7f7-ccc82d4a3a7e"),
                PortfolioUrl = "https://instagram/prod.mustang"
            }
        );
    }

    [Fact]
    public async Task GetAdvertDetails_Should_Return_Non_Empty_Data_When_There_Is_Data_Present()
    {
        // Arrange
        GetAdvertDetailsQuery query = new GetAdvertDetailsQuery
            { IdAdvert = Guid.Parse("2A892CFB-2A86-418E-AAF3-50910F197E8F") };

        GetAdvertDetailsQueryHandler handler = new GetAdvertDetailsQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _s3ServiceMock.Object
        );

        _advertRepositoryMock
            .Setup(exp => exp.DoesAdvertExistByIdAdvertAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup(exp => exp.FindAdvertDetailsByIdAdvertAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(await GenerateAdvertDetailsDto());

        _s3ServiceMock
            .Setup
            (exp => exp.GetPreSignedUrlForReadAsync
                (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync("https://mock-url.aws.com/whatever");

        // Act
        GetAdvertDetailsQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .IdAdvert
            .Should()
            .NotBeEmpty();

        result
            .IdUser
            .Should()
            .NotBeEmpty();

        result
            .CoverImageKey
            .Should()
            .NotBeEmpty();

        result
            .CoverImageUrl
            .Should()
            .NotBeEmpty();

        result
            .CategoryName
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task GetAdvertDetails_Should_Throw_Exception_When_Advert_Does_Not_Exist()
    {
        // Arrange
        GetAdvertDetailsQuery query = new GetAdvertDetailsQuery
            { IdAdvert = Guid.Parse("2A892CFB-2A86-418E-AAF3-50910F197E8F") };

        GetAdvertDetailsQueryHandler handler = new GetAdvertDetailsQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _s3ServiceMock.Object
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
            .WithMessage("Advert does not exist.");
    }
}