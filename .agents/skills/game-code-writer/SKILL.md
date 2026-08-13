---
name: game-code-writer
description: >-
  Specialized gameplay and engine programming skill for Godot 4 + C#.
  Use when writing, implementing, or refactoring game logic, character controllers,
  combat systems, state machines, hitboxes, animations, physics, and gameplay mechanics.
---

# Game Code Writer — Godot 4 & C# Gameplay Programming Skill

This skill guides the implementation of high-performance, robust, and responsive gameplay systems in Godot 4 using C# (.NET 8).

---

## 1. Core Principles of Gameplay Code

1. **Game Feel is King**:
   - Always implement responsive input processing.
   - Use input buffering (e.g., 0.15s buffer for attacks/dashes) and coyote time (for dodge windows/actions).
   - Ensure snappy acceleration and deceleration curves using `Mathf.MoveToward` or lerp with delta-time compensation.
   - Add micro-feedback: hit-stop (frame freeze), screen shake, particle sparks, and sound triggers on impact.

2. **Zero-Allocation in the Hot Loop**:
   - Never allocate objects (`new List<T>()`, `new Class()`, string concatenations, LINQ) inside `_Process` or `_PhysicsProcess`.
   - Pre-allocate arrays, reuse collections with `.Clear()`, and cache node references in `_Ready()`.
   - Use structs for lightweight, immutable data packages (e.g., `DamageInfo`, `HitResult`).

3. **Deterministic Physics & Separation of Concerns**:
   - All physics calculations, movement, velocity updates, and collisions belong in `_PhysicsProcess(double delta)`.
   - Visual interpolation, camera tracking smoothing, and UI updates belong in `_Process(double delta)`.

4. **Composition Over Deep Inheritance**:
   - Prefer Node-based component architecture:
     - `HealthComponent`
     - `HitboxComponent` / `HurtboxComponent`
     - `InteractionDetectorComponent`
     - `EmberSocketComponent`
   - Entities (Kael, Enemies, Machines) assemble these components rather than inheriting from a massive monolithic `LivingEntity` class.

---

## 2. State Machine Architecture

Every dynamic character or interactive machine must use a clean, decoupled finite state machine (FSM).

### State Interface
```csharp
public interface IState
{
    void Enter();
    void Exit();
    void Update(double delta);
    void PhysicsUpdate(double delta);
    void HandleInput(InputEvent @event);
}
```

### State Machine Implementation
```csharp
public class StateMachine
{
    public IState? CurrentState { get; private set; }
    private readonly Dictionary<Type, IState> _states = new();

    public void RegisterState(IState state)
    {
        _states[state.GetType()] = state;
    }

    public void ChangeState<T>() where T : IState
    {
        if (!_states.TryGetValue(typeof(T), out var newState))
            throw new KeyNotFoundException($"State {typeof(T).Name} not registered.");

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update(double delta) => CurrentState?.Update(delta);
    public void PhysicsUpdate(double delta) => CurrentState?.PhysicsUpdate(delta);
    public void HandleInput(InputEvent @event) => CurrentState?.HandleInput(@event);
}
```

---

## 3. Combat & Hitbox Conventions

1. **Area2D Layer Separation**:
   - **Layer 1**: World / Obstacles
   - **Layer 2**: Player Body
   - **Layer 3**: Enemy Body
   - **Layer 4**: Player Hitbox (damages enemies)
   - **Layer 5**: Enemy Hitbox (damages player)
   - **Layer 6**: Interactables / Ember Triggers

2. **Damage Pipeline**:
   ```csharp
   public enum DamageType
   {
       Physical,
       ForgeFire,
       MotionImpact,
       BonelightDrain,
       Environmental
   }

   public readonly struct DamageInfo
   {
       public int Amount { get; }
       public DamageType Type { get; }
       public Vector2 Knockback { get; }
       public Node2D? Source { get; }

       public DamageInfo(int amount, DamageType type, Vector2 knockback, Node2D? source = null)
       {
           Amount = amount;
           Type = type;
           Knockback = knockback;
           Source = source;
       }
   }

   public interface IDamageable
   {
       void TakeDamage(DamageInfo damage);
   }
   ```

3. **Hitbox Execution**:
   - The attacking weapon/enemy enables its `HitboxComponent` shape only during active attack frames.
   - When overlapping a `HurtboxComponent`, the Hurtbox calls `TakeDamage` on its parent `IDamageable`.

---

## 4. Godot C# Node Lifecycle Guidelines

- **Always check node validity** if referencing nodes dynamically using `GodotObject.IsInstanceValid(node)`.
- **Disconnect Signals in `_ExitTree()`** if connected via C# delegate handlers to avoid memory leaks.
- **Export Variables Cleanly**: Use `[Export]` with appropriate types and default values for game designer tuning.
- **Use StringNames** for repeated actions or animation names (`StringName.Create("idle")`) to avoid string allocation overhead.
