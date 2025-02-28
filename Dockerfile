# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files and restore as distinct layers
COPY ["src/API/API.csproj", "src/API/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

# Restore dependencies


RUN dotnet restore "src/API/API.csproj"

# Copy all other files
COPY . .

# Build the application
RUN dotnet publish "src/API/API.csproj" -c Release -o /app/publish

# Stage 2: Serve the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
