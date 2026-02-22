# Implementation Plan: Enforce Guid.CreateVersion7

## Phase 1: Global Build Configuration
Setting up the infrastructure to detect and block `Guid.NewGuid()`.

- [ ] Task: Create `Directory.Build.props` in the root directory.
    - Configure it to include the `Microsoft.CodeAnalysis.BannedApiAnalyzers` package for all projects.
    - Set `EnforceCodeStyleInBuild` to `true`.
- [ ] Task: Create `BannedSymbols.txt` in the root directory.
    - Add `M:System.Guid.NewGuid;Use Guid.CreateVersion7() instead` to the file.
- [ ] Task: Update project files or `Directory.Build.props` to ensure `BannedSymbols.txt` is treated as an `AdditionalFiles` item for the analyzer.
- [ ] Task: Conductor - User Manual Verification 'Global Build Configuration' (Protocol in workflow.md)

## Phase 2: Codebase Refactoring
Replacing all existing occurrences of `Guid.NewGuid()` to satisfy the new build rules.

- [ ] Task: Search the entire solution for `Guid.NewGuid()`.
- [ ] Task: Replace all found occurrences with `Guid.CreateVersion7()`.
    - Ensure necessary `using System;` or fully qualified names are used.
- [ ] Task: Conductor - User Manual Verification 'Codebase Refactoring' (Protocol in workflow.md)

## Phase 3: Final Validation
Confirming the enforcement is active and the build is clean.

- [ ] Task: Attempt to build the solution (`dotnet build`).
    - Verify it builds successfully after refactoring.
- [ ] Task: Temporarily re-introduce one instance of `Guid.NewGuid()` to verify that the build fails as expected.
- [ ] Task: Conductor - User Manual Verification 'Final Validation' (Protocol in workflow.md)
