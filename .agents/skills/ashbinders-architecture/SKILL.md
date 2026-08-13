---
name: ashbinders-architecture
description: >-
  Comprehensive guide to the Ashbinders monorepo architecture, domain breakdown,
  directory layout, dependency flow, and engineering guidelines.
  Use when creating or organizing files, scenes, scripts, or cross-domain features in Ashbinders.
---

# Ashbinders Architecture — Monorepo & Domain Guide

## 1. Top-Level Repository Structure

The Godot project root is deliberately isolated in `game/` rather than the repository root.

```text
ashbinders/
├── .github/          # CI/CD Workflows (build, test, lint, validate-assets)
├── .agents/          # Agent skills and rules
├── docs/             # Technical and game documentation + ADRs
├── game/             # The Godot 4 + C# Project
│   ├── project.godot
│   ├── Ashbinders.csproj
│   ├── core/         # Generic engine & infrastructure (no game lore)
│   ├── gameplay/     # Shared gameplay behaviors (interaction, damage, health)
│   ├── characters/   # Player (Kael), enemies, NPCs, bosses
│   ├── combat/       # Ashbinder chain, weapon heads, hitboxes, parry
│   ├── embers/       # Ember types, extraction, sockets, capacity, Ember Tide
│   ├── world/        # 6 Regions, world state, streaming, interactables
│   ├── factions/     # 5 Factions (Council, Hollow Cult, Wardens, Archivists, Scavengers)
│   ├── quests/       # Core quest runtime and objective evaluation
│   ├── puzzles/      # Ember routing, water, light/dark, memory puzzles, vaults
│   ├── narrative/    # Dialogue, lore, investigation evidence board, endings
│   ├── progression/  # Core stats, talent web, gear, capacity unlocks
│   ├── ui/           # HUD, menus, evidence board, dialogue UI
│   ├── audio/        # Music, sfx, ambience management
│   ├── rendering/    # Shaders, post-processing
│   ├── resources/    # Godot .tres data files
│   ├── assets/       # Sprites, textures, audio, fonts
│   └── tests/        # In-game integration tests
├── tools/            # Asset pipeline, level tools, validators, editors
├── tests/            # Automated unit tests (.NET / xUnit)
├── scripts/          # Automation scripts (build, test, asset validation)
├── infrastructure/   # Backend / telemetry / deployment scaffolds
└── config/           # Environment configs (development, staging, production)
```

---

## 2. Domain Dependency Rules

To prevent spaghetti architecture, dependencies must obey strict directional flow:

```
[UI / Quests / Narrative]
       ↓
[World / Regions / Factions / Puzzles]
       ↓
[Characters / Combat / Embers / Progression]
       ↓
[Gameplay Components (Health, Damage, Interaction)]
       ↓
[Core Infrastructure (EventBus, StateMachine, SaveManager, Logging)]
```

### Critical Rules
1. `core/` MUST NEVER import or know about `embers`, `combat`, `characters`, or `factions`.
2. `world/regions/<region_name>` is an autonomous module. An engineer working in `underlevels` should not edit `furnace_spire` files.
3. Cross-domain notifications MUST go through `EventBus` (`core/events/EventBus.cs`).
