# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy all source code and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BackendAPI.dll"]