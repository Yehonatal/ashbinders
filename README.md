# Ashbinders (Open-World Dark-Fantasy RPG)

> **"Power is moved, not created."**

**Ashbinders** is an open-world dark-fantasy RPG where an ember-mage explores the buried, interconnected ruins of the dying subterranean city of Veyr. By extracting living embers from creatures and ancient machines and placing them into other objects, the player restores forgotten mechanisms, uncovers the truth behind the sacred Heart Ember, and ultimately decides whether the power of the past should be preserved, freed, seized, or quietly transformed.

Built with **Godot 4** and **C# (.NET 8.0)**.

---

## 🏛️ Repository Architecture

This repository is structured as a scalable monorepo designed to support a studio expanding from 2 to 200+ engineers without architectural debt:

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

## 🚀 Quickstart & Setup

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Godot Engine 4.x (.NET version)](https://godotengine.org/download)

### Building & Running
1. Clone repository:
   ```bash
   git clone https://github.com/your-org/ashbinders.git
   cd ashbinders
   ```
2. Build C# solution:
   ```bash
   dotnet build game/Ashbinders.csproj
   ```
3. Run automated tests:
   ```bash
   dotnet test tests/Ashbinders.Tests.csproj
   ```
4. Open the `game/` folder in Godot Engine 4 (.NET build) and press **F5** to run.

---

## 🗺️ Production Phases

- **[Phases Overview](docs/phases_roadmap.md)**: Studio production roadmap spanning all phases from prototype to launch.
- **[Phase 1 Detailed Specification](docs/phase_1_detailed.md)**: Tactical 2-engineer foundation & playable prototype roadmap.

---

## 📖 Key Documentation
- **[Architecture Decision Records (ADRs)](docs/architecture/ADR/)**
- **[Game Design Document & Bible](docs/gameplay/ember_system.md)**
- **[Coding Standards](.agents/rules/coding_standards.md)**
- **[Asset Naming Conventions](.agents/rules/asset_and_resource_conventions.md)**
