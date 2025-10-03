using API.Contracts.Chat.Commands;
using API.Contracts.Chat.Commands.SendTextMessage;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendTextMessage;
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

[TestSubject(typeof(SendTextMessageCommandHandler))]
public class SendTextMessageCommandHandlerTests
{
    private readonly Mock<ILogger<SendTextMessageCommandHandler>> _loggerMock;
    private readonly SendTextMessageCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public SendTextMessageCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<SendTextMessageCommandHandler>>();
        _concreteValidator = new SendTextMessageCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new SendTextMessageProfile()), new NullLoggerFactory())
        );
        _userRepositoryMock = new Mock<IUserRepository>();
        _chatRepositoryMock = new Mock<IChatRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task SendTextMessage_Should_Persist_Data_And_Return_Message_Data()
    {
        // Arrange
        SendTextMessageCommand command = new SendTextMessageCommand
        {
            IdUserSender = Guid.Parse("7CF841A6-678C-4D7F-B188-5494AA821D1C"),
            IdUserRecipient = Guid.Parse("2E7E688E-02A8-49E6-AA62-047F2755476D"),
            TextContent = "Some example content, does not matter."
        };

        SendTextMessageCommandHandler handler = new SendTextMessageCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup
            (exp => exp.DoesUserExistByIdUserAsync
                (It.Is<Guid>(val => val == command.IdUserSender), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup
            (exp => exp.DoesUserExistByIdUserAsync
                (It.Is<Guid>(val => val == command.IdUserRecipient), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup
            (exp => exp.AreUsersInTheSameRoleAsync
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup
            (exp => exp.FindUserInfoByIdUserAsync
                (It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(Tuple.Create("John", "Doe"));

        // Act
        SendTextMessageCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _chatRepositoryMock
            .Verify(exp => exp.SaveMessageAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Once);

        _chatRepositoryMock
            .Verify
                (exp => exp.SaveUserMessageAsync(It.IsAny<UserMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);

        result
            .IdMessage
            .Should()
            .NotBeEmpty();

        result
            .IdUserSender
            .Should()
            .NotBeEmpty();

        result
            .SenderFirstName
            .Should()
            .NotBeEmpty();

        result
            .SenderLastName
            .Should()
            .NotBeEmpty();

        result
            .TextContent
            .Should()
            .NotBeEmpty();

        result
            .FileName
            .Should()
            .BeEmpty();

        result
            .FileUrl
            .Should()
            .BeEmpty();

        result
            .FileUrl
            .Should()
            .BeEmpty();

        result
            .DateSent
            .Should()
            .NotBeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task SendTextMessage_Should_Throw_Exception_When_At_Least_One_User_Does_Not_Exist()
    {
        // Arrange
        SendTextMessageCommand command = new SendTextMessageCommand
        {
            IdUserSender = Guid.Parse("7CF841A6-678C-4D7F-B188-5494AA821D1C"),
            IdUserRecipient = Guid.Parse("2E7E688E-02A8-49E6-AA62-047F2755476D"),
            TextContent = "Some example content, does not matter."
        };

        SendTextMessageCommandHandler handler = new SendTextMessageCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup
            (exp => exp.DoesUserExistByIdUserAsync
                (It.Is<Guid>(val => val == command.IdUserSender), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup
            (exp => exp.DoesUserExistByIdUserAsync
                (It.Is<Guid>(val => val == command.IdUserRecipient), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(false);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("Users not found.");
    }
    
    [Fact]
    public async Task SendTextMessage_Should_Throw_Exception_When_Users_Are_In_The_Same_Role()
    {
        // Arrange
        SendTextMessageCommand command = new SendTextMessageCommand
        {
            IdUserSender = Guid.Parse("7CF841A6-678C-4D7F-B188-5494AA821D1C"),
            IdUserRecipient = Guid.Parse("2E7E688E-02A8-49E6-AA62-047F2755476D"),
            TextContent = "Some example content, does not matter."
        };

        SendTextMessageCommandHandler handler = new SendTextMessageCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup
            (exp => exp.DoesUserExistByIdUserAsync
                (It.Is<Guid>(val => val == command.IdUserSender), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup
            (exp => exp.DoesUserExistByIdUserAsync
                (It.Is<Guid>(val => val == command.IdUserRecipient), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);
        
        _userRepositoryMock
            .Setup
            (exp => exp.AreUsersInTheSameRoleAsync
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("Users cannot be in the same role.");
    }
}