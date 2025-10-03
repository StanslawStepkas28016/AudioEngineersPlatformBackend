using API.Contracts.Chat.Queries.GetInteractedUsers;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetInteractedUsers;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Chat.Queries;

[TestSubject(typeof(GetInteractedUsersQueryHandler))]
public class GetInteractedUsersQueryHandlerTests
{
    private readonly Mock<ILogger<GetInteractedUsersQueryHandler>> _loggerMock;
    private readonly GetInteractedUsersQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChatRepository> _chatRepositoryMock;

    public GetInteractedUsersQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetInteractedUsersQueryHandler>>();
        _concreteValidator = new GetInteractedUsersQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
            (
                exp => exp.AddProfile(new GetInteractedUsersProfile()),
                new NullLoggerFactory()
            )
        );
        _userRepositoryMock = new Mock<IUserRepository>();
        _chatRepositoryMock = new Mock<IChatRepository>();
    }

    private Task<List<InteractedUserDto>> GenerateMessages()
    {
        InteractedUserDto i1 = new InteractedUserDto
        {
            IdUser = Guid.Parse("F64D2385-774F-4C51-ACFF-B54F7F966A07"),
            FirstName = "Joanna",
            LastName = "Doe",
            UnreadCount = 3
        };

        InteractedUserDto i2 = new InteractedUserDto
        {
            IdUser = Guid.Parse("B6D2BDDD-1790-43A3-B0D7-7FACF3FBBC74"),
            FirstName = "Joanna",
            LastName = "Doe",
            UnreadCount = 0
        };

        return Task.FromResult(new List<InteractedUserDto> { i1, i2 });
    }

    [Fact]
    public async Task GetInteractedUsers_Should_Return_Non_Empty_Data_When_Data_Is_Present()
    {
        // Arrange
        GetInteractedUsersQuery query = new GetInteractedUsersQuery
            { IdUser = Guid.Parse("B1EC767C-6FA4-4E98-AD17-F08E40C19922") };

        GetInteractedUsersQueryHandler handler = new GetInteractedUsersQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _chatRepositoryMock
            .Setup(exp => exp.FindInteractedUsersAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(await GenerateMessages());

        // Act
        GetInteractedUsersQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .InteractedUsersList
            .Should()
            .NotBeEmpty();
    }
    
    [Fact]
    public async Task GetInteractedUsers_Should_Return_Empty_List_If_There_Is_No_Data_Present()
    {
        // Arrange
        GetInteractedUsersQuery query = new GetInteractedUsersQuery
            { IdUser = Guid.Parse("B1EC767C-6FA4-4E98-AD17-F08E40C19922") };

        GetInteractedUsersQueryHandler handler = new GetInteractedUsersQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _chatRepositoryMock
            .Setup(exp => exp.FindInteractedUsersAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<InteractedUserDto>());

        // Act
        GetInteractedUsersQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .InteractedUsersList
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task GetInteractedUsers_Should_Throw_Exception_When_User_Does_Not_Exist()
    {
        // Arrange
        GetInteractedUsersQuery query = new GetInteractedUsersQuery
            { IdUser = Guid.Parse("B1EC767C-6FA4-4E98-AD17-F08E40C19922") };

        GetInteractedUsersQueryHandler handler = new GetInteractedUsersQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(query.IdUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("User not found.");
    }
}