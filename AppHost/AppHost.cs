using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Configure Postgres.
var postgresUsername = builder
    .AddParameter("PostgresUsername");
var postgresPassword = builder
    .AddParameter("PostgresPassword");

var postgresDb = builder
    .AddPostgres
    (
        "audio-engineers-platform-db",
        postgresUsername,
        postgresPassword,
        5432
    )
    .WithImage("postgres", "14-alpine")
    .WithBindMount
    (
        "../AudioEngineersPlatformBackend.API/Config/Scripts/",
        "/docker-entrypoint-initdb.d/",
        true
    );

// Configure API.
builder
    .AddProject<AudioEngineersPlatformBackend_API>("AudioEngineersPlatformBackend")
    .WaitFor(postgresDb);

builder
    .Build()
    .Run();