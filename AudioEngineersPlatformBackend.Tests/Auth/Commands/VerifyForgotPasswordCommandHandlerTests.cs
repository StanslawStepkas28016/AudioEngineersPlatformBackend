using API.Contracts.Auth.Commands.VerifyForgotPassword;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyForgotPassword;
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

[TestSubject(typeof(VerifyForgotPasswordCommandHandler))]
public class VerifyForgotPasswordCommandHandlerTests
{
    private readonly Mock<ILogger<VerifyForgotPasswordCommandHandler>> _loggerMock;
    private readonly VerifyForgotPasswordCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public VerifyForgotPasswordCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<VerifyForgotPasswordCommandHandler>>();
        _concreteValidator = new VerifyForgotPasswordCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration(exp => exp.AddProfile(new VerifyForgotPasswordProfile()), new NullLoggerFactory())
        );
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _authRepositoryMock = new Mock<IAuthRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private Task<User> GenerateUser(
        string password
    )
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

        user.SetHashedPassword(new PasswordHasher<User>().HashPassword(user, user.Password));

        user.UserAuthLog = userAuthLog;

        Token token = Token.Create
        (
            nameof(TokenNames.ForgotPasswordToken),
            Guid
                .Parse("98B4712F-E30D-4798-9EA4-7BEF03E3809B")
                .ToString(),
            DateTime.UtcNow.AddHours(1),
            user.IdUser
        );

        user.Tokens = new List<Token>();

        user.Tokens.Add(token);

        return Task.FromResult(user);
    }

    [Fact]
    public async Task
        VerifyForgotPassword_Should_Change_Users_Password_And_Set_Their_IsRemindingPasswordStatus_To_False()
    {
        // Arrange
        VerifyForgotPasswordCommand command = new VerifyForgotPasswordCommand
        {
            ForgotPasswordToken = Guid.Parse("98B4712F-E30D-4798-9EA4-7BEF03E3809B"),
            NewPassword = "MyStrongPassword1234!", NewPasswordRepeated = "MyStrongPassword1234!"
        };

        VerifyForgotPasswordCommandHandler handler = new VerifyForgotPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object
        );

        User generatedUser = await GenerateUser("SomeOldPassword1234!");

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogAndTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(generatedUser);

        _passwordHasherMock
            .Setup(exp => exp.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Failed);

        _passwordHasherMock
            .Setup(exp => exp.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("SomeHasWhichDoesNotMatter");

        // Act
        VerifyForgotPasswordCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        generatedUser
            .UserAuthLog
            .IsRemindingPassword
            .Should()
            .BeFalse();

        result
            .Message
            .Should()
            .BeEquivalentTo("Password reset completed.");

        _authRepositoryMock
            .Verify(exp => exp.DeleteTokenByValueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task
        VerifyForgotPassword_Should_Throw_Exception_If_New_Password_Does_Not_Differ_From_The_Old_One()
    {
        // Arrange
        VerifyForgotPasswordCommand command = new VerifyForgotPasswordCommand
        {
            ForgotPasswordToken = Guid.Parse("98B4712F-E30D-4798-9EA4-7BEF03E3809B"),
            NewPassword = "MyStrongPassword1234!", NewPasswordRepeated = "MyStrongPassword1234!"
        };

        VerifyForgotPasswordCommandHandler handler = new VerifyForgotPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object
        );

        User generatedUser = await GenerateUser("MyStrongPassword1234!"); // Old password.

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogAndTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(generatedUser);

        _passwordHasherMock
            .Setup(exp => exp.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Success);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("New password must be differ from the old password.");
    }

    [Fact]
    public async Task
        VerifyForgotPassword_Should_Throw_Exception_If_ForgotPasswordToken_Is_Invalid()
    {
        // Arrange
        VerifyForgotPasswordCommand command = new VerifyForgotPasswordCommand
        {
            ForgotPasswordToken = Guid.Parse("98B4712F-E30D-4798-9EA4-7BEF03E3809B"),
            NewPassword = "MyStrongPassword1234!", NewPasswordRepeated = "MyStrongPassword1234!"
        };

        VerifyForgotPasswordCommandHandler handler = new VerifyForgotPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogAndTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("User not found.");
    }
}