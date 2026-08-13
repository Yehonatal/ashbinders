# Architecture Boundaries & Domain Rules

## 1. Domain Separation
The repository is split into distinct domain folders inside `game/`:
- `core/`: Generic engine & framework utilities. Contains **NO** Ashbinders game lore or entity definitions.
- `gameplay/`: Reusable gameplay components (damage, health, interaction, movement, targeting).
- `combat/`: Weapons, combat state machines, hitboxes, combo logic, parry frames.
- `embers/`: Ember mechanics, sockets, capacity, types, extraction, transfer.
- `characters/`: Player (Kael), enemies, NPCs, bosses.
- `world/`: World state, regions, streaming, environment, interactables, puzzles.
- `factions/`: Faction definitions, reputation, specific quest branches.
- `quests/`: Quest state machine, condition evaluation, objective tracking.
- `puzzles/`: Puzzle mechanics (water levels, light/dark, memory replays, vaults).
- `narrative/`: Dialogue graphs, evidence board, lore records, endings.
- `progression/`: RPG stats, talent web, gear stats, capacity upgrades.
- `ui/`: User interface scenes, HUD, menus, evidence board UI.
- `audio/`: Sound effects, music controllers, ambience triggers.
- `rendering/`: Shaders, post-processing, visual effects.
- `resources/`: Pure data files (`.tres`).
- `assets/`: Raw art, audio, and font files.

## 2. Dependency Flow
Dependencies flow strictly in one direction:
`UI / Quests / Narrative` $\rightarrow$ `World / Factions / Puzzles` $\rightarrow$ `Characters / Combat / Embers / Progression` $\rightarrow$ `Gameplay` $\rightarrow$ `Core`.

### Violations to Prevent
- **Never** allow `core/` to import `Ashbinders.Combat` or `Ashbinders.Embers`.
- **Never** allow circular dependencies between domains.
- **Never** allow one region to hardcode direct node references to another region's internals.
- **Always** use `EventBus` for cross-domain notifications.
