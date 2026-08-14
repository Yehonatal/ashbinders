# Gameplay Specification: Traversal & Movement Systems

## 1. Overview

Traversal in *Ashbinders* combines crisp 2.5D isometric combat mobility with vertical exploration and environmental navigation. Kael possesses three distinct traversal competencies: **Jumping**, **Climbing**, and **Swimming / Wading**.

---

## 2. Granular Mechanics & Parameters

### 2.1 Jumping & Ledge Vaulting
- **Activation**: `Space` (Keyboard) / `A` or `Left Bumper` (Gamepad).
- **Jump Height ($H_{max}$)**: 24 px elevation peak.
- **Jump Duration ($T_{jump}$)**: 0.38s.
- **Horizontal Momentum**: Retains 100% of current ground velocity during leap; cannot reverse direction mid-air, but can perform a mid-air dash or plunging attack.
- **Obstacle Clearance**: Clears ground hazards (ground saws, acid puddles, electrified rails) and low collision barriers (pipes, rubble).
- **Ledge Jump**: Running off a high cliff ($Z = 1$) automatically launches Kael into an extended hop down to the lower floor ($Z = 0$).

### 2.2 Climbing (Ladders, Grates & Scaffolding)
- **Activation**: Approach an industrial ladder or climbing grate and press `W` / `Up` or `S` / `Down`.
- **Climb Speed**: 140 px/s.
- **Controls**:
  - `W` / `S`: Ascend / Descend.
  - `Space`: Eject / Dismount backwards.
  - `Attack`: Quick kick to dislodge pursuing scavengers.
- **Top / Bottom Transition**: Automatically snaps onto the upper landing platform or dismounts cleanly to ground level.

### 2.3 Swimming & Water Traversal
- **Shallow Wading ($Depth < 1.0m$)**:
  - **Move Speed**: 75% of base speed.
  - **Visuals**: Dynamic isometric ripple rings expanding from feet.
  - **Combat**: Unrestricted weapon swings and Ember abilities.
- **Deep Swimming ($Depth \ge 1.0m$)**:
  - **Move Speed**: 60% of base speed (boostable with Guard Ember resonance).
  - **Visuals**: Torso submerged with water surface refraction and continuous wake particles.
  - **Dash in Water**: Converts into a hydrodynamic surge with invulnerability.
  - **Currents**: Water channels apply directional velocity vectors to the player.

---

## 3. Ember Traversal Synergies

| Traversal Action | Motion Ember Synergy | Guard Ember Synergy | Forge Ember Synergy |
| :--- | :--- | :--- | :--- |
| **Jumping** | **Kinetic Vault**: +50% jump distance and explosive landing shockwave | **Anchor Drop**: Heavy slam that crushes armored plates beneath | **Thermal Leap**: Rocket boost over double-width abysses |
| **Climbing** | **Rapid Ascent**: +80% climb speed | **Iron Grip**: Immune to knockback while on ladders | **Superheat Rungs**: Leaves burning wake behind on metal scaffolding |
| **Swimming** | **Water Skim**: Dash across water surfaces without sinking | **Tide Shield**: Complete immunity to toxic currents and whirlpool pulls | **Steam Boiler**: Superheats surrounding water to damage nearby aquatic beasts |
