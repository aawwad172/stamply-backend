# Specification: Containerize Development Environment

## Goal
Establish a consistent, isolated, and reproducible development environment for Stamply using Docker and Docker Compose. This allows any developer to start the backend and its dependencies (PostgreSQL) with a single command.

## Requirements

### 1. Backend Dockerfile
- Use a multi-stage build for efficiency.
- Base image: .NET 8 or 9 (SDK for build, ASP.NET runtime for final image).
- Target: `Stamply.Presentation.API`.
- Support environment variable overrides.

### 2. Docker Compose Configuration
- **App Service:**
    - Build from the backend Dockerfile.
    - Expose necessary ports (e.g., 5000/5001 or 8080).
    - Map local source code for hot-reloading if possible (or focus on build-and-run for now).
    - Link to the database service.
- **Database Service:**
    - Image: `postgres:alpine`.
    - Persistent volume for data to survive container restarts.
    - Set environment variables for `POSTGRES_DB`, `POSTGRES_USER`, and `POSTGRES_PASSWORD`.

### 3. Networking & Connectivity
- Configure the backend to connect to the database container using the service name (e.g., `db`).
- Use Docker networks to isolate the environment.

### 4. Developer Experience
- Ensure the environment can be started using `docker-compose up`.
- Update documentation (README) with instructions for running via Docker.
