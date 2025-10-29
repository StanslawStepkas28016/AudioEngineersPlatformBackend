using System.Text;
using API.Contracts.Advert.Commands.CreateAdvert;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.CreateAdvert;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Advert.Commands;

[TestSubject(typeof(CreateAdvertCommandHandler))]
public class CreateAdvertCommandHandlerTests
{
    private readonly Mock<ILogger<CreateAdvertCommandHandler>> _loggerMock;
    private readonly CreateAdvertCommandValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IAdvertRepository> _advertRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IS3Service> _s3ServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateAdvertCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateAdvertCommandHandler>>();
        _concreteValidator = new CreateAdvertCommandValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
                (exp => exp.AddProfile(new CreateAdvertProfile()), new NullLoggerFactory())
        );
        _advertRepositoryMock = new Mock<IAdvertRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _s3ServiceMock = new Mock<IS3Service>();
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

    private Task<AdvertCategory> GenerateAdvertCategory()
    {
        return Task.FromResult(AdvertCategory.Create("Mixing"));
    }

    private Task<FormFile> GenerateTestFormFile(
        string fileName,
        string content
    )
    {
        byte[] bytes = Encoding.UTF8.GetBytes(content);

        return Task.FromResult
        (
            new FormFile
            (
                new MemoryStream(bytes),
                0,
                bytes.Length,
                "Data",
                fileName
            )
        );
    }

    [Fact]
    public async Task CreateAdvert_Should_Create_Advert_And_Return_Its_New_Id()
    {
        // Arrange
        User user = await GenerateUser();

        IFormFile coverImageFormFile = await GenerateTestFormFile("image.jpg", "SomeBytesContent");

        CreateAdvertCommand command = new CreateAdvertCommand
        {
            IdUser = user.IdUser,
            Title = "My mixing!",
            Description = "Some description, irrelevant.",
            CoverImageFile = coverImageFormFile,
            PortfolioUrl = "https://instagram.com/drake",
            CategoryName = "Mixing", Price = 299.99
        };

        CreateAdvertCommandHandler handler = new CreateAdvertCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _s3ServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup
                (exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup
                (exp => exp.DoesUserHaveAnyAdvertByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        AdvertCategory category = await GenerateAdvertCategory();

        _advertRepositoryMock
            .Setup
                (exp => exp.FindAdvertCategoryByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _s3ServiceMock
            .Setup(exp => exp.UploadFileAsync(It.IsAny<string>(), coverImageFormFile, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid());

        // Act
        CreateAdvertCommandResult result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _s3ServiceMock
            .Verify
            (
                exp => exp.UploadFileAsync(It.IsAny<string>(), It.IsAny<FormFile>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

        _advertRepositoryMock.Verify
            (exp => exp.AddAdvertLogAsync(It.IsAny<AdvertLog>(), It.IsAny<CancellationToken>()), Times.Once);

        _advertRepositoryMock.Verify
            (exp => exp.AddAdvertAsync(It.IsAny<Domain.Entities.Advert>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock
            .Verify(exp => exp.CompleteAsync(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task CreateAdvert_Should_Throw_Exception_When_User_Already_Has_An_Advert()
    {
        // Arrange
        User user = await GenerateUser();

        IFormFile coverImageFormFile = await GenerateTestFormFile("image.jpg", "SomeBytesContent");

        CreateAdvertCommand command = new CreateAdvertCommand
        {
            IdUser = user.IdUser,
            Title = "My mixing!",
            Description = "Some description, irrelevant.",
            CoverImageFile = coverImageFormFile,
            PortfolioUrl = "https://instagram.com/drake",
            CategoryName = "Mixing", Price = 299.99
        };

        CreateAdvertCommandHandler handler = new CreateAdvertCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _s3ServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup
                (exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup
                (exp => exp.DoesUserHaveAnyAdvertByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("You already have an advert posted.");
    }

    [Fact]
    public async Task CreateAdvert_Should_Throw_Exception_If_Category_Does_Not_Exist()
    {
        // Arrange
        User user = await GenerateUser();

        IFormFile coverImageFormFile = await GenerateTestFormFile("image.jpg", "SomeBytesContent");

        CreateAdvertCommand command = new CreateAdvertCommand
        {
            IdUser = user.IdUser,
            Title = "My mixing!",
            Description = "Some description, irrelevant.",
            CoverImageFile = coverImageFormFile,
            PortfolioUrl = "https://instagram.com/drake",
            CategoryName = "Mixing", Price = 299.99
        };

        CreateAdvertCommandHandler handler = new CreateAdvertCommandHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _advertRepositoryMock.Object,
            _userRepositoryMock.Object,
            _s3ServiceMock.Object,
            _unitOfWorkMock.Object
        );

        _userRepositoryMock
            .Setup
                (exp => exp.DoesUserExistByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _advertRepositoryMock
            .Setup
                (exp => exp.DoesUserHaveAnyAdvertByIdUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _advertRepositoryMock
            .Setup
                (exp => exp.FindAdvertCategoryByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AdvertCategory)null!);

        _s3ServiceMock
            .Setup(exp => exp.UploadFileAsync(It.IsAny<string>(), coverImageFormFile, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid());

        // Act
        Func<Task> func = async () => await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<BusinessRelatedException>()
                .WithMessage("Category does not exist.");
    }
}