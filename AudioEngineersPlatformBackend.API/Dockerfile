# We are using an offical aspnet container 
# to build our own image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 

# This is the location in the image which docker 
# is going to use for file related operations.
# Any operations we do in the image are going to 
# use this directory as its default directory.
WORKDIR /app

## Copy the output of `dotnet publish...` to the image
COPY Published/ ./

# Entrypoint specifies what exaclty needs to be executed
# inside this container to get the application started.
ENTRYPOINT ["dotnet", "AudioEngineersPlatformBackend.API.dll"]
