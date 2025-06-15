namespace AudioEngineersPlatformBackend.Domain.Entities;

public class Role
{
    public Guid IdRole { get; private set; }
    public string RoleName { get; private set; }
    public ICollection<User> Users { get; private set; }

    // Constants
    private const int RoleNameMinLength = 3;

    private Role()
    {
    }

    public Role(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("You must provide a roleName", nameof(roleName));
        }

        if (roleName.Length < RoleNameMinLength)
        {
            throw new ArgumentException("You must provide at least " + RoleNameMinLength + " characters for rolename" +
                                        nameof(roleName));
        }

        IdRole = Guid.NewGuid();
        RoleName = roleName;
    }
}