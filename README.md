# Ashbinders

> "Power is moved, not created."

Ashbinders is an open-world dark-fantasy RPG where an ember-mage explores the buried, interconnected ruins of the subterranean city of Veyr. By extracting living embers from creatures and ancient machines and placing them into other mechanisms, the player restores forgotten infrastructure, uncovers the truth behind the sacred Heart Ember, and decides whether the power of the past should be preserved, freed, seized, or transformed.

Built with **Godot 4** and **C# (.NET 8.0)**.

---

## 1. Repository Architecture

The repository is organized as a monorepo with domain isolation to scale from prototype to large studio development:

```text
ashbinders/
├── .github/          # Automated CI/CD workflows
├── .agents/          # Antigravity agent skills, rules, and AI development workflows
├── docs/             # Technical docs, architecture ADRs, GDD, and phase roadmaps
├── game/             # The Godot 4 + C# game project root
│   ├── project.godot
│   ├── Ashbinders.csproj
│   ├── core/         # Engine & generic infrastructure (domain-agnostic)
│   ├── gameplay/     # Shared gameplay components (health, damage, interaction)
│   ├── characters/   # Player (Kael), enemies, NPCs, bosses
│   ├── combat/       # Ashbinder chain, weapon heads, hitboxes, parry
│   ├── embers/       # Ember types, sockets, capacity, Ember Tide
│   ├── world/        # 6 Regions, world state, streaming, interactables
│   ├── factions/     # 5 Factions (Council, Cult, Wardens, Archivists, Scavengers)
│   ├── quests/       # Quest runtime, objectives, condition graphs
│   ├── puzzles/      # Ember routing, water, light/dark, memory replays
│   ├── narrative/    # Dialogue, investigations evidence board, endings
│   ├── progression/  # Stats, talent web, gear, capacity unlocks
│   ├── ui/           # HUD, menus, evidence board UI
│   ├── audio/        # Music, sfx, ambience management
│   ├── rendering/    # Shaders, post-processing
│   ├── resources/    # Godot .tres data files
│   ├── assets/       # Sprites, textures, audio, fonts
│   └── tests/        # In-game integration tests
├── tools/            # Internal tools, validators, editors
├── tests/            # Automated unit tests (.NET / xUnit)
├── scripts/          # Automation scripts (build, test, validation)
├── infrastructure/   # Telemetry, backend, and deployment scaffolds
└── config/           # Environment configs (development, staging, production)
```

---

## 2. Setup and Build

### Prerequisites
- .NET 8.0 SDK
- Godot Engine 4.x (.NET version)

### Commands
- Build solution:
  ```bash
  dotnet build game/Ashbinders.csproj
  ```
- Run automated tests:
  ```bash
  ./scripts/run_tests.sh
  ```
- Validate asset naming:
  ```bash
  python3 scripts/validate_assets.py
  ```

---

- [Local Setup and Execution Guide](docs/engineering/local_setup_and_running.md)
- [Phases Roadmap](docs/phases_roadmap.md)
- [Phase 1 Detailed Specification](docs/phase_1_detailed.md)
- [Architecture Decision Records (ADRs)](docs/architecture/ADR/)
- [Coding Standards](.agents/rules/coding_standards.md)
- [Documentation Standards](.agents/rules/documentation_standards.md)
- [Asset Conventions](.agents/rules/asset_and_resource_conventions.md)
