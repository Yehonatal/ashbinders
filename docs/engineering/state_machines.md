# Engineering Guide: Finite State Machines (FSM)

## 1. Architectural Philosophy
Finite state machines govern character controllers (Player, Enemies, Bosses) and interactive mechanisms (Doors, Elevators, Puzzles) to eliminate brittle boolean flag matrices (`isAttacking`, `isDashing`, `canMove`).

---

## 2. Core Interfaces & Implementations

```csharp
namespace Ashbinders.Core.Architecture;

public interface IState
{
    void Enter();
    void Exit();
    void Update(double delta);
    void PhysicsUpdate(double delta);
    void HandleInput(Godot.InputEvent @event);
}

public class StateMachine
{
    public IState? CurrentState { get; private set; }
    private readonly Dictionary<Type, IState> _states = new();

    public void RegisterState(IState state) => _states[state.GetType()] = state;

    public void ChangeState<T>() where T : IState
    {
        if (!_states.TryGetValue(typeof(T), out var nextState))
            throw new InvalidOperationException($"State '{typeof(T).Name}' is not registered.");

        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Update(double delta) => CurrentState?.Update(delta);
    public void PhysicsUpdate(double delta) => CurrentState?.PhysicsUpdate(delta);
    public void HandleInput(Godot.InputEvent @event) => CurrentState?.HandleInput(@event);
}
```

---

## 3. Best Practices
- Keep states self-contained. Transitions should only occur in response to explicit conditions or triggers.
- Clean up any active timers, audio loops, or hitbox shapes inside `Exit()`.
