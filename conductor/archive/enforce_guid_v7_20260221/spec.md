# Specification: Enforce Guid.CreateVersion7

## Overview
To improve database performance (specifically index fragmentation in PostgreSQL) and ensure consistent UUID versioning, we want to enforce the use of `Guid.CreateVersion7()` and prohibit `Guid.NewGuid()`. This enforcement will be implemented at the build level, ensuring that any use of the prohibited method results in a compilation error.

## Functional Requirements
- **Build-Level Enforcement:** Use a `Directory.Build.props` file to configure the `Microsoft.CodeAnalysis.BannedApiAnalyzers` across the entire solution.
- **Compiler Error:** Configure the rule so that any occurrence of `Guid.NewGuid()` triggers a compiler error, preventing the build from succeeding.
- **Solution-Wide Scope:** The rule must apply to all projects within the solution, including Domain, Application, Infrastructure, and Presentation.
- **No Exceptions:** No parts of the codebase (including migrations and tests) are exempt from this rule.

## Non-Functional Requirements
- **Developer Feedback:** The error message should clearly state that `Guid.CreateVersion7()` must be used instead of `Guid.NewGuid()`.
- **Performance:** Build-time analysis should have negligible impact on overall compilation time.

## Acceptance Criteria
- [ ] Any use of `Guid.NewGuid()` anywhere in the solution results in a build failure.
- [ ] The build error message points to the prohibited usage and suggests the correct alternative.
- [ ] All existing instances of `Guid.NewGuid()` are replaced with `Guid.CreateVersion7()`.

## Out of Scope
- Enforcement of other coding standards or banned APIs beyond `Guid.NewGuid()`.
