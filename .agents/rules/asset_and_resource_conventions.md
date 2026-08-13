# Asset and Resource Naming Conventions

## 1. Directory Structure for Assets
Assets in `game/assets/` must strictly follow domain-based subdirectories:
```text
game/assets/
├── characters/
│   ├── kael/
│   ├── npcs/
│   └── enemies/
├── environments/
│   ├── settlement/
│   ├── underlevels/
│   ├── drowned_districts/
│   ├── bonelight_warrens/
│   ├── sunken_archive/
│   └── furnace_spire/
├── weapons/
├── armor/
├── embers/
├── props/
├── vfx/
├── ui/
├── audio/
└── icons/
```

## 2. File Naming Rules
- **All lowercase with underscores** (`snake_case`).
- **No spaces or special characters**.
- **Descriptive prefixes and suffixes**:
  - `kael_idle_down_01.png`
  - `underlevels_wall_stone_a.png`
  - `ember_motion_idle.png`
  - `weapon_chain_blade_attack_01.png`
  - `sfx_ember_extract.wav`
  - `bgm_underlevels_ambient.ogg`

## 3. Godot Resources (`.tres`)
All static configuration and data live in `game/resources/` using `.tres` files:
- `resources/embers/motion_ember.tres`
- `resources/weapons/blade_head.tres`
- `resources/enemies/ember_beast.tres`
- `resources/quests/scavenger_race_01.tres`
