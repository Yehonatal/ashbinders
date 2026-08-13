# Testing Strategy & Quality Assurance Philosophy

## 1. Automated Testing Pyramid

```
        / \
       /   \     End-to-End Integration Tests (Godot Test Scenes)
      /-----\
     /       \   Subsystem Integration Tests (Save/Load, FSM, Damage Pipeline)
    /---------\
   /           \ Pure C# Unit Tests (Math, State Logic, EventBus, Serialization)
  /-------------\
```

---

## 2. Test Suites Overview

1. **Pure Unit Tests (`tests/unit/`)**:
   - `EventBusTests.cs`: Verifies publish, subscribe, unsubscribe, and zero memory leaks.
   - `StateMachineTests.cs`: Verifies state entry, exit lifecycle, and invalid transition handling.
   - `DamageCalculationTests.cs`: Verifies damage types, armor reductions, and poise calculations.
2. **Save & Persistence Integration Tests (`tests/integration/save_load/`)**:
   - `SavePersistenceTests.cs`: Serializes game state to JSON, deserializes, and verifies complete object graph fidelity.
   - `SchemaMigrationTests.cs`: Verifies older save schema versions upgrade gracefully.
3. **Asset & Path Validation (`scripts/validate_assets.py`)**:
   - Checks that all textures, audio files, and resources adhere to snake_case naming rules.
