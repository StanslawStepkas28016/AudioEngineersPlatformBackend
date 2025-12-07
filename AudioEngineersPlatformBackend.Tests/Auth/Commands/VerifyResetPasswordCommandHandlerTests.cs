using API.Contracts.Auth.Commands.VerifyResetPassword;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetPassword;
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

[TestSubject(typeof(VerifyResetPasswordCommandHandler))]
public class VerifyResetPasswordCommandHandlerTests
{
    private readonly Mock<ILogger<VerifyResetPasswordCommandHandler>> _loggerMock;
    private readonly VerifyResetPasswordValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public VerifyResetPasswordCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<VerifyResetPasswordCommandHandler>>();
        _concreteValidator = new VerifyResetPasswordValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
            (
                exp => exp.AddProfile(new VerifyResetPasswordProfile()),
                new NullLoggerFactory()
            )
        );
        _authRepositoryMock = new Mock<IAuthRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private Task<User> GenerateUser(
        DateTime tokenExpireDate
    )
    {
        UserAuthLog userAuthLog = UserAuthLog.Create();

        userAuthLog.SetIsVerifiedStatus(true);
        userAuthLog.SetIsResettingPassword(true);

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

        Token token = Token.Create
        (
            nameof(TokenNames.ResetPasswordToken),
            Guid
                .Parse("528B382D-F2C1-44BD-8A82-C6C087BA1795")
                .ToString(),
            tokenExpireDate,
            user.IdUser
        );

        user.Tokens = new List<Token>();
        user.Tokens.Add(token);

        return Task.FromResult(user);
    }

    [Fact]
    public async Task VerifyResetPassword_Should_Set_IsResettingPassword_Status_And_Delete_The_Active_Token()
    {
        // Arrange
        VerifyResetPasswordCommand command = new VerifyResetPasswordCommand
            { ResetPasswordToken = Guid.Parse("528B382D-F2C1-44BD-8A82-C6C087BA1795") };

        VerifyResetPasswordCommandHandler handler = new VerifyResetPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        User user = await GenerateUser(DateTime.UtcNow.AddHours(1));

        _authRepositoryMock
            .Setup
                (exp => exp.FindUserAndUserLogAndTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        VerifyResetPasswordCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _authRepositoryMock
            .Verify(exp => exp.DeleteTokenByValueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);

        user
            .UserAuthLog
            .IsResettingPassword
            .Should()
            .BeFalse();

        result
            .Message
            .Should()
            .BeEquivalentTo("Password reset successful.");
    }

    [Fact]
    public async Task VerifyResetPassword_Should_Throw_Exception_When_Token_Is_Invalid()
    {
        // Arrange
        VerifyResetPasswordCommand command = new VerifyResetPasswordCommand
            { ResetPasswordToken = Guid.Parse("118B382D-F2C1-44BD-8A82-C6C087BA1795") };

        VerifyResetPasswordCommandHandler handler = new VerifyResetPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup
                (exp => exp.FindUserAndUserLogAndTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("User does not exist.");
    }

    [Fact]
    public async Task VerifyResetPassword_Should_Throw_Exception_When_Token_Is_Expired()
    {
        // Arrange
        VerifyResetPasswordCommand command = new VerifyResetPasswordCommand
            { ResetPasswordToken = Guid.Parse("118B382D-F2C1-44BD-8A82-C6C087BA1795") };

        VerifyResetPasswordCommandHandler handler = new VerifyResetPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        User user = await GenerateUser(DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)));

        _authRepositoryMock
            .Setup
                (exp => exp.FindUserAndUserLogAndTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("Token expired.");
    }
}