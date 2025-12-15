using API.Contracts.Auth.Commands.ResetEmail;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetEmail;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Auth.Commands;

[TestSubject(typeof(ResetEmailCommandHandler))]
public class ResetEmailCommandHandlerTests
{
    private readonly Mock<ILogger<ResetEmailCommandHandler>> _loggerMock;
    private readonly ResetEmailCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IUrlGeneratorUtil> _urlGeneratorMock;
    private readonly Mock<ISesService> _sesServiceMock;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public ResetEmailCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ResetEmailCommandHandler>>();
        _concreteValidator = new ResetEmailCommandValidator();
        _concreteMapper = new Mapper
            (new MapperConfiguration(exp => exp.AddProfile(new ResetEmailProfile()), new NullLoggerFactory()));
        _urlGeneratorMock = new Mock<IUrlGeneratorUtil>();
        _sesServiceMock = new Mock<ISesService>();
        _authRepositoryMock = new Mock<IAuthRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private Task<User> GenerateUser()
    {
        UserAuthLog userAuthLog = UserAuthLog.Create();

        userAuthLog.SetIsVerifiedStatus(true);

        User user = User.Create
        (
            "John",
            "Doe",
            "my@gmail.com",
            "+48123123123",
            "MyStrongPassword1234!",
            It.IsAny<Guid>(),
            userAuthLog.IdUserAuthLog
        );

        user.UserAuthLog = userAuthLog;

        return Task.FromResult(user);
    }

    [Fact]
    public async Task ResetEmail_Should_Return_Instructions_And_Send_An_Email_To_New_Inbox()
    {
        // Arrange
        ResetEmailCommand command = new ResetEmailCommand
            { IdUser = Guid.Parse("C49149AE-18CA-487C-8DE6-B249E31B4A6E"), NewEmail = "test@gmail.com" };

        ResetEmailCommandHandler handler = new ResetEmailCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _urlGeneratorMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        User user = await GenerateUser();

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _authRepositoryMock
            .Setup(exp => exp.IsEmailAlreadyTakenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _urlGeneratorMock
            .Setup(exp => exp.GenerateResetVerificationUrl(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("Some random url");

        // Act
        ResetEmailCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        result
            .Instructions
            .Should()
            .BeEquivalentTo("Instructions were sent to your new email inbox.");

        user
            .UserAuthLog
            .IsResettingEmail
            .Should()
            .BeTrue();

        _authRepositoryMock
            .Verify(exp => exp.AddTokenAsync(It.IsAny<Token>(), It.IsAny<CancellationToken>()), Times.Once);

        _authRepositoryMock
            .Verify
            (
                exp => exp.DeleteAllTokensWithSpecificNameByIdUserAsync
                    (It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

        _sesServiceMock
            .Verify
            (
                exp => exp.SendEmailResetEmailAsync
                (
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ),
                Times.Once
            );

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ResetEmail_Should_Throw_Exception_If_NewEmail_Is_Equivalent_To_Old_Email()
    {
        // Arrange
        ResetEmailCommand command = new ResetEmailCommand
            { IdUser = Guid.Parse("C49149AE-18CA-487C-8DE6-B249E31B4A6E"), NewEmail = "my@gmail.com" };

        ResetEmailCommandHandler handler = new ResetEmailCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _urlGeneratorMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(await GenerateUser());

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("New email must differ from the old one.");
    }

    [Fact]
    public async Task ResetEmail_Should_Throw_Exception_If_NewEmail_Is_Already_Taken()
    {
        // Arrange
        ResetEmailCommand command = new ResetEmailCommand
            { IdUser = Guid.Parse("C49149AE-18CA-487C-8DE6-B249E31B4A6E"), NewEmail = "test@gmail.com" };

        ResetEmailCommandHandler handler = new ResetEmailCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _urlGeneratorMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(await GenerateUser());

        _authRepositoryMock
            .Setup(exp => exp.IsEmailAlreadyTakenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("Email already taken.");
    }
}