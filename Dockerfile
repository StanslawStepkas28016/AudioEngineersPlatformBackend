FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AudioEngineersPlatformBackend.API/AudioEngineersPlatformBackend.API.csproj", "AudioEngineersPlatformBackend.API/"]
COPY ["AudioEngineersPlatformBackend.Application/AudioEngineersPlatformBackend.Application.csproj", "AudioEngineersPlatformBackend.Application/"]
COPY ["AudioEngineersPlatformBackend.Domain/AudioEngineersPlatformBackend.Domain.csproj", "AudioEngineersPlatformBackend.Domain/"]
COPY ["AudioEngineersPlatformBackend.Infrastructure/AudioEngineersPlatformBackend.Infrastructure.csproj", "AudioEngineersPlatformBackend.Infrastructure/"]
COPY ["AudioEngineersPlatformBackend.Resources/AudioEngineersPlatformBackend.Resources.csproj", "AudioEngineersPlatformBackend.Resources/"]
RUN dotnet restore "AudioEngineersPlatformBackend.API/AudioEngineersPlatformBackend.API.csproj"
COPY . .
WORKDIR "/src/AudioEngineersPlatformBackend.API"
RUN dotnet build "./AudioEngineersPlatformBackend.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AudioEngineersPlatformBackend.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AudioEngineersPlatformBackend.API.dll"]