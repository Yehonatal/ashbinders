---
name: godot-csharp-guidelines
description: >-
  Specific programming patterns, conventions, and memory-safety guidelines for Godot 4 + C# (.NET 8).
  Use when writing scripts, configuring csproj dependencies, using Godot source generators,
  handling signals, or serializing custom Godot Resource classes.
---

# Godot 4 & C# Guidelines (.NET 8)

## 1. Class Definitions & Source Generators
- Always inherit from appropriate Godot node types (`Node2D`, `CharacterBody2D`, `Area2D`, `Resource`, etc.).
- Mark classes as `partial` so Godot's C# source generator can bind scripts and signals correctly.

```csharp
using Godot;

namespace Ashbinders.Gameplay.Characters;

[GlobalClass]
public partial class PlayerController : CharacterBody2D
{
    [Export] public float MoveSpeed { get; set; } = 180.0f;
    [Export] public float Acceleration { get; set; } = 1200.0f;
    [Export] public float Friction { get; set; } = 1000.0f;
}
```

---

## 2. Signals & Event Pattern

- Use Godot C# typed signals with `[Signal]` attribute:
```csharp
[Signal]
public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);

[Signal]
public delegate void DiedEventHandler();
```
- Emit using `EmitSignal(SignalName.HealthChanged, currentHealth, maxHealth);`
- Use pure C# events in domain services/managers that don't need Godot editor binding.

---

## 3. Resource-Driven Architecture (`.tres`)
Define custom data containers by inheriting from `Resource` and decorating with `[GlobalClass]`:

```csharp
namespace Ashbinders.Combat.Weapons;

[GlobalClass]
public partial class WeaponHeadData : Resource
{
    [Export] public string HeadId { get; set; } = string.Empty;
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public int BaseDamage { get; set; } = 10;
    [Export] public float AttackCooldown { get; set; } = 0.4f;
    [Export] public float ReachDistance { get; set; } = 64.0f;
}
```

---

## 4. Performance Checklist
1. **Never allocate in `_Process` or `_PhysicsProcess`**.
2. **Cache node lookups** in `_Ready()`.
3. **Use `Vector2.MoveToward`** and `Mathf.MoveToward` for physics transitions.
4. **Use StringName constants** for input actions:
```csharp
public static class InputActions
{
    public static readonly StringName MoveLeft = new("move_left");
    public static readonly StringName MoveRight = new("move_right");
    public static readonly StringName MoveUp = new("move_up");
    public static readonly StringName MoveDown = new("move_down");
    public static readonly StringName Attack = new("attack");
    public static readonly StringName Dash = new("dash");
    public static readonly StringName Interact = new("interact");
    public static readonly StringName SocketEmber = new("socket_ember");
}
```
