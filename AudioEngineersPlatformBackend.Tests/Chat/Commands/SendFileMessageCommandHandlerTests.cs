using API.Contracts.Chat.Commands.SendFileMessage;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendFileMessage;
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

[TestSubject(typeof(SendFileMessageCommandHandler))]
public class SendFileMessageCommandHandlerTests
{
    private readonly Mock<ILogger<SendFileMessageCommandHandler>> _loggerMock;
    private readonly SendFileMessageCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<IS3Service> _s3ServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public SendFileMessageCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<SendFileMessageCommandHandler>>();
        _concreteValidator = new SendFileMessageCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new SendFileMessageProfile()), new NullLoggerFactory())
        );
        _userRepositoryMock = new Mock<IUserRepository>();
        _chatRepositoryMock = new Mock<IChatRepository>();
        _s3ServiceMock = new Mock<IS3Service>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task SendFileMessage_Should_Persist_Data_And_Return_Message_Data()
    {
        // Arrange
        SendFileMessageCommand command = new SendFileMessageCommand
        {
            IdUserSender = Guid.Parse("7CF841A6-678C-4D7F-B188-5494AA821D1C"),
            IdUserRecipient = Guid.Parse("2E7E688E-02A8-49E6-AA62-047F2755476D"),
            FileKey = Guid.Parse("9FE41C8E-D1A0-437E-A73E-6F0B4D07ECCD"),
            FileName = "audio.wav"
        };

        SendFileMessageCommandHandler handler = new SendFileMessageCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _s3ServiceMock.Object,
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

        _s3ServiceMock
            .Setup
            (exp => exp.GetPreSignedUrlForReadAsync
                (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync("https://aws.amazon/sound-best-bucket/files/whatever-does-not-matter");

        // Act
        SendFileMessageCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

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
            .BeEmpty();

        result
            .FileName
            .Should()
            .NotBeEmpty();

        result
            .FileUrl
            .Should()
            .NotBeEmpty();

        result
            .FileUrl
            .Should()
            .NotBeEmpty();

        result
            .DateSent
            .Should()
            .NotBeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task SendFileMessage_Should_Throw_Exception_When_At_Least_One_User_Does_Not_Exist()
    {
        // Arrange
        SendFileMessageCommand command = new SendFileMessageCommand
        {
            IdUserSender = Guid.Parse("7CF841A6-678C-4D7F-B188-5494AA821D1C"),
            IdUserRecipient = Guid.Parse("2E7E688E-02A8-49E6-AA62-047F2755476D"),
            FileKey = Guid.Parse("9FE41C8E-D1A0-437E-A73E-6F0B4D07ECCD"),
            FileName = "audio.wav"
        };

        SendFileMessageCommandHandler handler = new SendFileMessageCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _s3ServiceMock.Object,
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
    public async Task SendFileMessage_Should_Throw_Exception_When_User_Are_In_The_Same_Role()
    {
        // Arrange
        SendFileMessageCommand command = new SendFileMessageCommand
        {
            IdUserSender = Guid.Parse("7CF841A6-678C-4D7F-B188-5494AA821D1C"),
            IdUserRecipient = Guid.Parse("2E7E688E-02A8-49E6-AA62-047F2755476D"),
            FileKey = Guid.Parse("9FE41C8E-D1A0-437E-A73E-6F0B4D07ECCD"),
            FileName = "audio.wav"
        };

        SendFileMessageCommandHandler handler = new SendFileMessageCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _s3ServiceMock.Object,
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