---
name: ember-system-engine
description: >-
  Architectural and mechanical specification for the Ember resource system in Ashbinders.
  Use when writing or modifying Ember extraction, Ember socketing, capacity tracking,
  Ember types (Motion, Guard, Forge, Bonelight, Memory), routing networks, and the Ember Tide.
---

# Ember System Engine — Architecture & Mechanics

## 1. Core Principle
"Power is moved, not created."

Embers represent finite, movable energy units. An ember assigned to a weapon socket cannot simultaneously power an environmental machine or door conduit.

```text
[Ember Source] (Enemy Drop / Machine)
      |
      v (Extract)
[Ashbinder Chain] <---> [Ember Sockets] (Combat Boosts)
      |
      v (Insert)
[Ancient Machine] ----> Opens Path / Alters World
```

## 2. Ember Types

| Ember Type | Primary Color | Combat Effect | World / Puzzle Utility |
| :--- | :--- | :--- | :--- |
| **Motion** | Amber / Gold | Dash attacks, +30% move speed | Powers hydraulic pistons, gears, elevators |
| **Guard** | Deep Blue | Timed parry window & barrier | Powers protective shields & water gates |
| **Forge** | Crimson Red | Fire damage & armor melt | Thaws frozen mechanisms & smelters |
| **Bonelight** | Pale Green | Lifesteal & critical hits | Illuminates Bonelight Warrens & dark paths |
| **Memory** | Ethereal Violet | Temporal clone & dodge illusion | Replays historical events in Sunken Archive |

## 3. C# Interface Contracts
```csharp
public enum EmberType
{
    Motion,
    Guard,
    Forge,
    Bonelight,
    Memory,
    HeartFragment
}

public interface IEmberReceiver
{
    bool AcceptsEmberType(EmberType type);
    bool TryInsertEmber(Ember ember);
    Ember? TryExtractEmber();
    bool HasEmber { get; }
    Ember? CurrentEmber { get; }
}
```

## 4. Ember Capacity and Ember Tide
- **Ember Capacity**: Replaces level progression. Kael starts with Capacity = 1. Anchor Embers reward permanent upgrades up to 5 slots.
- **Ember Tide**: Global simulation loop shifting regions between High Tide (active machines, aggressive creatures, higher yields) and Low Tide (dormant machines, lore exploration).
