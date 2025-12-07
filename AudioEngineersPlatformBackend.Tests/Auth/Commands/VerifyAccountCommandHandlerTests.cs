using API.Contracts.Auth.Commands.VerifyAccount;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyAccount;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Auth.Commands;

[TestSubject(typeof(VerifyAccountCommandHandler))]
public class VerifyAccountCommandHandlerTests
{
    private readonly Mock<ILogger<VerifyAccountCommandHandler>> _loggerMock;
    private readonly VerifyAccountCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public VerifyAccountCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<VerifyAccountCommandHandler>>();
        _concreteValidator = new VerifyAccountCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration(cfg => cfg.AddProfile(new VerifyAccountProfile()), new NullLoggerFactory())
        );
        _authRepositoryMock = new Mock<IAuthRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private Task<User> GenerateUser(
        bool isVerified
    )
    {
        UserAuthLog userAuthLog = UserAuthLog.Create();

        userAuthLog.SetIsVerifiedStatus(isVerified);

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

        Token token = Token.Create
            (nameof(TokenNames.VerifyAccountToken), "123123", DateTime.Now.AddHours(1), user.IdUser);

        user.UserAuthLog = userAuthLog;
        user.Tokens = new List<Token>();
        user.Tokens.Add(token);

        return Task.FromResult(user);
    }

    [Fact]
    public async Task Verify_Should_Verify_User_Account_When_Code_From_Email_Is_Valid()
    {
        // Arrange
        VerifyAccountCommand command = new VerifyAccountCommand { VerificationCode = "123123" };

        VerifyAccountCommandHandler handler = new VerifyAccountCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        User user = await GenerateUser(false);

        _authRepositoryMock
            .Setup
            (exp => exp.FindUserAndUserLogAndTokenByTokenAsync
                (command.VerificationCode, It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(user);

        // Act
        VerifyAccountCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _authRepositoryMock
            .Verify(exp => exp.DeleteTokenByValueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);

        user
            .UserAuthLog
            .IsVerified
            .Should()
            .BeTrue();

        result
            .Message
            .Should()
            .BeEquivalentTo("Successfully verified your account.");
    }

    [Fact]
    public async Task Verify_Should_Throw_If_Verification_Code_Format_Is_Invalid()
    {
        // Arrange
        VerifyAccountCommand command = new VerifyAccountCommand { VerificationCode = "231" };

        VerifyAccountCommandHandler handler = new VerifyAccountCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<ArgumentException>()
            .WithMessage($"{nameof(VerificationCodeVo.VerificationCode)} must be 6 characters long.");

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Verify_Should_Throw_If_Verification_Code_Is_Invalid()
    {
        // Arrange
        VerifyAccountCommand command = new VerifyAccountCommand { VerificationCode = "231123" };

        VerifyAccountCommandHandler handler = new VerifyAccountCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup
            (exp => exp.FindUserAndUserLogAndTokenByTokenAsync
                (command.VerificationCode, It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((User)null!);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage($"Provided {nameof(VerificationCodeVo.VerificationCode)} is invalid.");

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}