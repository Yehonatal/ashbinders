# Phase 1 Detailed Specification — Technical Foundation & Playable Prototype

**Target Duration**: 4–6 weeks  
**Team**: 2 Engineers  
**Engine**: Godot 4.x + C# (.NET 8.0)  
**Primary Goal**: Establish a rock-solid, scalable architecture and build an ultra-responsive, fun playable prototype demonstrating the foundational Ashbinders gameplay loop.

---

## 1. Scope Boundary (What is IN vs OUT)

### Explicitly IN Scope
- Clean monorepo structure with decoupled domain folders.
- CI/CD pipelines for automated compilation, unit testing, and asset checks.
- Kael character controller (8-way movement, acceleration, friction, facing direction, dash).
- Smooth follow camera with bounding box.
- Generic interaction framework (`IInteractable`, `InteractionDetector`).
- Modular Ashbinder Chain weapon framework with functional `BladeHead`.
- One enemy creature with FSM (`Idle`, `Detect`, `Chase`, `Attack`, `Hurt`, `Death`).
- Motion Ember extraction, chain socketing, and dash-attack enhancement.
- Ancient Ember Device accepting Motion Ember to activate world geometry.
- Single Underlevels-inspired test arena connecting combat, extraction, machine activation, and path opening.
- Schema-versioned Save/Load system persisting player stats, socketed embers, and world state.

### Explicitly OUT of Scope
- Full open-world streaming.
- Factions and dialogue trees.
- Quests and Talent Web.
- Complex story cutscenes or final high-fidelity art.
- Additional ember types (Guard, Forge, Bonelight, Memory).
- Boss encounters.

---

## 2. Granular Task Breakdown & Team Allocation

| Task ID | Component | Est. Days | Lead Owner | Deliverable |
| :--- | :--- | :--- | :--- | :--- |
| **1.1** | Project Setup & Architecture | 2 | Engineer A | Godot 4 C# solution, input map, repo standards, base scene |
| **1.2** | Player Movement & Game Feel | 4 | Engineer A | Kael 8-way movement, acceleration, dash, input buffer |
| **1.3** | Camera System | 1 | Engineer A | Smooth tracking, deadzones, boundary clamping |
| **1.4** | Interaction Framework | 2 | Engineer A | `IInteractable` detector, prompt UI, activation triggers |
| **1.5** | Ashbinder Chain (Blade Head) | 4 | Engineer A | Weapon head architecture, attack hitboxes, damage pipeline |
| **1.6** | First Enemy & Combat FSM | 3 | Engineer B | Basic ember creature AI, hitstun, death animation, drop |
| **1.7** | Motion Ember System | 3 | Engineer A | Extraction mechanic, chain socketing, move speed modifier |
| **1.8** | Ember Machine | 2 | Engineer B | `AncientEmberDevice`, power-routing, bridge/gate extension |
| **1.9** | Test Arena Gym | 3 | Engineer B | Underlevels-themed graybox level with combat & puzzle rooms |
| **1.10**| Save/Load System | 2 | Engineer A | JSON persistence for player, embers, and machine states |
| **1.11**| Integration & Playtesting | 4 | Both | End-to-end bug fixing, tuning game feel, definition of done |

---

## 3. Detailed Technical Specifications

### 3.1 Player Controller (Kael)
- **Base Speed**: 180 px/s (Walk), 240 px/s with Motion Ember socketed.
- **Acceleration**: 1400 px/s² (reaches max speed in ~0.13s).
- **Friction / Deceleration**: 1200 px/s² (crisp stop).
- **Dash**:
  - Distance: 120 px.
  - Duration: 0.18s.
  - Invulnerability frames (i-frames): 0.15s.
  - Cooldown: 0.6s.

### 3.2 Ashbinder Chain & Blade Head
- **Reach**: 72 px forward arc.
- **Base Damage**: 15 physical damage.
- **Attack Duration**: 0.32s (Windup: 0.08s, Active Hitbox: 0.12s, Recovery: 0.12s).
- **Hitstop**: 0.05s freeze frame on successful hit to provide punchy impact.

### 3.3 Basic Enemy (Ember Beast)
- **Health**: 40 HP (dies in 3 hits from Blade Head).
- **Detection Radius**: 180 px.
- **Chase Speed**: 120 px/s.
- **Attack Range**: 32 px (Melee swipe dealing 10 damage).
- **Drop**: Drops 1 `MotionEmber` on death.

### 3.4 Ancient Ember Device
- **Required Ember**: `MotionEmber`.
- **Interaction**: Press `E` / Gamepad `X` when in range.
- **Outcome**: Consumes/holds Motion Ember, powers hydraulic gears, and unlocks the gate leading to the Ember Chamber.

---

## 4. Acceptance Criteria & Definition of Done
The Phase 1 milestone is officially complete when:
1. All automated unit tests in `tests/` pass with 100% success.
2. A player can launch the test scene, navigate Kael, defeat the enemy creature using the Ashbinder Chain, extract the Motion Ember, socket it to test enhanced mobility, place it into the Ancient Ember Device to open the gate, enter the next room, save the game, exit, and reload with complete state fidelity.
