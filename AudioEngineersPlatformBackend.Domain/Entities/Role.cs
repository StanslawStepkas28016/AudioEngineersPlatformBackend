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

    // References (Navigation Properties)
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
    ///     Method used to set the IdRole for seeding purposes ONLY.
    /// </summary>
    /// <param name="idRole"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SetIdRoleForSeeding(Guid idRole)
    {
        if (idRole == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(idRole)} cannot be empty");
        }

        IdRole = idRole;
    }
}