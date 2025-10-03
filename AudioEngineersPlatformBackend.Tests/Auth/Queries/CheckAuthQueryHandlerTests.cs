using API.Contracts.Auth.Queries.CheckAuth;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Auth.Queries.CheckAuth;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Auth.Queries;

[TestSubject(typeof(CheckAuthQueryHandler))]
public class CheckAuthQueryHandlerTests
{
    private readonly Mock<ILogger<CheckAuthQueryHandler>> _loggerMock;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly CheckAuthQueryValidator _concreteValidator;

    public CheckAuthQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CheckAuthQueryHandler>>();
        _concreteValidator = new CheckAuthQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration(cfg => cfg.AddProfile(new CheckAuthProfile()), new NullLoggerFactory())
        );
        _authRepositoryMock = new Mock<IAuthRepository>();
    }

    [Fact]
    public async Task CheckAuth_Should_Return_Correct_User_Data()
    {
        // Arrange
        CheckAuthQuery query = new CheckAuthQuery { IdUser = Guid.Parse("A3005108-97A9-4D56-9AE3-ED3EBC266220") };

        CheckAuthQueryHandler handler = new CheckAuthQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.GetCheckAuthDataAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync
            (
                new CheckAuthDto
                {
                    IdUser = query.IdUser,
                    Email = "random@gmail.com",
                    FirstName = "John",
                    LastName = "Doe",
                    IdRole = Guid.Parse("A7E4B94E-6E3B-4CC7-BE72-FDA657BF2678"),
                    PhoneNumber = "+48 123 123 123",
                    RoleName = "Client"
                }
            );

        // Act
        CheckAuthQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .IdUser
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CheckAuth_Should_Throw_Exception_When_IdUser_Is_Invalid()
    {
        // Arrange
        CheckAuthQuery query = new CheckAuthQuery { IdUser = Guid.Parse("A3005108-97A9-4D56-9AE3-ED3EBC266220") };

        CheckAuthQueryHandler handler = new CheckAuthQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _authRepositoryMock.Object
        );

        _authRepositoryMock
            .Setup(exp => exp.GetCheckAuthDataAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CheckAuthDto)null!);

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await func
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage("User does not exist.");
    }
}