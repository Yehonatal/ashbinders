---
name: game-code-writer
description: >-
  Specialized gameplay and engine programming skill for Godot 4 + C#.
  Use when writing, implementing, or refactoring game logic, character controllers,
  combat systems, state machines, hitboxes, animations, physics, and gameplay mechanics.
---

# Game Code Writer — Godot 4 & C# Gameplay Programming

Specialized guidelines for implementing gameplay systems in Godot 4 using C# (.NET 8).

## 1. Zero-Fluff Standard
- Do not use emojis in comments, logs, or documentation.
- Avoid redundant explanations. Write self-documenting code with clear types.
- Comments must describe rationale, edge cases, and architectural constraints only.

## 2. Core Gameplay Principles
- **Input Response**: Implement input buffering (0.15s buffer for attacks/dashes) and coyote time.
- **Zero-Allocation Hot Loop**: Do not allocate objects (`new`, LINQ, closures, string formatting) in `_Process` or `_PhysicsProcess`. Cache references in `_Ready()`.
- **Deterministic Physics**: Movement, velocity updates, and collisions belong exclusively in `_PhysicsProcess(double delta)`.
- **Composition**: Use node-based components (`HealthComponent`, `Hitbox`, `Hurtbox`, `InteractionDetector`, `EmberSocket`) instead of monolithic inheritance hierarchies.

## 3. State Machine Pattern
```csharp
public interface IState
{
    void Enter();
    void Exit();
    void Update(double delta);
    void PhysicsUpdate(double delta);
    void HandleInput(object? @event = null);
}

public class StateMachine
{
    public IState? CurrentState { get; private set; }
    private readonly Dictionary<Type, IState> _states = new();

    public void RegisterState(IState state) => _states[state.GetType()] = state;

    public void ChangeState<T>() where T : IState
    {
        if (!_states.TryGetValue(typeof(T), out var next))
            throw new InvalidOperationException($"State '{typeof(T).Name}' not registered.");

        CurrentState?.Exit();
        CurrentState = next;
        CurrentState.Enter();
    }

    public void Update(double delta) => CurrentState?.Update(delta);
    public void PhysicsUpdate(double delta) => CurrentState?.PhysicsUpdate(delta);
    public void HandleInput(object? @event = null) => CurrentState?.HandleInput(@event);
}
```

## 4. Combat and Hitbox Standards
- **Physics Layer Separation**:
  - Layer 1: World
  - Layer 2: Player Body
  - Layer 3: Enemy Body
  - Layer 4: Player Hitbox
  - Layer 5: Enemy Hitbox
  - Layer 6: Interactables
- **Damage Pipeline**:
  - Use immutable struct `DamageInfo` containing `Amount`, `DamageType`, `Knockback`, and `Source`.
  - Hitboxes enable monitoring only during active attack frames.
