using API.Contracts.Auth.Commands.Register;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Register;
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

[TestSubject(typeof(RegisterCommandHandler))]
public class RegisterCommandHandlerTests
{
    private readonly Mock<ILogger<RegisterCommandHandler>> _loggerMock;
    private readonly RegisterCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<ISesService> _sesServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;

    public RegisterCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<RegisterCommandHandler>>();
        _concreteValidator = new RegisterCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration(cfg => cfg.AddProfile(new RegisterProfile()), new NullLoggerFactory())
        );
        _authRepositoryMock = new Mock<IAuthRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _sesServiceMock = new Mock<ISesService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Register_Should_Create_User_And_Send_A_Welcome_Email_Message()
    {
        // Arrange
        RegisterCommandHandler handler = new RegisterCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        RegisterCommand command = new RegisterCommand
        {
            Email = "test@mail.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "+48532123123",
            Password = "MyStrongPassword1234!",
            RoleName = "Client"
        };

        _authRepositoryMock
            .Setup(exp => exp.FindUserByEmailAsNoTrackingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        _authRepositoryMock
            .Setup(exp => exp.IsPhoneNumberAlreadyTakenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _authRepositoryMock
            .Setup(exp => exp.FindRoleByNameAsNoTrackingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Role.Create("Client"));

        _passwordHasherMock
            .Setup
            (exp => exp.HashPassword(It.IsAny<User>(), It.IsAny<string>())
            )
            .Returns("SomeHashDoesNotMatterHereJustMocking");

        // Act
        RegisterCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _authRepositoryMock
            .Verify(exp => exp.AddUserLogAsync(It.IsAny<UserAuthLog>(), It.IsAny<CancellationToken>()), Times.Once);

        _authRepositoryMock
            .Verify(exp => exp.AddTokenAsync(It.IsAny<Token>(), It.IsAny<CancellationToken>()), Times.Once);

        _authRepositoryMock
            .Verify(exp => exp.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

        _sesServiceMock
            .Verify
            (
                exp => exp.SendRegisterVerificationEmailAsync
                (
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ),
                Times.Once
            );

        result
            .Email
            .Should()
            .BeEquivalentTo(command.Email);
    }

    [Fact]
    public async Task Register_Should_Throw_Exception_If_Phone_Number_Is_Already_Taken()
    {
        // Arrange
        RegisterCommandHandler handler = new RegisterCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        RegisterCommand command = new RegisterCommand
        {
            Email = "test@mail.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "+48532123123",
            Password = "MyStrongPassword1234!",
            RoleName = "Client"
        };

        _authRepositoryMock
            .Setup(exp => exp.FindUserByEmailAsNoTrackingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        _authRepositoryMock
            .Setup(exp => exp.IsPhoneNumberAlreadyTakenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage
            (
                $"Provided {nameof(User.PhoneNumber)
                    .ToLower()} is already taken."
            );
    }

    [Fact]
    public async Task Register_Should_Throw_Exception_If_Email_Is_Already_Taken()
    {
        // Arrange
        RegisterCommandHandler handler = new RegisterCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _passwordHasherMock.Object,
            _sesServiceMock.Object,
            _unitOfWorkMock.Object
        );

        RegisterCommand command = new RegisterCommand
        {
            Email = "test@mail.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "+48532123123",
            Password = "MyStrongPassword1234!",
            RoleName = "Client"
        };

        _authRepositoryMock
            .Setup(exp => exp.FindUserByEmailAsNoTrackingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync
            (
                User.Create
                (
                    command.FirstName,
                    command.LastName,
                    command.Email,
                    command.PhoneNumber,
                    command.Password,
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()
                )
            );

        _authRepositoryMock
            .Setup(exp => exp.IsPhoneNumberAlreadyTakenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _authRepositoryMock
            .Setup(exp => exp.FindRoleByNameAsNoTrackingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Role.Create("Client"));

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage
            (
                $"Provided {nameof(User.Email)
                    .ToLower()} is already taken."
            );
    }
}