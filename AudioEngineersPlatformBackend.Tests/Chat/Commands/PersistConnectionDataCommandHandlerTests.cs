using API.Contracts.Chat.Commands.PersistConnectionData;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.PersistConnectionData;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Chat.Commands;

[TestSubject(typeof(PersistConnectionDataCommandHandler))]
public class PersistConnectionDataCommandHandlerTests
{
    private readonly Mock<ILogger<PersistConnectionDataCommandHandler>> _loggerMock;
    private readonly PersistConnectionDataCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public PersistConnectionDataCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<PersistConnectionDataCommandHandler>>();
        _concreteValidator = new PersistConnectionDataCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new PersistConnectionDataProfile()), new NullLoggerFactory())
        );
        _userRepositoryMock = new Mock<IUserRepository>();
        _chatRepositoryMock = new Mock<IChatRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task PersistConnectionData_Should_Add_Connection_Data_When_User_Is_Connecting()
    {
        // Arrange
        PersistConnectionDataCommand command = new PersistConnectionDataCommand
        {
            IdUser = Guid.Parse("E96B3F27-6267-4D08-BF2B-7CE5B13B1E43"),
            ConnectionId = "023124312312",
            IsConnecting = true
        };

        PersistConnectionDataCommandHandler handler = new PersistConnectionDataCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(command.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        PersistConnectionDataCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _chatRepositoryMock
            .Verify
            (
                exp => exp.AddConnectionDataAsync(It.IsAny<HubConnection>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

        _unitOfWorkMock
            .Verify
            (
                exp => exp.CompleteAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );

        result
            .Message
            .Should()
            .BeEquivalentTo("Successfully persisted connection data.");
    }

    [Fact]
    public async Task PersistConnectionData_Should_Delete_Connection_Data_When_User_Is_Connecting()
    {
        // Arrange
        PersistConnectionDataCommand command = new PersistConnectionDataCommand
        {
            IdUser = Guid.Parse("E96B3F27-6267-4D08-BF2B-7CE5B13B1E43"),
            ConnectionId = "023124312312",
            IsConnecting = false
        };

        PersistConnectionDataCommandHandler handler = new PersistConnectionDataCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(command.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        PersistConnectionDataCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _chatRepositoryMock
            .Verify
            (
                exp => exp.ExecuteDeleteConnectionDataAsync
                    (command.IdUser, command.ConnectionId, It.IsAny<CancellationToken>()),
                Times.Once
            );

        result
            .Message
            .Should()
            .BeEquivalentTo("Successfully persisted connection data.");
    }

    [Fact]
    public async Task PersistConnectionData_Should_Throw_Exception_When_User_Does_Not_Exist()
    {
        // Arrange
        PersistConnectionDataCommand command = new PersistConnectionDataCommand
        {
            IdUser = Guid.Parse("E96B3F27-6267-4D08-BF2B-7CE5B13B1E43"),
            ConnectionId = "023124312312",
            IsConnecting = false
        };

        PersistConnectionDataCommandHandler handler = new PersistConnectionDataCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(command.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("User not found.");
    }
}