using API.Contracts.Chat.Queries.GetUserData;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserData;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Chat.Queries;

[TestSubject(typeof(GetUserDataQueryHandler))]
public class GetUserDataQueryHandlerTests
{
    private readonly Mock<ILogger<GetUserDataQueryHandler>> _loggerMock;
    private readonly GetUserDataQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IChatRepository> _chatRepositoryMock;

    public GetUserDataQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetUserDataQueryHandler>>();
        _concreteValidator = new GetUserDataQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
            (
                exp => exp.AddProfile(new GetUserDataProfile()),
                new NullLoggerFactory()
            )
        );
        _chatRepositoryMock = new Mock<IChatRepository>();
    }

    private Task<UserDataDto> GenerateUserDataDto()
    {
        return Task.FromResult
        (
            new UserDataDto
            {
                IdUser = Guid.Parse("B7833E6F-62A5-4AE7-B869-D32A68CE50CC"),
                FirstName = "John",
                LastName = "Doe"
            }
        );
    }

    [Fact]
    public async Task GetUserData_Should_Return_Non_Empty_Data()
    {
        // Arrange
        GetUserDataQuery query = new GetUserDataQuery { IdUser = Guid.Parse("B7833E6F-62A5-4AE7-B869-D32A68CE50CC") };

        GetUserDataQueryHandler handler = new GetUserDataQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _chatRepositoryMock.Object
        );

        _chatRepositoryMock
            .Setup(exp => exp.FindUserDataAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(await GenerateUserDataDto());

        // Act
        GetUserDataQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .IdUser
            .Should()
            .NotBeEmpty();

        result
            .FirstName
            .Should()
            .NotBeEmpty();

        result
            .LastName
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task GetUserData_Throw_Exception_When_Data_Is_Not_Found()
    {
        // Arrange
        GetUserDataQuery query = new GetUserDataQuery { IdUser = Guid.Parse("B7833E6F-62A5-4AE7-B869-D32A68CE50CC") };

        GetUserDataQueryHandler handler = new GetUserDataQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _chatRepositoryMock.Object
        );

        _chatRepositoryMock
            .Setup(exp => exp.FindUserDataAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDataDto)null!);

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("User not found.");
    }
}