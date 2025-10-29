using API.Contracts.Advert.Commands.ChangeAdvertData;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.ChangeAdvertData;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Advert.Commands;

[TestSubject(typeof(ChangeAdvertDataCommandHandler))]
public class ChangeAdvertDataCommandHandlerTests
{
    private readonly Mock<ILogger<ChangeAdvertDataCommandHandler>> _loggerMock;
    private readonly ChangeAdvertDataCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAdvertRepository> _advertRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public ChangeAdvertDataCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ChangeAdvertDataCommandHandler>>();
        _concreteValidator = new ChangeAdvertDataCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new ChangeAdvertDataProfile()), new NullLoggerFactory())
        );
        _advertRepositoryMock = new Mock<IAdvertRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private Task<User> GenerateUser()
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

        return Task.FromResult(user);
    }

    private Task<Domain.Entities.Advert> GenerateAdvert(
        Guid idUser
    )
    {
        AdvertCategory advertCategory = AdvertCategory.Create("Mixing");

        AdvertLog advertLog = AdvertLog.Create();

        Domain.Entities.Advert advert = Domain.Entities.Advert.Create
        (
            "Buy my service!",
            "Here is a short description...",
            Guid.Parse("7332440A-86A9-4E1E-93AE-73B7E4E1C186"),
            "https://instagram.com/drake",
            299.99,
            idUser,
            advertCategory.IdAdvertCategory,
            advertLog.IdAdvertLog
        );

        advert.AdvertLog = advertLog;

        return Task.FromResult(advert);
    }

    [Fact]
    public async Task ChangeAdvertData_Should_Change_The_Data_And_Return_Ids_Of_Resources()
    {
        // Arrange
        User user = await GenerateUser();

        Domain.Entities.Advert advert = await GenerateAdvert(user.IdUser);

        ChangeAdvertDataCommand command = new ChangeAdvertDataCommand
        {
            IdUser = user.IdUser,
            IdAdvert = advert.IdAdvert,
            Title = "New title!",
            Description = "New description...",
            PortfolioUrl = "", // Not updating this field.
            Price = 299.99
        };

        ChangeAdvertDataCommandHandler handler = new ChangeAdvertDataCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _advertRepositoryMock
            .Setup
            (exp => exp.FindAdvertAndAdvertLogByIdUserAndIdAdvertAsync
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(advert);

        // Act
        ChangeAdvertDataCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);

        advert
            .Description
            .Should()
            .BeEquivalentTo(command.Description);

        advert
            .PortfolioUrl
            .Should()
            .NotBeEquivalentTo(command.PortfolioUrl);

        advert
            .Price
            .Should()
            .BeGreaterThanOrEqualTo(command.Price);

        result
            .IdUser
            .Should()
            .NotBeEmpty();

        result
            .IdAdvert
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task ChangeAdvertData_Should_Throw_Exception_When_Advert_Does_Not_Exist()
    {
        // Arrange
        User user = await GenerateUser();

        Domain.Entities.Advert advert = await GenerateAdvert(user.IdUser);

        ChangeAdvertDataCommand command = new ChangeAdvertDataCommand
        {
            IdUser = user.IdUser,
            IdAdvert = advert.IdAdvert,
            Title = "New title!",
            Description = "New description...",
            PortfolioUrl = "", // Not updating this field.
            Price = 299.99
        };

        ChangeAdvertDataCommandHandler handler = new ChangeAdvertDataCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _advertRepositoryMock
            .Setup
            (exp => exp.FindAdvertAndAdvertLogByIdUserAndIdAdvertAsync
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((Domain.Entities.Advert)null!);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("No advert found.");
    }

    [Fact]
    public async Task ChangeAdvertData_Should_Throw_Exception_When_Advert_Is_Inactive_Or_Is_Deleted()
    {
        // Arrange
        User user = await GenerateUser();

        Domain.Entities.Advert advert = await GenerateAdvert(user.IdUser);

        advert.AdvertLog.SetIsActiveStatus(false);

        ChangeAdvertDataCommand command = new ChangeAdvertDataCommand
        {
            IdUser = user.IdUser,
            IdAdvert = advert.IdAdvert,
            Title = "New title!",
            Description = "New description...",
            PortfolioUrl = "", // Not updating this field.
            Price = 299.99
        };

        ChangeAdvertDataCommandHandler handler = new ChangeAdvertDataCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _advertRepositoryMock
            .Setup
            (exp => exp.FindAdvertAndAdvertLogByIdUserAndIdAdvertAsync
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(advert);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("Advert not available.");
    }
}