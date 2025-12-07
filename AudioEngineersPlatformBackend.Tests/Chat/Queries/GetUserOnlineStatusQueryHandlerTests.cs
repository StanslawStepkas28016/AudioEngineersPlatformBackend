using API.Contracts.Chat.Queries.GetUserOnlineStatus;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserOnlineStatus;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Chat.Queries;

[TestSubject(typeof(GetUserOnlineStatusQueryHandler))]
public class GetUserOnlineStatusQueryHandlerTests
{
    private readonly Mock<ILogger<GetUserOnlineStatusQueryHandler>> _loggerMock;
    private readonly GetUserOnlineStatusQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChatRepository> _chatRepositoryMock;

    public GetUserOnlineStatusQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetUserOnlineStatusQueryHandler>>();
        _concreteValidator = new GetUserOnlineStatusQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
            (
                exp => exp.AddProfile(new GetUserOnlineStatusProfile()),
                new NullLoggerFactory()
            )
        );
        _userRepositoryMock = new Mock<IUserRepository>();
        _chatRepositoryMock = new Mock<IChatRepository>();
    }

    [Fact]
    public async Task GetUserOnline_Should_Return_Correct_Status()
    {
        // Arrange
        GetUserOnlineStatusQuery query = new GetUserOnlineStatusQuery
            { IdUser = Guid.Parse("EB4E79E3-9046-43F5-8B4A-D858131C7AEC") };

        GetUserOnlineStatusQueryHandler handler = new GetUserOnlineStatusQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object
        );

        _userRepositoryMock
            .Setup
                (exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _chatRepositoryMock
            .Setup
                (exp => exp.IsUserOnlineAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        GetUserOnlineStatusQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .IsOnline
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public async Task GetUserOnline_Should_Throw_Exception_When_User_Does_Not_Exist()
    {
        // Arrange
        GetUserOnlineStatusQuery query = new GetUserOnlineStatusQuery
            { IdUser = Guid.Parse("EB4E79E3-9046-43F5-8B4A-D858131C7AEC") };

        GetUserOnlineStatusQueryHandler handler = new GetUserOnlineStatusQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object
        );

        _userRepositoryMock
            .Setup
                (exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _chatRepositoryMock
            .Setup
                (exp => exp.IsUserOnlineAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("User not found.");
    }

    [Fact]
    public async Task GetUserOnline_Should_Throw_Exception_For_Invalid_Data()
    {
        // Arrange
        GetUserOnlineStatusQuery query = new GetUserOnlineStatusQuery
            { IdUser = Guid.Empty };

        GetUserOnlineStatusQueryHandler handler = new GetUserOnlineStatusQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object
        );

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<ArgumentException>()
            .WithMessage("IdUser must be provided.");
    }
}