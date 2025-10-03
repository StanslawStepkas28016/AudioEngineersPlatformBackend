using AudioEngineersPlatformBackend.Domain.Exceptions;
using AudioEngineersPlatformBackend.Domain.ValueObjects;

namespace AudioEngineersPlatformBackend.Domain.Entities;

public class User
{
    // Backing fields
    private Guid _idUser;
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _phoneNumber;
    private string _password;

    // Properties
    public Guid IdUser
    {
        get => _idUser;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdUser)} cannot be empty.");
            }

            _idUser = value;
        }
    }

    public string FirstName
    {
        get => _firstName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(FirstName)} cannot be empty.");
            }

            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(LastName)} cannot be empty.");
            }

            _lastName = value;
        }
    }

    public string Email
    {
        get => _email;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || new EmailVo(value).Email != value)
            {
                throw new ArgumentException($"Invalid {nameof(Email)}.");
            }

            _email = value;
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        private set
        {
            if (new PhoneNumberVo(value).PhoneNumber != value)
            {
                throw new ArgumentException($"Invalid {nameof(PhoneNumber)}.");
            }

            _phoneNumber = value;
        }
    }

    public string Password
    {
        get => _password;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Password)} cannot be empty.");
            }

            _password = value;
        }
    }

    // References
    public Guid IdRole { get; private set; }

    public Role Role { get; private set; }

    public Guid IdUserAuthLog { get; private set; }

    public UserAuthLog UserAuthLog { get; set; }

    public ICollection<Token> Tokens { get; set; }

    public ICollection<Advert> Adverts { get; private set; }

    public ICollection<Review> Reviews { get; private set; }

    public ICollection<SocialMediaLink> SocialMediaLinks { get; private set; }

    public ICollection<UserMessage> UserMessagesSender { get; set; }

    public ICollection<UserMessage> UserMessagesRecipient { get; set; }

    public ICollection<HubConnection> HubConnections { get; set; }

    // Private constructor used for EF Core
    private User()
    {
    }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string password,
        Guid idRole,
        Guid idUserLog
    )
    {
        return new User
        {
            IdUser = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            Password = password,
            IdRole = idRole,
            IdUserAuthLog = idUserLog
        };
    }

    public static User CreateWithId(
        Guid idUser,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string password,
        Guid idRole,
        Guid idUserLog
    )
    {
        return new User
        {
            IdUser = idUser,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            Password = password,
            IdRole = idRole,
            IdUserAuthLog = idUserLog
        };
    }

    public void SetHashedPassword(
        string hashedPassword
    )
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new ArgumentException($"{nameof(Password)} cannot be empty.");
        }

        Password = hashedPassword;
    }

    public void ResetHashedPassword(
        string newHashedPassword
    )
    {
        if (string.IsNullOrWhiteSpace(newHashedPassword))
        {
            throw new ArgumentException("New password cannot be empty.");
        }

        Password = newHashedPassword;
    }

    public void ResetEmail(
        string newEmail
    )
    {
        // Check if the emails differ 
        if (Email == newEmail)
        {
            throw new BusinessRelatedException
            (
                $"New {nameof(Email)
                    .ToLower()} can't be the same as previous."
            );
        }

        Email = newEmail;
    }

    public void ResetPhoneNumber(
        string newValidPhoneNumber
    )
    {
        if (string.IsNullOrWhiteSpace(newValidPhoneNumber))
        {
            throw new ArgumentException($"New {nameof(PhoneNumber)} cannot be empty.");
        }

        // Check if the numbers differ
        if (newValidPhoneNumber == PhoneNumber)
        {
            throw new BusinessRelatedException("New phone number must differ from the old one.");
        }

        PhoneNumber = newValidPhoneNumber;
    }
}