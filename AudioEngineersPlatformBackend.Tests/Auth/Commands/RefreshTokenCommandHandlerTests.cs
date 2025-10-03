using API.Contracts.Auth.Commands.RefreshToken;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.RefreshToken;
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

[TestSubject(typeof(RefreshTokenCommandHandler))]
public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<ILogger<RefreshTokenCommandHandler>> _loggerMock;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RefreshTokenCommandValidator _concreteValidator;

    public RefreshTokenCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<RefreshTokenCommandHandler>>();
        _concreteValidator = new RefreshTokenCommandValidator();
        _concreteMapper = new Mapper
            (new MapperConfiguration(exp => exp.AddProfile(new RefreshTokenProfile()), new NullLoggerFactory()));
        _authRepositoryMock = new Mock<IAuthRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private Task<User> GenerateUser(
        DateTime tokenExpirationDate
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

        user.UserAuthLog = userAuthLog;

        Token token = Token.Create
        (
            nameof(TokenNames.RefreshToken),
            "1F3B5AFC-B7B8-4B48-B40B-058DB4791343",
            tokenExpirationDate,
            user.IdUser
        );

        user.Tokens = new List<Token>();
        user.Tokens.Add(token);

        return Task.FromResult(user);
    }

    [Fact]
    public async Task RefreshToken_Should_Return_A_User_With_A_New_Valid_Access_And_Refresh_Token()
    {
        // Arrange
        RefreshTokenCommand command = new RefreshTokenCommand
            { RefreshToken = Guid.Parse("1F3B5AFC-B7B8-4B48-B40B-058DB4791343") };

        RefreshTokenCommandHandler handler = new RefreshTokenCommandHandler
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
                (command.RefreshToken.ToString(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(await GenerateUser(DateTime.Now.AddHours(24)));

        // Act
        RefreshTokenCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        result
            .User
            .Tokens
            .First()
            .Value
            .Should()
            .NotBeEmpty()
            .And
            .NotBe(command.RefreshToken.ToString());
    }

    [Fact]
    public async Task RefreshToken_Should_Throw_Exception_When_Refresh_Token_Is_Invalid()
    {
        // Arrange
        RefreshTokenCommand command = new RefreshTokenCommand
            { RefreshToken = Guid.Parse("1F3B5AFC-B7B8-4B48-B40B-058DB4791343") };

        RefreshTokenCommandHandler handler = new RefreshTokenCommandHandler
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
                (command.RefreshToken.ToString(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((User?)null!);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<BusinessRelatedException>()
            .WithMessage("User does not exist.");
    }

    [Fact]
    public async Task RefreshToken_Should_Throw_Exception_When_Refresh_Token_Is_Expired()
    {
        // Arrange
        RefreshTokenCommand command = new RefreshTokenCommand
            { RefreshToken = Guid.Parse("1F3B5AFC-B7B8-4B48-B40B-058DB4791343") };

        RefreshTokenCommandHandler handler = new RefreshTokenCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        User user = await GenerateUser(DateTime.UtcNow.Subtract(TimeSpan.FromHours(2)));

        _authRepositoryMock
            .Setup
            (exp => exp.FindUserAndUserLogAndTokenByTokenAsync
                (command.RefreshToken.ToString(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(user);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowExactlyAsync<UnauthorizedAccessException>()
            .WithMessage($"{nameof(TokenNames.RefreshToken)} expired, please login again.");
    }
}