using API.Contracts.Advert.Commands.AddReview;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.AddReview;
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

[TestSubject(typeof(AddReviewCommandHandler))]
public class AddReviewCommandHandlerTests
{
    private readonly Mock<ILogger<AddReviewCommandHandler>> _loggerMock;
    private readonly AddReviewCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAdvertRepository> _advertRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public AddReviewCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<AddReviewCommandHandler>>();
        _concreteValidator = new AddReviewCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new AddReviewProfile()), new NullLoggerFactory())
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
    public async Task AddReview_Should_Add_Review_And_Return_Success_Message()
    {
        // Arrange
        User userPoster = await GenerateUser();

        Domain.Entities.Advert advert = await GenerateAdvert(userPoster.IdUser);

        User userReviewer = await GenerateUser();

        AddReviewCommand command = new AddReviewCommand
        {
            IdUserReviewer = userReviewer.IdUser,
            IdAdvert = advert.IdAdvert,
            Content = "My review is good! Some other content, irrelevant, needs to be long enough.",
            SatisfactionLevel = 2
        };

        AddReviewCommandHandler handler = new AddReviewCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup(exp => exp.DoesAdvertExistByIdAdvertAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup
            (exp => exp.DoesUserHaveAReviewPostedAlreadyByIdUserReviewerAndIdAdvert
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(false);

        // Act
        AddReviewCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _advertRepositoryMock
            .Verify(exp => exp.AddReviewLogAsync(It.IsAny<ReviewLog>(), It.IsAny<CancellationToken>()), Times.Once);

        _advertRepositoryMock
            .Verify(exp => exp.AddReviewAsync(It.IsAny<Review>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);

        result
            .Message
            .Should()
            .BeEquivalentTo("Review created successfully.");
    }

    [Fact]
    public async Task AddReview_Should_Throw_Exception_If_Advert_With_Specific_Id_Does_Not_Exist()
    {
        // Arrange
        User userReviewer = await GenerateUser();

        AddReviewCommand command = new AddReviewCommand
        {
            IdUserReviewer = userReviewer.IdUser,
            IdAdvert = Guid.Parse("9FB2F1B2-8B68-4079-A22D-DA4291E9AAA8"), // IdAdvert of non-existent advert.
            Content = "My review is good! Some other content, irrelevant, needs to be long enough.",
            SatisfactionLevel = 2
        };

        AddReviewCommandHandler handler = new AddReviewCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _advertRepositoryMock
            .Setup(exp => exp.DoesAdvertExistByIdAdvertAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup
            (exp => exp.DoesUserHaveAReviewPostedAlreadyByIdUserReviewerAndIdAdvert
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(false);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("User not found.");
    }

    [Fact]
    public async Task AddReview_Should_Throw_Exception_If_User_With_Specific_Id_Does_Not_Exist()
    {
        // Arrange
        User userPoster = await GenerateUser();

        Domain.Entities.Advert advert = await GenerateAdvert(userPoster.IdUser);

        AddReviewCommand command = new AddReviewCommand
        {
            IdUserReviewer = Guid.Parse("B446AA5D-0809-458F-BF5E-E7F997F506A3"), // IdUser for non-existent user.
            IdAdvert = advert.IdAdvert,
            Content = "My review is good! Some other content, irrelevant, needs to be long enough.",
            SatisfactionLevel = 2
        };

        AddReviewCommandHandler handler = new AddReviewCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _advertRepositoryMock
            .Setup(exp => exp.DoesAdvertExistByIdAdvertAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup
            (exp => exp.DoesUserHaveAReviewPostedAlreadyByIdUserReviewerAndIdAdvert
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(false);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("User not found.");
    }

    [Fact]
    public async Task AddReview_Should_Throw_Exception_If_User_Has_Already_Posted_Review_Under_Advert_Before()
    {
        // Arrange
        User userPoster = await GenerateUser();

        Domain.Entities.Advert advert = await GenerateAdvert(userPoster.IdUser);

        User userReviewer = await GenerateUser();

        AddReviewCommand command = new AddReviewCommand
        {
            IdUserReviewer = userReviewer.IdUser,
            IdAdvert = advert.IdAdvert,
            Content = "My review is good! Some other content, irrelevant, needs to be long enough.",
            SatisfactionLevel = 2
        };

        AddReviewCommandHandler handler = new AddReviewCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup(exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup(exp => exp.DoesAdvertExistByIdAdvertAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup
            (exp => exp.DoesUserHaveAReviewPostedAlreadyByIdUserReviewerAndIdAdvert
                (It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("You have already posted a review.");
    }
}