using System;
using System.Collections.Generic;

namespace Ashbinders.Core.Architecture;

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
        var type = typeof(T);
        if (!_states.TryGetValue(type, out var nextState))
        {
            throw new InvalidOperationException($"State '{type.Name}' is not registered in this StateMachine.");
        }

        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Update(double delta) => CurrentState?.Update(delta);
    public void PhysicsUpdate(double delta) => CurrentState?.PhysicsUpdate(delta);
    public void HandleInput(object? @event = null) => CurrentState?.HandleInput(@event);
}
