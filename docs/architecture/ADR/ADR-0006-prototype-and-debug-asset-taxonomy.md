# ADR-0006: Prototype & Debug Asset Taxonomy and Domain-Aligned Scene Architecture

## Status
Accepted

## Context
During early phases of development (such as Phase 1) and ongoing mechanic prototyping, engineers and technical designers build systems without final artwork, voice lines, sound effects, or finalized region map geometry.

Without explicit architectural conventions:
1. Scenes placed in ad-hoc folders (such as a top-level `game/scenes/` directory) violate Domain Ownership (ADR-0002).
2. Developers cannot easily discern whether an entity (e.g. an enemy or NPC) is a temporary debug/mechanic dummy or a canonical production entity with temporary graybox geometry.
3. Test environments become monoliths that entangle mechanics from multiple regions, violating Modular Region Isolation (ADR-0005).

## Decision

### 1. Domain Co-Location of PackedScenes (`.tscn`)
All scene files MUST be co-located with their respective domain logic instead of in a global `scenes/` folder:
- Player scenes $\rightarrow$ `game/characters/player/`
- Enemy scenes $\rightarrow$ `game/characters/enemies/<region_or_category>/`
- Interactable objects $\rightarrow$ `game/world/interactables/`
- Dialogue & NPC scenes $\rightarrow$ `game/narrative/npcs/` and `game/narrative/dialogue/`
- User Interface $\rightarrow$ `game/ui/<component>/`
- Region levels & gyms $\rightarrow$ `game/world/regions/<region_name>/`

### 2. Taxonomy: Production Placeholders vs. Debug Fixtures

| Entity Category | Naming Convention | Location | Purpose |
| :--- | :--- | :--- | :--- |
| **Canonical Entity (Graybox)** | Canonical name (e.g., `player_kael.tscn`, `ember_beast.tscn`, `npc_archivist_vael.tscn`) | `game/<domain>/<feature>/` | Production-ready prefab containing game logic and components. Visuals use named placeholders (`VisualPlaceholder`, `GrayboxMesh`) awaiting final artist drops. |
| **Debug / Mechanic Fixture** | Prefixed with `debug_*` (e.g., `debug_combat_dummy.tscn`, `debug_armored_dummy.tscn`) | `game/<domain>/debug/` | Isolated test dummies and sandbox harnesses for evaluating edge cases, damage math, and tuning. |
| **Regional Test Gym** | Prefixed with `gym_<region>.tscn` (e.g., `gym_underlevels.tscn`) | `game/world/regions/<region>/` | Self-contained level gym testing region-specific puzzle & environmental mechanics. |
| **Universal Debug Gym** | `debug_gym.tscn` | `game/world/regions/debug_gym/` | Cross-cutting combat, movement, save/load, and HUD verification arena. |

### 3. Cross-Domain Communication
Like production levels, all test gyms and debug fixtures interact exclusively through typed events via `EventBus` and `WorldState`.

## Consequences
### Positive
- Direct adherence to ADR-0002 and ADR-0005.
- Complete clarity for incoming engineers and artists on which prefabs to reskin vs which are debug tools.
- Trivial exclusion of `debug/` and `debug_gym/` in release export profiles.
