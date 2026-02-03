# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copy solution and project files
COPY stamply.sln ./
COPY src/Stamply.Application/Stamply.Application.csproj src/Stamply.Application/
COPY src/Stamply.Domain/Stamply.Domain.csproj src/Stamply.Domain/
COPY src/Stamply.Infrastructure/Stamply.Infrastructure.csproj src/Stamply.Infrastructure/
COPY src/Stamply.Presentation.API/Stamply.Presentation.API.csproj src/Stamply.Presentation.API/

# Restore dependencies
RUN dotnet restore

# Copy the remaining source code
COPY . .

# Build and publish
WORKDIR /src/src/Stamply.Presentation.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Create a non-privileged user
RUN addgroup -S stamply && adduser -S stamply -G stamply
USER stamply

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Stamply.Presentation.API.dll"]