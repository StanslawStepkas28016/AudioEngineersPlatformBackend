using API.Contracts.Chat.Commands.GetPreSignedUrlForUpload;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetPresignedUrlForUpload;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Chat.Queries;

[TestSubject(typeof(GetPresignedUrlForUploadQueryHandler))]
public class GetPresignedUrlForUploadQueryHandlerTests
{
    private readonly Mock<ILogger<GetPresignedUrlForUploadQueryHandler>> _loggerMock;
    private readonly GetPresignedUrlForUploadQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IS3Service> _s3ServiceMock;

    public GetPresignedUrlForUploadQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetPresignedUrlForUploadQueryHandler>>();
        _concreteValidator = new GetPresignedUrlForUploadQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
            (
                exp => exp.AddProfile(new GetPreSignedUrlForUploadProfile()),
                new NullLoggerFactory()
            )
        );
        _s3ServiceMock = new Mock<IS3Service>();
    }

    [Fact]
    public async Task GetPresignedUrlForUpload_Should_Return_Data()
    {
        // Arrange
        GetPresignedUrlForUploadQuery query = new GetPresignedUrlForUploadQuery
            { Folder = "files", FileName = "audio.wav" };

        GetPresignedUrlForUploadQueryHandler handler = new GetPresignedUrlForUploadQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _s3ServiceMock.Object
        );

        _s3ServiceMock
            .Setup
            (exp => exp.GetPreSignedUrlForUploadAsync
                (
                    It.Is<string>(v => v == query.Folder),
                    It.IsAny<Guid>(),
                    It.Is<string>(v => v == query.FileName),
                    It.IsAny<CancellationToken>()
                )
            );

        // Act
        GetPresignedUrlForUploadQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        result
            .FileKey
            .Should()
            .NotBeEmpty();

        result
            .PreSignedUrlForUpload
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task GetPresignedUrlForUpload_Should_Throw_Exception_For_Invalid_Input_Data()
    {
        // Arrange
        GetPresignedUrlForUploadQuery query = new GetPresignedUrlForUploadQuery
            { Folder = "files", FileName = "" };

        GetPresignedUrlForUploadQueryHandler handler = new GetPresignedUrlForUploadQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _s3ServiceMock.Object
        );

        // Act
        Func<Task> func = async () => await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await
            func
                .Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithMessage("FileName must be provided.");
    }
}