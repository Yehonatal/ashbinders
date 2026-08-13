# ADR-0004: Asset Taxonomy and Godot Resource Data Architecture

## Status
Accepted

## Context
Ad-hoc asset naming (`enemy_final2.png`, `sword_test.png`) and hardcoding stat values in C# scripts causes chaos when scaling to dozens of artists and designers.

## Decision
1. Enforce strict `snake_case` naming taxonomy with descriptive category prefixes (`kael_idle_down_01.png`, `underlevels_wall_stone_a.png`).
2. Treat Godot `Resource` (`.tres`) files as pure data schemas (`WeaponHeadData.tres`, `EmberData.tres`).
3. Enforce asset naming and placement via automated CI script (`scripts/validate_assets.py`).

## Consequences
- Designers can balance weapons, stats, and drop rates in the Godot inspector without editing or recompiling C# code.
- Assets are consistently indexed, categorized, and searchable.
