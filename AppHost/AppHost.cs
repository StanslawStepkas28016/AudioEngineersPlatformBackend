using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add docker-compose environment for publish.
builder.AddDockerComposeEnvironment("docker-compose-artifacts");

// Configure Postgres.
var postgresUsername = builder
    .AddParameter("PostgresUsername");
var postgresPassword = builder
    .AddParameter("PostgresPassword");

var db = builder
    .AddPostgres
    (
        "soundbest-db",
        postgresUsername,
        postgresPassword,
        5432
    )
    .WithImage("postgres", "14-alpine")
    .WithBindMount
    (
        "../AudioEngineersPlatformBackend.Infrastructure/Config/Scripts/",
        "/docker-entrypoint-initdb.d/",
        true
    );

// Configure API.
var api = builder
    .AddProject<AudioEngineersPlatformBackend_API>("soundbest-api")
    .WithEnvironment("LuckyPennySoftwareSettings__LicenseKey", "")
    .WaitFor(db);

// Configure client app.
builder
    .AddViteApp("soundbest-frontend", "../audio-engineers-platform-frontend", "prod")
    .WithNpm()
    .WithEnvironment("VITE_BACKEND_API", "http://localhost:5181/api")
    .WithEnvironment("VITE_BACKEND_HUB", "http://localhost:5181/chat-hub")
    .WithReference(api)
    .WaitFor(api)
    .WithEndpoint("http", endpoint => { endpoint.Port = builder.ExecutionContext.IsRunMode ? 5173 : null; });

builder
    .Build()
    .Run();