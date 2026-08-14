# Ashbinders — Production Phases & Roadmap

This document outlines the phased development roadmap for **Ashbinders**, scaling from a 2-engineer foundation to a full open-world dark-fantasy RPG.

```
┌───────────────────────────────────────────────────────────────────────────┐
│ Phase 1: Technical Foundation & Playable Prototype (Current)              │
│ 2 Engineers • Architecture, Kael Controller, Combat, Ember Loop, Test Gym │
└─────────────────────────────────────┬─────────────────────────────────────┘
                                      │
                                      ▼
┌───────────────────────────────────────────────────────────────────────────┐
│ Phase 2: Underlevels Vertical Slice                                       │
│ 1 Complete Region • Scavenger Faction • 2 Weapon Heads • 1 Vault Dungeon  │
└─────────────────────────────────────┬─────────────────────────────────────┘
                                      │
                                      ▼
┌───────────────────────────────────────────────────────────────────────────┐
│ Phase 3: Core Expansion (Settlement + Drowned Districts + Warrens)        │
│ 3 Factions • Water/Light-Dark Puzzles • Talent Web • Evidence Board Beta  │
└─────────────────────────────────────┬─────────────────────────────────────┘
                                      │
                                      ▼
┌───────────────────────────────────────────────────────────────────────────┐
│ Phase 4: Full Open World (Archive + Furnace Spire + Endings)              │
│ All 6 Regions • All 5 Factions • Hollow Furnace Boss • 4 Endings          │
└─────────────────────────────────────┬─────────────────────────────────────┘
                                      │
                                      ▼
┌───────────────────────────────────────────────────────────────────────────┐
│ Phase 5: Polish, Optimization, Multi-Platform & Launch Readiness          │
│ Full Voiceover • Audio Mastering • Localization • Console Certification   │
└───────────────────────────────────────────────────────────────────────────┘
```

---

## Phase 1 — Technical Foundation & Playable Prototype
- **Team Size**: 2 engineers.
- **Objective**: Establish the scalable monorepo architecture, coding standards, CI/CD, and a tight playable test room demonstrating Kael movement, Ashbinder chain combat with Blade head, enemy defeat, Motion Ember extraction, socketing, machine activation, and save/load.
- **Detailed Spec**: See [Phase 1 Detailed Specification](phase_1_detailed.md) and [Phase 1 to Phase 2 Transition Plan](engineering/phase_1_to_phase_2_transition_plan.md).

---

## Phase 2 — Underlevels Vertical Slice
- **Team Size**: 4–6 developers (2 engineers, 2 artists, 1 designer, 1 narrative).
- **Objective**: Build the first complete, shipping-quality region: The Underlevels.
- **Key Features**:
  - Full Underlevels zone with deep-shaft mining aesthetics.
  - Scavenger Crews faction with rival extraction quests.
  - 2 Weapon Heads: Blade Head and Hammer Head.
  - First Vault Puzzle guarding an Anchor Ember (permanent capacity upgrade).
  - Ember Tide mechanics active across Underlevels sectors.
  - Audio and VFX pass for industrial subterranean ambience.

---

## Phase 3 — Core Expansion (Settlement, Drowned Districts, Bonelight Warrens)
- **Team Size**: 12–20 developers.
- **Key Features**:
  - The Settlement living hub reacting in real-time to routed ember power.
  - Drowned Districts: Map-wide water level manipulation puzzles.
  - Bonelight Warrens: Darkness mechanics that physically reshape map geometry.
  - Factions introduced: Settlement Council, Hollow Cult, Wardens.
  - Talent Web full implementation (Combat, Utility, Reasoning paths).
  - Evidence Board deduction system beta with initial Archivist investigations.

---

## Phase 4 — Full Open World (Sunken Archive, Furnace Spire, Bosses & Endings)
- **Team Size**: 30–50+ developers.
- **Key Features**:
  - Sunken Archive: Memory-ember temporal replay puzzles.
  - Furnace Spire: Monolithic vertical climb and Ash Cathedral revelation.
  - All 5 Faction final quest branches.
  - Final Boss: The Hollow Furnace multi-phase battle.
  - All 4 Endings implemented (Restoration, Release, Ascension, Fusion) with faction-dependent epilogues.

---

## Phase 5 — Polish, Platform Certification & Launch
- **Objective**: Performance profiling (60 FPS on target hardware), localization in 10+ languages, full gamepad support with haptics, steam achievements, accessibility features, and console compliance.
