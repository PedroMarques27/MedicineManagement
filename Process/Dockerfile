# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the project files
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects
COPY ["Process/Process.csproj", "Process/"]
COPY ["Database/Database.csproj", "Database/"]

# Restore the project dependencies
RUN dotnet restore "Process/Process.csproj"

# Copy all files and build the application
COPY . .
WORKDIR "/src/Process"
RUN dotnet build "Process.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Process.csproj" -c Release -o /app/publish

# Use the base image to run the published app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Process.dll"]
