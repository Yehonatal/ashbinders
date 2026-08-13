---
name: senior-systems-architect
description: >-
  Senior systems architect skill for large-scale game design, domain modeling, monorepo
  organization, ADR authorship, cross-team interface design, and scaling game repositories.
  Use when designing new subsystems, defining public APIs, structuring save schemas, or establishing technical standards.
---

# Senior Systems Architect — Game Systems Architecture Skill

This skill guides high-level technical decisions, architectural patterns, boundary enforcement, and cross-team contract design for scalable game development.

---

## 1. Architectural Tenets for Scalable Games

1. **"Architecture scales from Day 1; Implementation stays lean for prototypes."**
   - Lay out clean folder structures, strict namespaces, and decoupling patterns early so adding 50 developers later requires zero structural refactoring.
   - Build only the minimum gameplay mechanics needed to prove the vertical slice.

2. **Domain Isolation**:
   - Every major system (`embers`, `combat`, `world`, `quests`, `puzzles`, `factions`, `narrative`, `progression`) must have an explicit public API surface.
   - Dependencies flow strictly downwards:
     `World/Quests` $\rightarrow$ `Combat/Embers` $\rightarrow$ `Gameplay` $\rightarrow$ `Core`.
   - Never create circular dependencies across domains.

3. **Event-Driven Decoupling via Strong Contracts**:
   - Systems communicate via typed events (`IEvent`) published through an `EventBus`.
   - Publishers never need to know who is listening (e.g. killing an enemy fires `EnemyDefeatedEvent`; Ember drop system, Quest system, and Audio system all listen independently).

4. **Data-Driven & Toolable**:
   - Design data models as Godot `Resource` (`.tres`) objects.
   - Separate code (behavior) from content (numbers, strings, asset paths).

---

## 2. Writing Architecture Decision Records (ADRs)

When introducing a major technical system or breaking change, document it in `docs/architecture/ADR/` following this template:

```markdown
# ADR-XXXX: [Short Title]

## Context
What problem are we solving? What are the constraints, requirements, and alternatives considered?

## Decision
What is the chosen approach and technical architecture?

## Consequences
- **Positive**: Benefits gained, performance improvements, scalability wins.
- **Negative / Trade-offs**: Added boilerplate, learning curve, migration requirements.

## Compliance & Enforcement
How is this decision verified (e.g., CI linter, unit test, code review checklist)?
```

---

## 3. Subsystem Contract Guidelines

- **Service Interfaces**: Expose pure C# interfaces (e.g., `ISaveService`, `IEmberRegistry`, `IInteractionManager`).
- **State Persistence**: Design save schemas with `SchemaVersion`, metadata headers, and isolated state dictionaries.
- **Async & Threading**: All scene tree mutations MUST occur on Godot's main thread. Use `Callable.From(...)` or `CallDeferred(...)` when returning from background tasks.
