# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files and restore as distinct layers
COPY ["Src/API/API.csproj", "Src/API/"]
COPY ["Src/Domain/Domain.csproj", "Src/Domain/"]
COPY ["Src/Application/Application.csproj", "Src/Application/"]
COPY ["Src/Infrastructure/Infrastructure.csproj", "Src/Infrastructure/"]

# Restore dependencies


RUN dotnet restore "Src/API/API.csproj"

# Copy all other files
COPY . .

# Build the application
RUN dotnet publish "Src/API/API.csproj" -c Release -o /app/publish

# Stage 2: Serve the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
