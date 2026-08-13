# Contributing to Ashbinders

Welcome to the **Ashbinders** development team! Whether you are one of the initial core engineers or joining as part of our scaled studio development, this guide outlines the workflow and quality standards required for all contributions.

---

## 1. Monorepo Philosophy
- All code, tools, automation, documentation, and tests live in this repository.
- The Godot engine project lives in `game/`.
- Domain isolation must be strictly maintained (see [Architecture Boundaries](.agents/rules/architecture_boundaries.md)).

---

## 2. Development Workflow

1. **Create an Issue & Branch**:
   - Branch names must follow: `feature/<feature-name>`, `fix/<bug-name>`, or `refactor/<cleanup-name>`.
2. **Implement with Quality**:
   - Adhere to [Coding Standards](.agents/rules/coding_standards.md).
   - Write unit/integration tests in `tests/` for gameplay algorithms, state machines, and save/load logic.
   - Run asset validation: `python3 scripts/validate_assets.py`.
3. **Submit a Pull Request**:
   - Ensure all CI tests pass.
   - Request review from a peer engineer.
   - Squash-and-merge once approved.

---

## 3. Pre-Commit Checklist
- [ ] Code builds without errors or warnings (`dotnet build game/Ashbinders.csproj`).
- [ ] Unit tests pass (`dotnet test tests/Ashbinders.Tests.csproj`).
- [ ] Assets and resources conform to naming standards.
- [ ] No game-specific logic leaked into `game/core/`.
