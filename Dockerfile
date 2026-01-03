FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY . .
RUN dotnet restore "AudioEngineersPlatformBackend.API/AudioEngineersPlatformBackend.API.csproj"
RUN dotnet restore "AudioEngineersPlatformBackend.Tests/AudioEngineersPlatformBackend.Tests.csproj"

WORKDIR "/src/AudioEngineersPlatformBackend.API"
RUN dotnet build "./AudioEngineersPlatformBackend.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/AudioEngineersPlatformBackend.Tests"
RUN dotnet test --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/AudioEngineersPlatformBackend.API"
RUN dotnet publish "./AudioEngineersPlatformBackend.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AudioEngineersPlatformBackend.API.dll"]