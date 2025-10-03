using API.Contracts.Advert.Commands.DeleteAdvert;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.DeleteAdvert;
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

[TestSubject(typeof(DeleteAdvertCommandHandler))]
public class DeleteAdvertCommandHandlerTests
{
    private readonly Mock<ILogger<DeleteAdvertCommandHandler>> _loggerMock;
    private readonly DeleteAdvertCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAdvertRepository> _advertRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public DeleteAdvertCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteAdvertCommandHandler>>();
        _concreteValidator = new DeleteAdvertCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new DeleteAdvertProfile()), new NullLoggerFactory())
        );
        _advertRepositoryMock = new Mock<IAdvertRepository>();
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
    public async Task DeleteAdvert_Should_Set_IsDeleted_Status_And_Return_A_Success_Message()
    {
        // Arrange
        User user = await GenerateUser();

        Domain.Entities.Advert advert = await GenerateAdvert(user.IdUser);

        DeleteAdvertCommand command = new DeleteAdvertCommand { IdUser = user.IdUser, IdAdvert = advert.IdAdvert };

        DeleteAdvertCommandHandler handler = new DeleteAdvertCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _advertRepositoryMock
            .Setup
            (exp => exp.FindAdvertAndAdvertLogByIdUserAndIdAdvertAsync
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(advert);

        // Act
        DeleteAdvertCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);

        advert
            .AdvertLog
            .IsDeleted
            .Should()
            .BeTrue();

        result
            .Message
            .Should()
            .BeEquivalentTo("Advert successfully deleted.");
    }

    [Fact]
    public async Task DeleteAdvert_Should_Throw_Exception_When_Advert_Does_Not_Exist()
    {
        // Arrange
        User user = await GenerateUser();

        Domain.Entities.Advert advert = await GenerateAdvert(user.IdUser);

        DeleteAdvertCommand command = new DeleteAdvertCommand { IdUser = user.IdUser, IdAdvert = advert.IdAdvert };

        DeleteAdvertCommandHandler handler = new DeleteAdvertCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
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
                .WithMessage("Advert not found.");
    }
}