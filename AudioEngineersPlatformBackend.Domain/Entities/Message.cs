namespace AudioEngineersPlatformBackend.Domain.Entities;

public class Message
{
    // Backing fields
    private Guid _idMessage;
    private string _textContent;
    private string? _fileName;
    private Guid _fileKey;
    private DateTime _dateSent;

    // Constants
    private const short MaxLength = 500;

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

    public Guid FileKey
    {
        get => _fileKey;
        set { _fileKey = value; }
    }

    public string? FileName
    {
        get => _fileName;
        set { _fileName = value; }
    }

    public DateTime DateSent
    {
        get => _dateSent;
        set => _dateSent = value;
    }

    public ICollection<UserMessage> UserMessages { get; set; }

    // Private constructor for EF Core
    private Message()
    {
    }

    /// <summary>
    ///     Factory method used for creating a text message.
    /// </summary>
    /// <param name="textContent"></param>
    /// <returns></returns>
    public static Message CreateTextMessage(
        string textContent)
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

    /// <summary>
    ///     Factory method to create a new Message with a specific IdMessage.
    ///     Used for seeding purposes.
    /// </summary>
    /// <param name="idMessage"></param>
    /// <param name="textContent"></param>
    /// <returns></returns>
    public static Message CreateTextMessageWithId(
        Guid idMessage, string textContent)
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

    /// <summary>
    ///     Factory method to create a new file containing Message.
    ///     It contains a key used to an AWS S3 bucket, pointing towards a specific file. 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileKey"></param>
    /// <returns></returns>
    public static Message CreateFileMessage(string fileName,
        Guid fileKey)
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

    /// <summary>
    ///     Factory method to create a new file containing Message with a specific idMessage.
    ///     It contains a key used to an AWS S3 bucket, pointing towards a specific file.
    ///     Used for seeding purposes. 
    /// </summary>
    /// <param name="idMessage"></param>
    /// <param name="fileName"></param>
    /// <param name="fileKey"></param>
    /// <returns></returns>
    public static Message CreateFileMessageWithId(
        Guid idMessage, string fileName, Guid fileKey)
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