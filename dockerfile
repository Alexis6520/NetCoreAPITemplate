# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY src/Host/*.csproj ./Host/
COPY src/Persistence/*.csproj ./Persistence/
RUN dotnet restore Host/Host.csproj

# copy everything else and build app
COPY src/. .
WORKDIR /source/Host
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Host.dll"]