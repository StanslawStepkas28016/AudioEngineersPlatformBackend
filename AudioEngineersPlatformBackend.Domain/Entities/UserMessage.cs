namespace AudioEngineersPlatformBackend.Domain.Entities;

public class UserMessage
{
    // Backing fields
    private Guid _idUserMessage;
    private Guid _idUserSender;
    private User _userSender;
    private Guid _idUserRecipient;
    private User _userRecipient;
    private Guid _idMessage;
    private Message _message;

    // Properties
    public Guid IdUserMessage
    {
        get => _idUserMessage;
        set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdUserMessage)} cannot be empty.");
            }

            _idUserMessage = value;
        }
    }

    public Guid IdUserSender
    {
        get => _idUserSender;
        set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdUserSender)} cannot be empty.");
            }

            _idUserSender = value;
        }
    }

    public bool IsRead { get; set; }

    public User UserSender
    {
        get => _userSender;
        set => _userSender = value;
    }

    public Guid IdUserRecipient
    {
        get => _idUserRecipient;
        set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdUserRecipient)} cannot be empty.");
            }

            _idUserRecipient = value;
        }
    }

    public User UserRecipient
    {
        get => _userRecipient;
        set => _userRecipient = value;
    }

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

    public Message Message
    {
        get => _message;
        set => _message = value;
    }

    // Constructor for EF Core
    private UserMessage()
    {
    }

    /// <summary>
    ///     Factory method to create a UserMessage.
    /// </summary>
    /// <param name="idUserSender"></param>
    /// <param name="idUserRecipient"></param>
    /// <param name="idMessage"></param>
    /// <returns></returns>
    public static UserMessage Create(Guid idUserSender, Guid idUserRecipient, Guid idMessage)
    {
        return new UserMessage
        {
            IdUserMessage = Guid.NewGuid(),
            IdUserSender = idUserSender,
            IdUserRecipient = idUserRecipient,
            IsRead = false,
            IdMessage = idMessage
        };
    }

    /// <summary>
    ///     Factory method to create a UserMessage entity with a specific idUserMessage,
    ///     used for seeding purposes.
    /// </summary>
    /// <param name="idUserMessage"></param>
    /// <param name="idUserSender"></param>
    /// <param name="idUserRecipient"></param>
    /// <param name="idMessage"></param>
    /// <returns></returns>
    public static UserMessage CreateWithId(Guid idUserMessage, Guid idUserSender, Guid idUserRecipient, Guid idMessage)
    {
        return new UserMessage
        {
            IdUserMessage = idUserMessage,
            IdUserSender = idUserSender,
            IdUserRecipient = idUserRecipient,
            IsRead = false,
            IdMessage = idMessage
        };
    }
}