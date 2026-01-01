using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add docker-compose environment for publish.
builder
    .AddDockerComposeEnvironment("compose")
    .WithDashboard
    (dashboard =>
        {
            dashboard
                .WithHostPort(8080);
        }
    );

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
    )
    .WithHostPort(5432)
    .WithDataVolume()
    .PublishAsDockerComposeService
    ((
            _,
            service
        ) =>
        {
            service.Restart = "unless-stopped";
            service.Healthcheck = new()
            {
                Interval = "10s",
                Timeout = "3s",
                Retries = 3,
                Test =
                [
                    "CMD-SHELL",
                    "pg_isready -U postgres -d audio-engineers-platform-db"
                ],
                StartPeriod = "0s"
            };
        }
    );

// Configure API.
var env = builder.AddParameter("Env", builder.Environment.EnvironmentName);
var lpsLicenseKey = builder.AddParameter("LpsLicenseKey");
var awsAccessKey = builder.AddParameter("AwsAccessKey");
var awsSecretKey = builder.AddParameter("AwsSecretKey");
var dbConnectionString = builder.AddParameter("DbConnectionString");
var jwtSecret = builder.AddParameter("JwtSecret");

var api = builder
    .AddProject<AudioEngineersPlatformBackend_API>("soundbest-api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", env)
    .WithEnvironment("LuckyPennySoftwareSettings__LicenseKey", lpsLicenseKey)
    .WithEnvironment("AwsSettings__AccessKey", awsAccessKey)
    .WithEnvironment("AwsSettings__SecretKey", awsSecretKey)
    .WithEnvironment("ConnectionStrings__Db", dbConnectionString)
    .WithEnvironment("JwtSettings__Secret", jwtSecret)
    .WaitFor(db)
    .WithHttpEndpoint(9080, name: "api")
    .WithExternalHttpEndpoints()
    .PublishAsDockerComposeService
    ((
            _,
            service
        ) =>
        {
            service.Restart = "unless-stopped";
        }
    );

// Configure client app.
builder
    .AddViteApp("soundbest-frontend", "../audio-engineers-platform-frontend")
    .WaitFor(api)
    .WithEndpoint
    (
        "http",
        endpoint => { endpoint.Port = 5173; }
    )
    .WithExternalHttpEndpoints();

builder
    .Build()
    .Run();