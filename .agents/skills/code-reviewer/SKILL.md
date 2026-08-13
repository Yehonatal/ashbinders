---
name: code-reviewer
description: >-
  Specialized code review skill for game systems, Godot 4, C#, architecture integrity,
  performance, and maintainability. Use when reviewing PRs, diffs, refactors, or checking
  for bugs, memory leaks, domain boundary violations, and non-deterministic physics.
---

# Code Reviewer — Game Architecture & Systems Quality Skill

This skill enforces high engineering standards, defensive programming, domain separation, and zero-leak memory management for Godot + C# game code.

---

## 1. Code Review Checklist for Game Systems

### A. Architectural Integrity & Domain Boundaries
- [ ] **No Domain Leaks**: Does `core/` know anything about game lore (e.g. references to Kael, Ember types, Factions)? `core/` must be 100% domain-agnostic.
- [ ] **Region Modularity**: Are region scripts in `world/regions/<region_name>` self-contained without hard dependencies on other region assets/scenes?
- [ ] **Event-Driven Decoupling**: Are disparate systems communicating via `EventBus` rather than direct singleton references or spaghetti cross-node calls?
- [ ] **Data Separation**: Is static game data defined as Godot `Resource` (`.tres`) instead of hardcoded magic values inside C# scripts?

### B. Performance & Memory Management (Hot Loop Safety)
- [ ] **No Allocations in Physics/Process**: Are there `new` instantiations, LINQ queries (`.Where()`, `.Select()`), lambda closures, or string interpolations inside `_Process` or `_PhysicsProcess`?
- [ ] **Signal Leak Prevention**: Are C# event subscriptions or Godot signal connections properly unregistered in `_ExitTree()` or `Dispose()`?
- [ ] **Node Reference Caching**: Are `GetNode<T>()` or `%UniqueName` calls cached in `_Ready()` instead of being looked up on every frame?
- [ ] **Instance Validity**: Are dynamic node references verified with `GodotObject.IsInstanceValid(node)` before accessing properties?

### C. Game Feel & Determinism
- [ ] **Delta Time Utilization**: Are all velocity changes and movement equations multiplied by `delta` or processed via `Mathf.MoveToward` in `_PhysicsProcess`?
- [ ] **State Machine Invariants**: Can an entity enter an illegal state? Are `Exit()` and `Enter()` methods guaranteed to clean up timers, hitboxes, and temporary modifiers?
- [ ] **Hitbox Safety**: Are attack hitboxes disabled by default and only enabled during valid active frames?

### D. Save / Load & Persistence
- [ ] **Serialization Completeness**: Does any newly added persistent state implement `ISaveable` or register with `SaveManager`?
- [ ] **Schema Versioning**: Are save models backwards-compatible and schema-versioned to prevent breaking user saves during updates?

---

## 2. Review Verdict Guidelines

When conducting a review, structure the response clearly:

1. **Summary of Changes**: High-level evaluation of the diff.
2. **Critical Blockers** (Must Fix): Memory leaks, boundary violations, frame-rate drops, crash risks.
3. **Important Improvements** (Should Fix): Game feel refinements, missing tests, code duplication.
4. **Architectural / Nitpicks** (Optional): Variable naming, doc comments, minor style preferences.
5. **Verdict**: `APPROVE`, `REQUEST_CHANGES`, or `COMMENT`.
