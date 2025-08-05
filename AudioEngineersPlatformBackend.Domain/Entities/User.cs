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
    private Guid _idRole;
    private Role _role;
    private Guid _idUserLog;
    private UserLog _userLog;
    private ICollection<Advert> _adverts;
    private ICollection<Review> _reviews;
    private ICollection<SocialMediaLink> _socialMediaLinks;

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
    public Guid IdRole
    {
        get => _idRole;
        private set => _idRole = value;
    }

    public virtual Role Role
    {
        get => _role;
        private set => _role = value;
    }

    public Guid IdUserLog
    {
        get => _idUserLog;
        private set => _idUserLog = value;
    }

    public virtual UserLog UserLog
    {
        get => _userLog;
        private set => _userLog = value;
    }

    public ICollection<Advert> Adverts
    {
        get => _adverts;
        private set => _adverts = value;
    }

    public ICollection<Review> Reviews
    {
        get => _reviews;
        private set => _reviews = value;
    }

    public ICollection<SocialMediaLink> SocialMediaLinks
    {
        get => _socialMediaLinks;
        private set => _socialMediaLinks = value;
    }

    // Private constructor used for EF Core
    private User()
    {
    }

    /// <summary>
    ///     Factory method to create a new User.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="password"></param>
    /// <param name="idRole"></param>
    /// <param name="idUserLog"></param>
    /// <returns></returns>
    public static User Create(string firstName, string lastName, string email, string phoneNumber,
        string password, Guid idRole, Guid idUserLog)
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
            IdUserLog = idUserLog,
        };
    }

    /// <summary>
    ///     Factory method to create a new User with a specific IdUser.
    ///     Used for seeding purposes.
    /// </summary>
    /// <param name="idUser"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="password"></param>
    /// <param name="idRole"></param>
    /// <param name="idUserLog"></param>
    /// <returns></returns>
    public static User CreateWithId(Guid idUser, string firstName, string lastName, string email, string phoneNumber,
        string password, Guid idRole, Guid idUserLog)
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
            IdUserLog = idUserLog,
        };
    }

    /// <summary>
    ///     Method used for setting the hashed password for the user, as hashing a password is not a
    ///     domain responsibility but rather the application layer's responsibility. This method is provided
    ///     since the Password has a private setter, and it is not possible to set it directly.
    /// </summary>
    /// <param name="password"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SetHashedPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException($"{nameof(Password)} cannot be empty.");
        }

        Password = password;
    }

    public void ChangeEmail(string newEmail)
    {
        // Check if the emails differ 
        if (Email == newEmail)
        {
            throw new ArgumentException($"New {nameof(Email).ToLower()} can't be the same as previous.");
        }

        Email = newEmail;
    }

    public void ChangePhoneNumber(string newValidPhoneNumber)
    {
        // Check if the numbers differ
        if (newValidPhoneNumber == PhoneNumber)
        {
            throw new ArgumentException($"New {nameof(PhoneNumber).ToLower()} must differ from the old one.");
        }

        PhoneNumber = newValidPhoneNumber;
    }
}