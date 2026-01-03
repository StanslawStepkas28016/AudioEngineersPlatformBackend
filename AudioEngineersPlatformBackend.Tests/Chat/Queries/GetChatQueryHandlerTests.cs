using API.Contracts.Chat.Queries.GetChat;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetChat;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AudioEngineersPlatformBackend.Tests.Chat.Queries;

[TestSubject(typeof(GetChatQueryHandler))]
public class GetChatQueryHandlerTests
{
    private readonly Mock<ILogger<GetChatQueryHandler>> _loggerMock;
    private readonly GetChatQueryValidator _concreteValidator;
    private readonly Mapper _concreteMapper;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<IS3Service> _s3Service;

    public GetChatQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetChatQueryHandler>>();
        _concreteValidator = new GetChatQueryValidator();
        _concreteMapper = new Mapper
        (
            new MapperConfiguration
            (
                exp => exp.AddProfile(new GetChatProfile()),
                new NullLoggerFactory()
            )
        );
        _userRepositoryMock = new Mock<IUserRepository>();
        _chatRepositoryMock = new Mock<IChatRepository>();
        _s3Service = new Mock<IS3Service>();
    }

    private Task<List<ChatMessageDto>> GeneratePagedChatMessagesDto()
    {
        Guid idUser1 = Guid.Parse("3E8F7A02-CE39-4AD1-94BC-9DD6C47FD487");
        Guid idUser2 = Guid.Parse("5D95D2B6-9FEE-45B7-964D-C25CBEDD1981");

        ChatMessageDto message1 = new ChatMessageDto
        {
            IdMessage = Guid.NewGuid(),
            IdUserSender = idUser2,
            IsRead = false,
            TextContent = "This is a text message.",
            FileKey = Guid.Empty,
            FileName = "",
            FileUrl = "",
            DateSent = DateTime.UtcNow
        };

        ChatMessageDto message2 = new ChatMessageDto
        {
            IdMessage = Guid.NewGuid(),
            IdUserSender = idUser1,
            IsRead = false,
            TextContent = "",
            FileKey = Guid.NewGuid(),
            FileName = "stems.zip",
            FileUrl = "",
            DateSent = DateTime.UtcNow
        };

        return Task.FromResult(new List<ChatMessageDto> { message1, message2 });
    }

    [Fact]
    public async Task GetChat_Should_Return_Correct_Chat_Data()
    {
        // Arrange
        GetChatQuery query = new GetChatQuery
        {
            IdUserSender = Guid.Parse("3E8F7A02-CE39-4AD1-94BC-9DD6C47FD487"),
            IdUserRecipient = Guid.Parse("5D95D2B6-9FEE-45B7-964D-C25CBEDD1981"),
            Page = 1,
            PageSize = 3
        };

        GetChatQueryHandler handler = new GetChatQueryHandler
        (
            _loggerMock.Object,
            _concreteValidator,
            _concreteMapper,
            _userRepositoryMock.Object,
            _chatRepositoryMock.Object,
            _s3Service.Object
        );

        _userRepositoryMock
            .Setup
            (exp => exp.DoesUserExistByIdUserAsync
                (It.Is<Guid>(v => v == query.IdUserSender), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup
            (exp => exp.DoesUserExistByIdUserAsync
                (It.Is<Guid>(v => v == query.IdUserRecipient), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        List<ChatMessageDto> generatedMessagesList = await GeneratePagedChatMessagesDto();

        _chatRepositoryMock
            .Setup
            (exp => exp.FindChatAsync
                (
                    query.IdUserSender,
                    query.IdUserRecipient,
                    query.Page,
                    query.PageSize,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync
            (
                new PagedListDto<ChatMessageDto>
                    (generatedMessagesList, query.Page, query.PageSize, generatedMessagesList.Count)
            );

        _s3Service
            .Setup
            (exp => exp.GetPreSignedUrlForReadAsync
                ("some-folder", "some-file", It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync("https://aws.s3.com/some-url");

        // Act
        GetChatQueryResult result = await handler.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        _chatRepositoryMock
            .Verify
            (
                exp => exp.ExecuteMarkUserMessagesAsReadAsync
                    (query.IdUserSender, query.IdUserRecipient, It.IsAny<CancellationToken>()),
                Times.Once
            );

        List<ChatMessageDto> chatMessageDtos = result
            .PagedChatMessages
            .Items;

        chatMessageDtos
            .Count
            .Should()
            .BeGreaterThanOrEqualTo(generatedMessagesList.Count);

        chatMessageDtos[1]
            .IdMessage
            .Should()
            .NotBeEmpty();

        chatMessageDtos[1]
            .IdUserSender
            .ToString()
            .Should()
            .BeEquivalentTo(query.IdUserRecipient.ToString());

        chatMessageDtos[1]
            .TextContent
            .Should()
            .BeEquivalentTo("This is a text message.");

        chatMessageDtos[1]
            .FileKey
            .Should()
            .BeEmpty();

        chatMessageDtos[1]
            .FileName
            .Should()
            .BeEmpty();

        chatMessageDtos[1]
            .FileUrl
            .Should()
            .BeEmpty();

        chatMessageDtos[1]
            .DateSent
            .Should()
            .NotBeAfter(DateTime.UtcNow);

        chatMessageDtos[0]
            .IdMessage
            .Should()
            .NotBeEmpty();

        chatMessageDtos[0]
            .IdUserSender
            .ToString()
            .Should()
            .BeEquivalentTo(query.IdUserSender.ToString());

        chatMessageDtos[0]
            .TextContent
            .Should()
            .BeEmpty();

        chatMessageDtos[0]
            .FileKey
            .Should()
            .NotBeEmpty();

        chatMessageDtos[0]
            .FileName
            .Should()
            .NotBeEmpty();

        chatMessageDtos[0]
            .FileUrl
            .Should()
            .NotBeEmpty();

        chatMessageDtos[0]
            .DateSent
            .Should()
            .NotBeAfter(DateTime.UtcNow);
    }
}