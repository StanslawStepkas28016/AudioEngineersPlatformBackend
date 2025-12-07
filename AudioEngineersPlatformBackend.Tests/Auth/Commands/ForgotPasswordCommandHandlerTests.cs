using API.Contracts.Auth.Commands.ForgotPassword;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ForgotPassword;
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

[TestSubject(typeof(ForgotPasswordCommandHandler))]
public class ForgotPasswordCommandHandlerTests
{
    private readonly Mock<ILogger<ForgotPasswordCommandHandler>> _loggerMock;
    private readonly ForgotPasswordCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUrlGeneratorUtil> _urlGeneratorUtilMock;
    private readonly Mock<ISesService> _sesServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public ForgotPasswordCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ForgotPasswordCommandHandler>>();
        _concreteValidator = new ForgotPasswordCommandValidator();
        _concreteMapper = new Mapper
            (new MapperConfiguration(exp => exp.AddProfile(new ForgotPasswordProfile()), new NullLoggerFactory()));
        _authRepositoryMock = new Mock<IAuthRepository>();
        _urlGeneratorUtilMock = new Mock<IUrlGeneratorUtil>();
        _sesServiceMock = new Mock<ISesService>();
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
    public async Task
        ForgotPassword_Should_Add_A_ForgotPasswordToken_Mark_The_User_As_IsRemindingPassword_And_Send_Email()
    {
        // Arrange
        ForgotPasswordCommand command = new ForgotPasswordCommand { Email = "s28016@pjwstk.edu.pl" };

        ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _urlGeneratorUtilMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        User user = await GenerateUser(true);

        _authRepositoryMock
            .Setup
                (exp => exp.FindUserAndUserLogAndRoleByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        ForgotPasswordCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        result
            .Instructions
            .Should()
            .BeEquivalentTo("Instructions for resetting your password were sent to your email address.");

        _authRepositoryMock
            .Verify(exp => exp.AddTokenAsync(It.IsAny<Token>(), It.IsAny<CancellationToken>()), Times.Once);

        _sesServiceMock
            .Verify
            (
                exp => exp.SendForgotPasswordResetEmailAsync
                    (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once
            );
    }

    [Fact]
    public async Task
        ForgotPassword_Should_Throw_An_Exception_When_User_With_Specified_Email_Address_Does_Not_Exist()
    {
        // Arrange
        ForgotPasswordCommand command = new ForgotPasswordCommand { Email = "s28016@pjwstk.edu.pl" };

        ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _urlGeneratorUtilMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup
                (exp => exp.FindUserAndUserLogAndRoleByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("User does not exist.");
    }

    [Fact]
    public async Task
        ForgotPassword_Should_Throw_An_Exception_If_Provided_Email_Is_Invalid()
    {
        // Arrange
        ForgotPasswordCommand command = new ForgotPasswordCommand { Email = ".pl" };

        ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _urlGeneratorUtilMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<ArgumentException>()
            .WithMessage("Invalid email.");
    }
}