# ADR-0001: Selection of Godot 4 and C# (.NET 8)

## Status
Accepted

## Context
Ashbinders is envisioned as a deep, scalable dark-fantasy RPG with complex systems: Ember routing networks, faction reputation graphs, modular weapon combos, deduction evidence boards, and persistent world state. We need an engine and language combination that provides:
1. Lightweight, fast-iterating 2D/top-down rendering and scene composition.
2. Strong typing, enterprise refactoring tools, and rich object-oriented/component patterns.
3. Zero licensing lock-in or royalty overhead.
4. Seamless transition from a 2-engineer indie team to a large studio team.

## Decision
We chose **Godot Engine 4.x with C# (.NET 8.0)** as the core technical stack.

## Consequences
### Positive
- Full access to modern C# 12 features (pattern matching, file-scoped namespaces, records, source generators).
- Highly performant execution with JIT/AOT compilation.
- Rich IDE support (VSCode, JetBrains Rider, Visual Studio).
- Easy automated testing with standard .NET test runners (`xUnit`, `NUnit`).

### Trade-offs & Mitigations
- Garbage collection overhead: Mitigated by strictly avoiding allocations in `_Process` and `_PhysicsProcess` hot loops.
- C# export templates require .NET SDK installed: Automated in CI pipelines.
