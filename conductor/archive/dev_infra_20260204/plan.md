# Implementation Plan: Deployment Containerization and Local Dev DB

## Phase 1: Local Development Infrastructure
- [x] **Task: Setup Local PostgreSQL Container**
    - [ ] Create `docker-compose.dev.yml` (or similar) to manage only the PostgreSQL service.
    - [ ] Configure volume persistence for local data.
    - [ ] Add a target to the `Makefile` (e.g., `make dev-db`) to start/stop the DB container.
- [x] **Task: Verify Local Connectivity**
    - [ ] Start the DB container and verify connectivity from the host using `dotnet run` or a database client.

## Phase 2: Production Containerization
- [x] **Task: Create Production Dockerfile**
    - [ ] Implement a multi-stage `Dockerfile` for the backend API.
    - [ ] Ensure it builds the solution and publishes the API project correctly.
- [x] **Task: Create Deployment Docker Compose**
    - [ ] Create `docker-compose.yml` for the full deployment stack.
    - [ ] Include the API service and environment variable configurations for production.

## Phase 3: Verification
- [x] **Task: Verify Production Build**
    - [ ] Run `docker compose build` to ensure the Dockerfile is valid.
- [x] **Task: Conductor - User Manual Verification 'Deployment Setup' (Protocol in workflow.md)**