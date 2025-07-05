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

    // Properties
    public Guid IdUser
    {
        get { return _idUser; }
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("IdUser cannot be empty", nameof(value));
            }

            _idUser = value;
        }
    }

    public string FirstName
    {
        get { return _firstName; }
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("FirstName cannot be empty", nameof(value));
            }

            _firstName = value;
        }
    }

    public string LastName
    {
        get { return _lastName; }
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("LastName cannot be empty", nameof(value));
            }

            _lastName = value;
        }
    }

    public string Email
    {
        get { return _email; }
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || new EmailVo(value).GetValidEmail() != value)
            {
                throw new ArgumentException("Invalid email address", nameof(value));
            }

            _email = value;
        }
    }

    public string PhoneNumber
    {
        get { return _phoneNumber; }
        private set
        {
            if (new PhoneNumberVo(value).GetValidPhoneNumber() != value)
            {
                throw new ArgumentException("Invalid phone number", nameof(value));
            }

            _phoneNumber = value;
        }
    }

    public string Password
    {
        get { return _password; }
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Password cannot be empty", nameof(value));
            }

            _password = value;
        }
    }

    // References
    public Guid IdRole
    {
        get { return _idRole; }
        private set { _idRole = value; }
    }

    public virtual Role Role
    {
        get { return _role; }
        private set { _role = value; }
    }

    public Guid IdUserLog
    {
        get { return _idUserLog; }
        private set { _idUserLog = value; }
    }

    public virtual UserLog UserLog
    {
        get { return _userLog; }
        private set { _userLog = value; }
    }

    public ICollection<Advert> Adverts
    {
        get { return _adverts; }
        private set { _adverts = value; }
    }

    // Private constructor used for EF Core
    private User()
    {
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
            throw new ArgumentException("Password cannot be empty", nameof(password));
        }

        Password = password;
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
}