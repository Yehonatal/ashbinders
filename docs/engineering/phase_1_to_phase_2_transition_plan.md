# Phase 1 to Phase 2 Transition Plan: Architecture, Taxonomy & Production Skinning

## 1. Executive Summary

During **Phase 1 (Technical Foundation & Playable Prototype)**, all systems are built under a **Logic-First Component Architecture**. Gameplay mechanics, physics, event-driven state transitions, and save/load serialization are validated using geometric graybox placeholders (`ColorRect`, primitive collision shapes) before high-fidelity visual assets, audio, or final dialogue scripts are authored.

This document formalizes the decisions made in Phase 1 and outlines the exact technical pipeline for transitioning the project into **Phase 2: Underlevels Vertical Slice** without technical debt, merge chaos, or throwaway code.

---

## 2. Phase 1 Architectural Baseline & Decisions

All systems in Phase 1 adhere to strict Architecture Decision Records (ADRs):

1. **Godot 4.x + C# (.NET 8.0)** ([ADR-0001](file:///home/yehonatal/Documents/Work/ashbinders/docs/architecture/ADR/ADR-0001-godot-csharp.md)): Engine logic and math are written in high-performance, type-safe C#.
2. **Domain Ownership** ([ADR-0002](file:///home/yehonatal/Documents/Work/ashbinders/docs/architecture/ADR/ADR-0002-monorepo-and-domain-ownership.md)): Code and scenes are co-located by functional domain (`characters/`, `world/`, `combat/`, `embers/`, `ui/`, `narrative/`), eliminating flat technical folders.
3. **Save & Persistence Fidelity** ([ADR-0003](file:///home/yehonatal/Documents/Work/ashbinders/docs/architecture/ADR/ADR-0003-save-and-persistence-architecture.md)): Isolated state dictionaries keyed by component with integer `SchemaVersion` tracking.
4. **Asset Taxonomy & Resource Schemas** ([ADR-0004](file:///home/yehonatal/Documents/Work/ashbinders/docs/architecture/ADR/ADR-0004-asset-and-resource-conventions.md)): Strict `snake_case` taxonomy with Godot `.tres` data schemas.
5. **Modular Region Isolation** ([ADR-0005](file:///home/yehonatal/Documents/Work/ashbinders/docs/architecture/ADR/ADR-0005-modular-region-isolation.md)): Autonomous region folders in `game/world/regions/<region_name>/` communicating exclusively via `EventBus` and `WorldState`.
6. **Prototype & Debug Asset Taxonomy** ([ADR-0006](file:///home/yehonatal/Documents/Work/ashbinders/docs/architecture/ADR/ADR-0006-prototype-and-debug-asset-taxonomy.md)): Formal separation between Canonical Production Placeholders, Developer Debug Fixtures (`debug_*`), and Regional Test Gyms (`gym_<region>.tscn`).
7. **2.5D Isometric Traversal & Layered Elevation** ([ADR-0007](file:///home/yehonatal/Documents/Work/ashbinders/docs/architecture/ADR/ADR-0007-traversal-mechanics-and-elevation-architecture.md)): Traversal state machine (`Grounded`, `Jumping`, `Climbing`, `Swimming`) and environmental volume interactions.

---

## 3. The Decoupled Skinning Pattern

To prevent rewriting scenes when production art arrives, every `.tscn` file is constructed with a decoupled visual layer:

```text
┌─────────────────────────────────────────────────────────────────────────────┐
│                             player_kael.tscn                                │
├──────────────────────────────────────┬──────────────────────────────────────┤
│          C# LOGIC SKELETON           │            VISUAL LAYER              │
│     (Permanent Across All Phases)    │  (Phase 1 Graybox → Phase 2+ Final)  │
├──────────────────────────────────────┼──────────────────────────────────────┤
│ • PlayerController.cs (Movement/Dash)│                                      │
│ • HealthComponent.cs (Damage/Death)  │  Phase 1: [VisualPlaceholder]        │
│ • InteractionDetector.cs             │           (ColorRect + Glow)         │
│ • AshbinderChain.cs (Hitbox)         │                  ↓                   │
│ • EmberSocket.cs (Motion Ember)      │  Phase 2+: [Visuals]                 │
│ • Camera2D (Tracking)                │           (AnimatedSprite2D / VFX)   │
└──────────────────────────────────────┴──────────────────────────────────────┘
```

### Entity Classification Matrix

| Entity | Phase 1 Role (Current) | Phase 2 Action (Vertical Slice) | Final Category |
| :--- | :--- | :--- | :--- |
| `characters/player/player_kael.tscn` | Validates 8-way movement, acceleration, dash i-frames, chain attack arc | Replace `VisualPlaceholder` with Kael sprite sheet, animation tree, and cloth cloak physics | **Canonical Entity** |
| `characters/enemies/underlevels/ember_beast.tscn` | Validates chase AI, attack windup, hitstun, and Motion Ember drop | Replace `VisualPlaceholder` with Scavenger Beast spritesheet and death dissolve shader | **Canonical Entity** |
| `characters/enemies/debug/debug_combat_dummy.tscn` | Damage calculation, knockback, and hitstop testing harness | Preserved as permanent developer debug fixture for unit testing and weapon balancing | **Debug Fixture** |
| `characters/enemies/debug/debug_armored_dummy.tscn` | Armor-break threshold and heavy weapon swap testing | Preserved as permanent developer debug fixture for armor penetration tests | **Debug Fixture** |
| `world/interactables/ancient_ember_device.tscn` | Socket extraction validation, gate power trigger | Replace `VisualBase` with isometric hydraulic boiler art and steam particle effects | **Canonical Entity** |
| `world/interactables/ember_gate.tscn` | Gate collision enable/disable state logic | Replace `VisualRect` with rusted iron portcullis sprite and mechanical lowering animation | **Canonical Entity** |
| `world/interactables/ancient_save_anchor.tscn` | JSON save/load disk persistence trigger | Replace `VisualBase` with carved stone obelisk and ember flame shader | **Canonical Entity** |
| `narrative/npcs/npc_archivist_vael.tscn` | Proximity interaction and dialogue event trigger | Replace placeholder with Archivist Vael animated NPC sprite and dialogue idle loop | **Canonical Entity** |
| `ui/hud/hud.tscn` | HP bar, weapon head label, Ember state indicators | Style with 9-patch frames, ornamental HP vial texture, and Ember crystal socket UI | **Canonical UI** |
| `ui/dialogue/dialogue_modal.tscn` | EventBus dialogue listener and choice selection | Style with dark-fantasy dialog frame, speaker portrait box, and typewriter SFX | **Canonical UI** |
| `ui/debug/test_mode_info_window.tscn` | In-game developer controls and mechanic legend | Retained in `ui/debug/` for developer builds; stripped in release exports | **Debug UI** |
| `world/regions/underlevels/gym_underlevels.tscn` | Phase 1 end-to-end playable loop arena | Retained as the Underlevels regression test gym alongside the new production slice | **Regional Gym** |
| `world/regions/debug_gym/debug_gym.tscn` | Cross-cutting mechanics sandbox arena | Retained as the global combat and progression debug arena | **Universal Gym** |

---

## 4. Phase 2 Scope & Team Expansion

Phase 2 scales the team from 2 engineers to **4–6 developers** (2 Engineers, 2 2D Artists, 1 Level/Quest Designer, 1 Narrative Writer).

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                      PHASE 2 DELIVERABLES BREAKDOWN                         │
├──────────────────────┬───────────────────────────────┬──────────────────────┤
│ Domain               │ Deliverable                   │ Technical Dependency │
├──────────────────────┼───────────────────────────────┼──────────────────────┤
│ **Art & Animation**  │ Kael & Ember Beast Sprites    │ `player_kael.tscn`   │
│                      │ Underlevels Tilemap (16x16)   │ `TileMapLayer`       │
│                      │ Machine & Gate VFX Shaders    │ `CanvasItemShader`   │
├──────────────────────┼───────────────────────────────┼──────────────────────┤
│ **Combat & Embers**  │ Hammer Head Weapon Impl       │ `IWeaponHead`        │
│                      │ Armor Piercing / Stun System  │ `DamagePipeline`     │
│                      │ Guard Ember & Barrier Skill   │ `EmberType.Guard`    │
├──────────────────────┼───────────────────────────────┼──────────────────────┤
│ **World & Levels**   │ Underlevels Sector A Map      │ `underlevels/`       │
│                      │ First Vault Puzzle Dungeon    │ `puzzles/`           │
│                      │ Ember Tide Surge Hazard       │ `WorldState`         │
├──────────────────────┼───────────────────────────────┼──────────────────────┤
│ **Narrative & Quests**│ Scavenger Faction Dialogue    │ `EventBus`           │
│                      │ Archivist Vael Intro Quest    │ `quests/`            │
│                      │ Evidence Board Item Schema    │ `narrative/`         │
├──────────────────────┼───────────────────────────────┼──────────────────────┤
│ **Audio & Polish**   │ Footstep & Surface SFX        │ `audio/`             │
│                      │ Weapon Swing & Hit Audio      │ `AudioManager`       │
│                      │ Subterranean Ambient Track    │ `AudioStreamPlayer2D`│
└──────────────────────┴───────────────────────────────┴──────────────────────┘
```

---

## 5. Step-by-Step Production Skinning Workflow

When 2D Artists and Level Designers begin Phase 2 tasks:

### Step 1: Skinning an Existing Character or Interactable
1. Open the target scene in Godot (e.g. `res://characters/player/player_kael.tscn`).
2. Add the artist asset node (e.g. `AnimatedSprite2D` or `Sprite2D`) under the scene root.
3. Import the corresponding texture from `game/assets/textures/characters/kael/`.
4. Delete or hide the `VisualPlaceholder` node.
5. Re-run `python3 scripts/validate_assets.py` to ensure naming adherence.

### Step 2: Creating a Production Level Slice
1. Create `game/world/regions/underlevels/underlevels_sector_a.tscn`.
2. Place `TileMapLayer` nodes using the Underlevels tileset (`game/assets/textures/environment/underlevels/`).
3. Instantiate canonical prefabs (`player_kael.tscn`, `ember_beast.tscn`, `ancient_ember_device.tscn`, `ember_gate.tscn`, `ancient_save_anchor.tscn`).
4. Keep `gym_underlevels.tscn` intact for automated CI and fast regression testing.

### Step 3: Save Schema Evolution
When adding new fields in Phase 2 (e.g. `UnlocksHammerHead`, `GuardEmberCount`):
1. Increment `SaveData.SchemaVersion` from `1` to `2` in `game/core/save/SaveData.cs`.
2. Add migration handler in `SaveManager.cs` to guarantee backward compatibility with Phase 1 saves.
3. Add a unit test in `tests/` verifying schema upgrade fidelity.

---

## 6. Definition of Done for Phase 1 $\rightarrow$ Phase 2 Gate

The project is ready for Phase 2 kickoff when:
- [x] All legacy flat scene directories (`game/scenes/`) are deleted.
- [x] All `.tscn` files reside in domain folders with clear `VisualPlaceholder` markers.
- [x] Both `gym_underlevels.tscn` and `debug_gym.tscn` load without errors or missing references.
- [x] All automated unit tests in `tests/` pass with 100% success.
- [x] Asset validation script passes with 0 errors.
- [x] ADR-0006 and Transition Plan are committed to `docs/`.
