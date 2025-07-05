namespace AudioEngineersPlatformBackend.Domain.Entities;

public class Role
{
    // Backing fields
    private Guid _idRole;
    private string _roleName;
    private ICollection<User> _users;

    // Constants
    private const int RoleNameMinLength = 3;

    // Properties
    public Guid IdRole
    {
        get { return _idRole; }
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdRole)} cannot be empty)");
            }

            _idRole = value;
        }
    }

    public string RoleName
    {
        get { return _roleName; }
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(RoleName)} cannot be empty");
            }

            if (value.Length < RoleNameMinLength)
            {
                throw new ArgumentException(
                    $"You must provide at least {RoleNameMinLength} characters for {nameof(RoleName)}");
            }

            _roleName = value;
        }
    }

    // References
    public ICollection<User> Users
    {
        get { return _users; }
        private set { _users = value; }
    }

    // Private constructor for EF Core
    private Role()
    {
    }

    /// <summary>
    ///     Factory method to create a new Role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public static Role Create(string roleName)
    {
        return new Role
        {
            IdRole = Guid.NewGuid(),
            RoleName = roleName,
        };
    }

    /// <summary>
    ///     Factory method to create a new Role with a specific IdRole.
    ///     Used for seeding.
    /// </summary>
    /// <param name="idRole"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public static Role CreateWithId(Guid idRole, string roleName)
    {
        return new Role
        {
            IdRole = idRole,
            RoleName = roleName,
        };
    }
}