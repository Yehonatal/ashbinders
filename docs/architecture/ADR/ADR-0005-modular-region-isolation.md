# ADR-0005: Modular Region Isolation

## Status
Accepted

## Context
Ashbinders features 6 massive subterranean regions (`settlement`, `underlevels`, `drowned_districts`, `bonelight_warrens`, `sunken_archive`, `furnace_spire`). If region files are intermingled or tightly coupled, multiple level designers and environment artists will constantly step on each other's toes.

## Decision
Each region inside `game/world/regions/<region_name>/` is a self-contained module containing its own scenes, local scripts, puzzle prefabs, level data, and audio configs. Cross-region communication occurs exclusively through the global `WorldState` manager and `EventBus`.

## Consequences
- A designer working on `underlevels` never touches `furnace_spire` files.
- Regions can be loaded, tested, and benchmarked in complete isolation.
