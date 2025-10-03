using API.Contracts.Advert.Queries.GetIdAdvertByIdUser;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetIdAdvertByIdUser;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Advert.Queries;

[TestSubject(typeof(GetIdAdvertByIdUserQueryHandler))]
public class GetIdAdvertByIdUserQueryHandlerTests
{
    private readonly Mock<ILogger<GetIdAdvertByIdUserQueryHandler>> _loggerMock;
    private readonly GetIdAdvertByIdUserQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAdvertRepository> _advertRepositoryMock;

    public GetIdAdvertByIdUserQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetIdAdvertByIdUserQueryHandler>>();
        _concreteValidator = new GetIdAdvertByIdUserQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new GetIdAdvertByIdUserProfile()), new NullLoggerFactory())
        );
        _advertRepositoryMock = new Mock<IAdvertRepository>();
    }

    [Fact]
    public async Task GetIdById_Should_Return_Non_Empty_Data_When_There_Is_Data()
    {
        // Arrange
        GetIdAdvertByIdUserQuery query = new GetIdAdvertByIdUserQuery
            { IdUser = Guid.Parse("ACC8EAC9-EE28-4383-883D-78B0A90E1885") };

        GetIdAdvertByIdUserQueryHandler handler = new GetIdAdvertByIdUserQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object
        );

        _advertRepositoryMock
            .Setup(exp => exp.FindIdAdvertByIdUser(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.Parse("5C31AAA9-909F-4FC7-93B4-DB95CC26C89B"));

        // Act
        GetIdAdvertByIdUserQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .IdAdvert
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task GetIdById_Should_Throw_Exception_When_Advert_Does_Not_Exist()
    {
        // Arrange
        GetIdAdvertByIdUserQuery query = new GetIdAdvertByIdUserQuery
            { IdUser = Guid.Parse("ACC8EAC9-EE28-4383-883D-78B0A90E1885") };

        GetIdAdvertByIdUserQueryHandler handler = new GetIdAdvertByIdUserQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object
        );

        _advertRepositoryMock
            .Setup(exp => exp.FindIdAdvertByIdUser(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.Empty);

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("IdAdvert not found.");
    }
}