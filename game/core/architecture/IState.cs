namespace Ashbinders.Core.Architecture;

public interface IState
{
    void Enter();
    void Exit();
    void Update(double delta);
    void PhysicsUpdate(double delta);
    void HandleInput(object? @event = null);
}
