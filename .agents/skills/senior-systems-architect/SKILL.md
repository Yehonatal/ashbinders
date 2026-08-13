---
name: senior-systems-architect
description: >-
  Senior systems architect skill for large-scale game design, domain modeling, monorepo
  organization, ADR authorship, cross-team interface design, and scaling game repositories.
  Use when designing new subsystems, defining public APIs, structuring save schemas, or establishing technical standards.
---

# Senior Systems Architect — Game Systems Architecture

Guidelines for technical direction, architectural boundaries, ADR documentation, and cross-team contract design.

## 1. Architectural Tenets
- **Scalable Architecture, Lean Initial Implementation**: Design folder layouts, interfaces, and namespaces to support 200+ developers while writing only the code required for the current milestone.
- **Strict Domain Isolation**: Each domain (`core`, `gameplay`, `embers`, `combat`, `world`, `factions`, `quests`, `puzzles`, `narrative`, `progression`) must expose an explicit public API.
- **Directional Dependency Flow**: `UI / Quests / Narrative` -> `World / Factions / Puzzles` -> `Characters / Combat / Embers / Progression` -> `Gameplay` -> `Core`. No circular dependencies.
- **Event-Driven Decoupling**: Disparate subsystems communicate through typed events dispatched via `EventBus`.

## 2. Architecture Decision Records (ADRs)
Document major technical decisions in `docs/architecture/ADR/` using this format:

```markdown
# ADR-XXXX: [Short Title]

## Status
[Proposed | Accepted | Superseded]

## Context
Technical problem, constraints, and alternatives evaluated.

## Decision
Selected architecture, component breakdown, and patterns.

## Consequences
- Positive: Performance, maintainability, and scaling benefits.
- Trade-offs: Added complexity, boilerplate, or migration requirements.
```

## 3. Subsystem Contract Rules
- Service interfaces must be pure C# (`ISaveService`, `IEmberRegistry`).
- Save schemas must include `SchemaVersion` and isolated state dictionaries.
- Scene tree mutations must occur on the main thread; use `Callable.From(...)` or `CallDeferred(...)` when handling asynchronous events.
- All documentation and specs must be technical, high-density, and free of emojis and marketing fluff.
