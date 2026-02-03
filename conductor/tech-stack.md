# Stamply Tech Stack

## 1. Core Platform
- **Language:** C# / .NET
- **Web Framework:** ASP.NET Core Minimal APIs
- **Architecture:** Clean Architecture with Domain-Driven Design (DDD)
- **Multi-tenancy:** Row-level isolation within a single PostgreSQL database.

## 2. Data & Persistence
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core (EF Core)
- **Migrations:** Managed via EF Core and a custom Makefile.

## 3. Patterns & Practices
- **CQRS:** Implemented via a **Manual Dispatcher** to manage commands and queries, avoiding heavy external dependencies like MediatR.
- **Result Pattern:** Use the Result pattern for expected domain failures and logical flows.
- **Exception Handling:** Centralized middleware for unexpected system behavior and unhandled exceptions.
- **Repository Pattern:** Generic repository for standard CRUD, with custom repositories for specialized domain logic.
- **Unit of Work:** Transaction management across repositories.
- **Validation:** Fluent Validation for request and domain validation.
- **Mapping:** Mapster for efficient object-to-object mapping.
- **Logging:** **Serilog** for structured logging across all layers.

## 4. Security & Authentication
- **Authentication:** JWT-based authentication with custom middleware.
- **Authorization:** Role-Based Access Control (RBAC).
- **Encryption:** Custom security services for secret hashing and data encryption.
- **Identity:** **UUID v7** for all entity identifiers using `GUID.GenerateVersion7()`.

## 5. Integrations & API
- **Digital Wallets:** Direct integration with **Apple Wallet (PassKit)** and **Google Wallet APIs** for real-time loyalty card management.
- **API Documentation:** **OpenAPI (Swagger)** with **API Versioning** to prevent breaking changes and ensure clear contracts.

## 6. Infrastructure & Tooling
- **Deployment:** **Docker** and **Docker Compose** for local development and production environments (Database is managed externally).
- **Testing:** **xUnit** for unit and integration testing, targeting at least 60% code coverage.
- **Git Hooks:** Husky for pre-commit linting and consistency checks.
- **Automation:** Makefile for common CLI tasks (database updates, project renaming, running).
