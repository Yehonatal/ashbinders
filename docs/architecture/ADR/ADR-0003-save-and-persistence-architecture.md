# ADR-0003: Save and Persistence Architecture

## Status
Accepted

## Context
Ashbinders is an open-world RPG where every mechanical choice (ember routing, machine activations, Ember-Bound fates, faction alliances) causes persistent world state mutations. A naive save system that serializes raw Godot scene trees is fragile, non-backwards-compatible, and prone to game-breaking corruptions across patches.

## Decision
1. Build a decoupled, schema-versioned `SaveManager` in `core/save/`.
2. Persist state through explicit data transfer objects (`SaveData`, `PlayerSaveData`, `WorldSaveData`) serialized to JSON with checksum verification.
3. Interactive nodes implement `ISaveable` to export and restore their state cleanly without saving engine node references.

## Consequences
- Saves are deterministic, human-readable for debugging, and easily migrated across game versions.
- Saves can be tested thoroughly via automated unit tests in `tests/`.
