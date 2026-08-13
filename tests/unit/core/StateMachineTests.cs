using Xunit;
using Ashbinders.Core.Architecture;

namespace Ashbinders.Tests.Unit.Core;

public class StateMachineTests
{
    private class IdleState : IState
    {
        public bool IsEntered { get; private set; }
        public bool IsExited { get; private set; }

        public void Enter() => IsEntered = true;
        public void Exit() => IsExited = true;
        public void Update(double delta) { }
        public void PhysicsUpdate(double delta) { }
        public void HandleInput(object? @event = null) { }
    }

    private class AttackState : IState
    {
        public bool IsEntered { get; private set; }
        public bool IsExited { get; private set; }

        public void Enter() => IsEntered = true;
        public void Exit() => IsExited = true;
        public void Update(double delta) { }
        public void PhysicsUpdate(double delta) { }
        public void HandleInput(object? @event = null) { }
    }

    [Fact]
    public void StateMachine_TransitionsCorrectly()
    {
        var fsm = new StateMachine();
        var idle = new IdleState();
        var attack = new AttackState();

        fsm.RegisterState(idle);
        fsm.RegisterState(attack);

        fsm.ChangeState<IdleState>();
        Assert.Same(idle, fsm.CurrentState);
        Assert.True(idle.IsEntered);

        fsm.ChangeState<AttackState>();
        Assert.Same(attack, fsm.CurrentState);
        Assert.True(idle.IsExited);
        Assert.True(attack.IsEntered);
    }
}
