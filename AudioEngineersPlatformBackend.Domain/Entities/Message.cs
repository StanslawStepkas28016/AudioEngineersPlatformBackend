namespace AudioEngineersPlatformBackend.Domain.Entities;

public class Message
{
    // Backing fields
    private Guid _idMessage;
    private string _textContent;

    // Constants
    public const short MaxLength = 1000;

    // Properties
    public Guid IdMessage
    {
        get => _idMessage;
        set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdMessage)} cannot be empty.");
            }

            _idMessage = value;
        }
    }

    public string? TextContent
    {
        get => _textContent;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(TextContent)} cannot be empty.");
            }

            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"{nameof(TextContent)} cannot be longer than {MaxLength} characters.");
            }

            _textContent = value;
        }
    }

    public Guid FileKey { get; set; }

    public string? FileName { get; set; }

    public DateTime DateSent { get; set; }

    public ICollection<UserMessage> UserMessages { get; set; }

    // Private constructor for EF Core
    private Message()
    {
    }

    public static Message CreateTextMessage(
        string textContent
    )
    {
        return new Message
        {
            IdMessage = Guid.NewGuid(),
            TextContent = textContent,
            FileName = null,
            FileKey = Guid.Empty,
            DateSent = DateTime.UtcNow
        };
    }

    public static Message CreateTextMessageWithId(
        Guid idMessage,
        string textContent
    )
    {
        return new Message
        {
            IdMessage = idMessage,
            TextContent = textContent,
            FileName = null,
            FileKey = Guid.Empty,
            DateSent = DateTime.UtcNow
        };
    }

    public static Message CreateFileMessage(
        string fileName,
        Guid fileKey
    )
    {
        return new Message
        {
            IdMessage = Guid.NewGuid(),
            _textContent = null, // Using this to omit the property setter not allowing null values.
            FileName = fileName,
            FileKey = fileKey,
            DateSent = DateTime.UtcNow
        };
    }

    public static Message CreateFileMessageWithId(
        Guid idMessage,
        string fileName,
        Guid fileKey
    )
    {
        return new Message
        {
            IdMessage = idMessage,
            _textContent = null, // Using this to omit the property setter not allowing null values.
            FileName = fileName,
            FileKey = fileKey,
            DateSent = DateTime.UtcNow
        };
    }
}