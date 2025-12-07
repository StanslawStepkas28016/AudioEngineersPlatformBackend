namespace AudioEngineersPlatformBackend.Domain.Entities;

public class HubConnection
{
    private Guid _idHubConnection;
    private Guid _idUser;
    private string _connectionId;
    private User _user;

    // Properties
    public Guid IdHubConnection
    {
        get => _idHubConnection;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdHubConnection)} cannot be empty.");
            }

            _idHubConnection = value;
        }
    }

    public Guid IdUser
    {
        get => _idUser;
        set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdUser)} cannot be empty.");
            }

            _idUser = value;
        }
    }

    public string ConnectionId
    {
        get => _connectionId;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(ConnectionId)} cannot be empty.");
            }

            _connectionId = value;
        }
    }

    public User User
    {
        get => _user;
        set => _user = value;
    }

    // Constructor for EF Core
    private HubConnection()
    {
    }

    /// <summary>
    ///     Factory method to create a HubConnection.
    /// </summary>
    /// <param name="idUser"></param>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public static HubConnection Create(Guid idUser, string connectionId)
    {
        return new HubConnection
        {
            IdHubConnection = Guid.NewGuid(),
            IdUser = idUser,
            ConnectionId = connectionId,
        };
    }

    /// <summary>
    ///     Factory method to create a HubConnection entity with a specific idHubConnection,
    ///     used for seeding purposes.
    /// </summary>
    /// <param name="idHubConnection"></param>
    /// <param name="idUser"></param>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public static HubConnection CreateWithId(Guid idHubConnection, Guid idUser, string connectionId)
    {
        return new HubConnection
        {
            IdHubConnection = idHubConnection,
            IdUser = idUser,
            ConnectionId = connectionId,
        };
    }
}