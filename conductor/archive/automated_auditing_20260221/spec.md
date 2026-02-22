# Specification: Automated Auditing for Entities

## Overview
Currently, auditing fields such as `CreatedAt`, `CreatedBy`, `UpdatedAt`, and `UpdatedBy` are manually populated in command handlers. This feature aims to automate this process using an Entity Framework Core Interceptor, ensuring consistent data integrity and cleaner application logic across the system.

## Functional Requirements
- **Automatic Interception:** Implement a custom `SaveChangesInterceptor` that intercepts database save operations.
- **Interface-Driven Auditing:** The interceptor must target all entities implementing `ICreationAudit` (for new records) and `IModificationAudit` (for existing records).
- **Audit Field Population:**
  - **Added Entities:** Automatically set `CreatedAt` to the current UTC time and `CreatedBy` to the current user's ID.
  - **Modified Entities:** Automatically set `UpdatedAt` to the current UTC time and `UpdatedBy` to the current user's ID.
- **User Identity Integration:** Use the existing `ICurrentUserService` to retrieve the current user's ID.
- **Fallback Mechanism:** If no user context is available (e.g., system tasks, background jobs), fall back to using `AuthSeedConstants.SystemUserId`.
- **Simplification:** Once automated, remove manual auditing field assignments from existing command handlers (e.g., `RegisterUserCommandHandler`).

## Non-Functional Requirements
- **Performance:** Ensure the interceptor logic is optimized to minimize overhead during database transactions.
- **Reliability:** The auditing logic must be idempotent and correctly handle transactions through the `UnitOfWork`.
- **Maintainability:** centralize auditing logic within the Infrastructure layer.

## Acceptance Criteria
- [ ] New entities implementing `ICreationAudit` have their `CreatedAt` and `CreatedBy` fields populated automatically on save.
- [ ] Existing entities implementing `IModificationAudit` have their `UpdatedAt` and `UpdatedBy` fields updated automatically on save.
- [ ] Manual assignments of these fields in command handlers are removed and the system continues to work correctly.
- [ ] The system correctly uses the "System" user ID when a request is made without a user context.

## Out of Scope
- Implementation of a full historical audit log (e.g., tracking "what" changed in each field).
- Auditing for entities that do not implement the specified auditing interfaces.
