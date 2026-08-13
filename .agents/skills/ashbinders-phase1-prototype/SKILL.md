---
name: ashbinders-phase1-prototype
description: >-
  Execution roadmap and Definition of Done for Phase 1 (Playable Prototype) of Ashbinders.
  Use when implementing or verifying Phase 1 features (Kael controller, camera, interaction,
  Ashbinder chain, enemy, Motion Ember, Ancient Ember Device, Underlevels test level, save/load).
---

# Phase 1 Prototype — Technical Foundation & Definition of Done

## 1. Scope & Objective
- **Team**: 2 engineers.
- **Engine**: Godot 4.x + C# (.NET 8).
- **Core Loop**:
  $$\text{Explore} \rightarrow \text{Fight} \rightarrow \text{Extract Ember} \rightarrow \text{Socket Ember} \rightarrow \text{Activate Machine} \rightarrow \text{Open Route} \rightarrow \text{Save/Load}$$

---

## 2. Phase 1 Feature Breakdown

1. **1.1 Project Setup**: Godot 4 + C# repo, input mappings (`move_*`, `attack`, `dash`, `interact`, `socket_ember`).
2. **1.2 Player Controller**: 8-way movement, acceleration/friction, facing direction, dash, crisp controls.
3. **1.3 Camera System**: Smooth target tracking, deadzone, room bounding.
4. **1.4 Interaction System**: Generic `IInteractable` and `InteractionDetector` component.
5. **1.5 Ashbinder Chain Prototype**: Modular weapon head framework, `BladeHead` implementation with hitbox and timing.
6. **1.6 First Enemy**: Finite state machine (`Idle`, `Detect`, `Chase`, `Attack`, `Hurt`, `Death`, `DropEmber`).
7. **1.7 First Ember Prototype**: `MotionEmber`, extraction prompt, socketing into Ashbinder chain.
8. **1.8 First Ember-Powered Object**: `AncientEmberDevice` accepting Motion Ember to activate world geometry (e.g. extend bridge/door).
9. **1.9 Test Environment**: Underlevels-themed test arena containing combat room, machine, locked door, and vault chamber.
10. **1.10 Save/Load Foundation**: Schema-versioned `SaveManager` storing player stats, socketed embers, and world object states.
11. **1.11 Integration**: Full end-to-end playable loop.

---

## 3. Definition of Done Checklist
- [ ] Godot + C# project configured cleanly without warnings.
- [ ] Kael moves smoothly with snappy acceleration and responsive collision.
- [ ] Camera follows Kael with smooth interpolation.
- [ ] Generic interaction detector highlights nearby interactables and executes interactions.
- [ ] Ashbinder chain swings blade head with accurate hitboxes and damage.
- [ ] Enemy chases player, attacks, takes damage, flashes, dies, and drops an ember.
- [ ] Kael can extract the dropped Motion Ember.
- [ ] Kael can socket the Motion Ember into the Ashbinder Chain to gain enhanced dash.
- [ ] Kael can transfer the Motion Ember into the Ancient Ember Device to open a pathway.
- [ ] Game state (player position, health, embers, world objects) can be saved to disk and reloaded.
