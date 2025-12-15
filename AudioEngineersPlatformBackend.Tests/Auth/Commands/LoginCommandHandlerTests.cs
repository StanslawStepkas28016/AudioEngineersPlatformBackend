using API.Contracts.Auth.Commands.Login;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Login;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Auth.Commands;

[TestSubject(typeof(LoginCommandHandler))]
public class LoginCommandHandlerTests
{
    private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;
    private readonly Mapper _concreteMapper;
    private readonly LoginCommandValidator _concreteValidator;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public LoginCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<LoginCommandHandler>>();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration(cfg => cfg.AddProfile(new LoginProfile()), new NullLoggerFactory())
        );
        _concreteValidator = new LoginCommandValidator();
        _authRepositoryMock = new Mock<IAuthRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
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

        user.SetHashedPassword(new PasswordHasher<User>().HashPassword(user, user.Password));

        user.UserAuthLog = userAuthLog;

        return Task.FromResult(user);
    }

    [Fact]
    public async Task Login_Should_Return_Correct_Login_Data()
    {
        // Arrange
        LoginCommand command = new LoginCommand { Email = "my@gmail.com", Password = "MyStrongPassword1234!" };

        LoginCommandHandler handler = new LoginCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogAndRoleByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(await GenerateUser(true));

        _passwordHasherMock
            .Setup(exp => exp.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Success);

        // Act
        LoginCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        result
            .User
            .Email
            .Should()
            .BeEquivalentTo(command.Email);

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Login_Should_Throw_Exception_For_Invalid_Credentials()
    {
        // Arrange
        LoginCommand command = new LoginCommand { Email = "my@gmail.com", Password = "WrongPassword1234!" };

        LoginCommandHandler handler = new LoginCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogAndRoleByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(await GenerateUser(true));

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<ArgumentException>()
            .WithMessage("Invalid credentials.");
    }

    [Fact]
    public async Task Login_Should_Throw_Exception_If_User_Status_Is_Invalid()
    {
        // Arrange
        LoginCommand command = new LoginCommand { Email = "my@gmail.com", Password = "WrongPassword1234!" };

        LoginCommandHandler handler = new LoginCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogAndRoleByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(await GenerateUser(false));

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("User is not verified.");
    }
}