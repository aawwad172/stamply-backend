# Specification: Deployment Containerization and Local Dev DB

## Goal
Prepare Stamply for deployment using Docker and Docker Compose while providing a streamlined local development workflow where the database runs in a container and the application runs natively.

## Requirements

### 1. Production Dockerfile
- Multi-stage build for the `Stamply.Presentation.API` project.
- Optimized for runtime (using ASP.NET runtime image).
- Proper handling of environment variables for production configuration.

### 2. Deployment Docker Compose
- Orchestrates the Stamply API container.
- Configurable to connect to an external PostgreSQL instance (or one defined in a separate compose).
- Handles port mapping and restart policies for production resilience.

### 3. Local Development Workflow
- A dedicated Docker Compose or script to start only the PostgreSQL database.
- Database data must be persistent across container restarts.
- The API should be able to connect to this local container when run via `dotnet run` or `make run`.

### 4. Networking
- Standardized port (5432) for PostgreSQL.
- Clear separation between deployment (container-to-container) and development (host-to-container) connection strings.