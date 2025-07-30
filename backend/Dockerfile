# Use the official .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy solution file and project files
COPY *.sln ./
COPY TMW2Play.Api/*.csproj ./TMW2Play.Api/

# Restore dependencies for the solution
RUN dotnet restore TMW2Play.Api/TMW2Play.Api.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish TMW2Play.Api/TMW2Play.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build-env /app/out .

# Expose port
EXPOSE 80

ENTRYPOINT ["dotnet", "TMW2Play.Api.dll"]