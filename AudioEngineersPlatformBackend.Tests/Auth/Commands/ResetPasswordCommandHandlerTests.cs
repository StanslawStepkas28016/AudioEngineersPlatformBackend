using API.Contracts.Auth.Commands.ResetPassword;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetPassword;
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

[TestSubject(typeof(ResetPasswordCommandHandler))]
public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<ILogger<ResetPasswordCommandHandler>> _loggerMock;
    private readonly ResetPasswordCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<IUrlGeneratorUtil> _urlGeneratorMock;
    private readonly Mock<ISesService> _sesServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public ResetPasswordCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ResetPasswordCommandHandler>>();
        _concreteValidator = new ResetPasswordCommandValidator();
        _concreteMapper = new Mapper
            (new MapperConfiguration(exp => exp.AddProfile(new ResetPasswordProfile()), new NullLoggerFactory()));
        _authRepositoryMock = new Mock<IAuthRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _urlGeneratorMock = new Mock<IUrlGeneratorUtil>();
        _sesServiceMock = new Mock<ISesService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private Task<User> GenerateUser()
    {
        UserAuthLog userAuthLog = UserAuthLog.Create();

        userAuthLog.SetIsVerifiedStatus(true);

        User user = User.CreateWithId
        (
            Guid.Parse("9BC8EADB-6AFA-47F7-B2B9-F738280CA62C"),
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
    public async Task ResetPassword_Should_Set_IsResettingPassword_Status_And_Send_Email_With_Confirmation()
    {
        // Arrange
        User user = await GenerateUser();

        ResetPasswordCommand command = new ResetPasswordCommand
        {
            IdUser = user.IdUser,
            CurrentPassword = "MyStrongPassword1234!",
            NewPassword = "NewPassword123!",
            NewPasswordRepeated = "NewPassword123!"
        };

        ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _urlGeneratorMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup
                (exp => exp.FindUserAndUserLogByIdUserAsync(user.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(exp => exp.VerifyHashedPassword(user, user.Password, command.CurrentPassword))
            .Returns(PasswordVerificationResult.Success);

        _urlGeneratorMock
            .Setup(exp => exp.GenerateResetVerificationUrl(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("WhateverUrl");

        // Act
        ResetPasswordCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        user
            .UserAuthLog
            .IsResettingPassword
            .Should()
            .BeTrue();
        
        result
            .Instructions
            .Should()
            .BeEquivalentTo("Instructions were sent to your email.");

        _authRepositoryMock
            .Verify(exp => exp.AddTokenAsync(It.IsAny<Token>(), It.IsAny<CancellationToken>()), Times.Once);

        _urlGeneratorMock.Verify
            (exp => exp.GenerateResetVerificationUrl(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        _unitOfWorkMock.Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_Should_Throw_Exception_When_NewEmails_Are_Not_Matching()
    {
        // Arrange
        User user = await GenerateUser();

        ResetPasswordCommand command = new ResetPasswordCommand
        {
            IdUser = user.IdUser, CurrentPassword = "MyStrongPassword1234!", NewPassword = "NewPassword123!",
            NewPasswordRepeated = "NotMatchingPassword123!"
        };

        ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _urlGeneratorMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup
                (exp => exp.FindUserAndUserLogByIdUserAsync(user.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(exp => exp.VerifyHashedPassword(user, user.Password, command.CurrentPassword))
            .Returns(PasswordVerificationResult.Success);

        _urlGeneratorMock
            .Setup(exp => exp.GenerateResetVerificationUrl(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("WhateverUrl");

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("New password must match.");
    }

    [Fact]
    public async Task ResetPassword_Should_Throw_Exception_When_Current_Password_Is_Invalid()
    {
        // Arrange
        User user = await GenerateUser();

        ResetPasswordCommand command = new ResetPasswordCommand
        {
            IdUser = user.IdUser,
            CurrentPassword = "InvalidPassword1234!!",
            NewPassword = "NewPassword1234!",
            NewPasswordRepeated = "NewPassword1234!"
        };

        ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _urlGeneratorMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup
                (exp => exp.FindUserAndUserLogByIdUserAsync(user.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(exp => exp.VerifyHashedPassword(user, user.Password, command.CurrentPassword))
            .Returns(PasswordVerificationResult.Failed);

        _urlGeneratorMock
            .Setup(exp => exp.GenerateResetVerificationUrl(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("WhateverUrl");

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("Invalid current password.");
    }
}