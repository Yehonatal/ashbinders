---
name: code-reviewer
description: >-
  Specialized code review skill for game systems, Godot 4, C#, architecture integrity,
  performance, and maintainability. Use when reviewing PRs, diffs, refactors, or checking
  for bugs, memory leaks, domain boundary violations, and non-deterministic physics.
---

# Code Reviewer — Game Architecture & Systems Quality

Guidelines for reviewing Godot 4 and C# code changes for correctness, performance, and architecture integrity.

## 1. Review Rules & Tone
- No emojis or subjective conversational filler in review feedback.
- Point directly to line numbers, concrete failure cases, performance regressions, and domain boundary leaks.

## 2. Review Checklist

### A. Architecture and Domain Boundaries
- `core/` contains zero references to game lore, specific entities, or faction names.
- Region directories under `world/regions/<region_name>/` are self-contained without cross-region dependencies.
- Communication between decoupled domains passes through `EventBus`.
- Static data is defined as Godot `Resource` (`.tres`) files, not hardcoded constants in logic scripts.

### B. Performance and Allocations
- Zero memory allocations inside `_Process` or `_PhysicsProcess` hot loops.
- C# event subscriptions and Godot signals are disconnected in `_ExitTree()` to prevent memory leaks.
- Node paths and references are cached in `_Ready()`.
- Dynamic node references verify `GodotObject.IsInstanceValid(node)` before access.

### C. Game Feel and Determinism
- Velocity equations use `Mathf.MoveToward` or are multiplied by delta in `_PhysicsProcess`.
- State machines guarantee complete cleanup of active timers and hitboxes on `Exit()`.
- Attack hitboxes are disabled by default and enabled only during active frames.

### D. Persistence
- Persistent state changes implement `ISaveable` or register with `SaveManager`.
- Save schemas maintain backward compatibility.

## 3. Review Output Format
- Summary: High-level technical evaluation.
- Blockers: Memory leaks, boundary violations, frame drops, or crashes.
- Improvements: Non-blocking performance or design improvements.
- Verdict: APPROVE, REQUEST_CHANGES, or COMMENT.
