# ADR-0002: Monorepo Structure and Domain Ownership

## Status
Accepted

## Context
As game codebases grow to dozens or hundreds of developers, organizing code by technical file type (e.g. `scripts/`, `scenes/`, `textures/`) leads to massive merge conflicts, unclear ownership, and architectural rot. Furthermore, separating tools, docs, and the game into distinct repositories creates version-drift nightmares.

## Decision
1. Adopt a **Monorepo** containing `.github/`, `docs/`, `game/`, `tools/`, `tests/`, `scripts/`, `infrastructure/`, and `config/`.
2. Position the Godot project root at `game/`.
3. Organize `game/` by **Domain Ownership** (`core/`, `gameplay/`, `characters/`, `combat/`, `embers/`, `world/`, `factions/`, `quests/`, `puzzles/`, `narrative/`, `progression/`, `ui/`, `audio/`, `rendering/`, `resources/`, `assets/`).

## Consequences
### Positive
- Changes across gameplay, tools, and docs are atomic in single commits and PRs.
- Feature teams can work autonomously within their domain folder with minimal risk of merge conflicts.
- Code is easy to locate by functional responsibility rather than arbitrary technical type.
