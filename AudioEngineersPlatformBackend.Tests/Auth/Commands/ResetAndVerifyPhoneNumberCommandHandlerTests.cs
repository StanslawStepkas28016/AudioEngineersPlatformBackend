using API.Contracts.Auth.Commands.ResetAndVerifyPhoneNumber;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetAndVerifyPhoneNumber;
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

[TestSubject(typeof(ResetAndVerifyPhoneNumberCommandHandler))]
public class ResetAndVerifyPhoneNumberCommandHandlerTests
{
    private readonly Mock<ILogger<ResetAndVerifyPhoneNumberCommandHandler>> _loggerMock;
    private readonly ResetAndVerifyPhoneNumberCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public ResetAndVerifyPhoneNumberCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ResetAndVerifyPhoneNumberCommandHandler>>();
        _concreteValidator = new ResetAndVerifyPhoneNumberCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new ResetAndVerifyPhoneNumberProfile()), new NullLoggerFactory())
        );
        _authRepositoryMock = new Mock<IAuthRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private Task<User> GenerateUser(
        string phoneNumber
    )
    {
        UserAuthLog userAuthLog = UserAuthLog.Create();

        userAuthLog.SetIsVerifiedStatus(true);

        User user = User.Create
        (
            "John",
            "Doe",
            "my@gmail.com",
            phoneNumber,
            "MyStrongPassword1234!",
            It.IsAny<Guid>(),
            userAuthLog.IdUserAuthLog
        );

        user.UserAuthLog = userAuthLog;

        return Task.FromResult(user);
    }

    [Fact]
    public async Task ResetAndVerifyPhoneNumber_Should_Reset_The_PhoneNumber_And_Return_Message()
    {
        // Arrange
        const string oldPhoneNumber = "+48 123 123 123";

        User user = await GenerateUser(oldPhoneNumber);

        ResetAndVerifyPhoneNumberCommand command = new ResetAndVerifyPhoneNumberCommand
            { IdUser = user.IdUser, NewPhoneNumber = "+48 345 123 123" };

        ResetAndVerifyPhoneNumberCommandHandler handler = new ResetAndVerifyPhoneNumberCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogByIdUserAsync(user.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        ResetAndVerifyPhoneNumberCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);

        user
            .PhoneNumber
            .Should()
            .NotBeEquivalentTo(oldPhoneNumber);

        result
            .Message
            .Should()
            .BeEquivalentTo("Phone reset successful.");
    }

    [Fact]
    public async Task
        ResetAndVerifyPhoneNumber_Should_Throw_Exception_When_PhoneNumber_Is_Equivalent_To_Old_PhoneNumber()
    {
        // Arrange
        const string oldPhoneNumber = "+48 123 123 123";

        User user = await GenerateUser(oldPhoneNumber);

        ResetAndVerifyPhoneNumberCommand command = new ResetAndVerifyPhoneNumberCommand
            { IdUser = user.IdUser, NewPhoneNumber = oldPhoneNumber };

        ResetAndVerifyPhoneNumberCommandHandler handler = new ResetAndVerifyPhoneNumberCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.FindUserAndUserLogByIdUserAsync(user.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("New phone number must differ from the old one.");
    }
}