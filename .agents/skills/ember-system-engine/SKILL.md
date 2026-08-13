---
name: ember-system-engine
description: >-
  Architectural and mechanical specification for the Ember resource system in Ashbinders.
  Use when writing or modifying Ember extraction, Ember socketing, capacity tracking,
  Ember types (Motion, Guard, Forge, Bonelight, Memory), routing networks, and the Ember Tide.
---

# Ember System Engine — Architecture & Mechanics

## 1. The Core Ember Principle
"Power is moved, not created."

In Ashbinders, Embers are a zero-sum, movable energy currency representing ancient fragmented consciousness.
An ember placed into a weapon for combat CANNOT simultaneously power a door or activate an ancient elevator.

```
       [Ember Source] (Enemy Drop / Machine)
             │
             ▼ (Extract)
     [Kael's Ashbinder Chain] ◄──► [Ember Sockets] (Combat Boosts)
             │
             ▼ (Insert / Route)
    [Ancient World Machine] ───► Opens Path / Alters Region
```

---

## 2. The Ember Types

| Ember Type | Primary Color | Combat Effect | World / Puzzle Utility |
| :--- | :--- | :--- | :--- |
| **Motion** | Amber / Gold | Dash attacks & +30% move speed | Powers hydraulic pistons, gears, elevators |
| **Guard** | Deep Blue | Timed parry window & barrier | Powers protective shields & water gates |
| **Forge** | Crimson Red | Fire damage & armor melt | Thaws frozen mechanisms & smelters |
| **Bonelight** | Pale Green | Lifesteal & critical hits | Illuminates Bonelight Warrens & dark paths |
| **Memory** | Ethereal Violet | Temporal clone & dodge illusion | Replays historical events in Sunken Archive |

---

## 3. Ember System C# Architecture

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

public class Ember
{
    public string Id { get; }
    public EmberType Type { get; }
    public string Name { get; }
    public string Description { get; }

    public Ember(string id, EmberType type, string name, string description)
    {
        Id = id;
        Type = type;
        Name = name;
        Description = description;
    }
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

---

## 4. Ember Capacity & The Ember Tide
- **Ember Capacity**: Replaces traditional XP levels. Kael starts with Capacity = 1. Defeating Vault puzzles grants Anchor Embers, expanding total capacity up to 5.
- **Ember Tide**: Global simulation loop. Regions transition between High Tide (surging machinery, aggressive creatures, higher extraction yield) and Low Tide (dormant machines, quiet exploration).
