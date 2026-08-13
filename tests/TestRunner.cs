using System;
using System.IO;
using Ashbinders.Core.Architecture;
using Ashbinders.Core.Events;
using Ashbinders.Core.Save;
using Ashbinders.Embers.Core;

namespace Ashbinders.Tests;

public class TestRunner
{
    private static int _passed = 0;
    private static int _failed = 0;

    public static int Main(string[] args)
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("    Ashbinders Core Systems Unit Test Runner     ");
        Console.WriteLine("=================================================\n");

        RunTest("EventBus: Publish & Subscribe", TestEventBusPublishSubscribe);
        RunTest("EventBus: Unsubscribe", TestEventBusUnsubscribe);
        RunTest("StateMachine: Transitions & Lifecycle", TestStateMachineLifecycle);
        RunTest("SaveManager: Serialization & Deserialization Fidelity", TestSavePersistence);

        Console.WriteLine("\n=================================================");
        Console.WriteLine($"Results: {_passed} Passed, {_failed} Failed");
        Console.WriteLine("=================================================");

        return _failed == 0 ? 0 : 1;
    }

    private static void RunTest(string testName, Action testMethod)
    {
        try
        {
            testMethod();
            Console.WriteLine($"  [PASS] {testName}");
            _passed++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [FAIL] {testName}: {ex.Message}");
            _failed++;
        }
    }

    private readonly struct EmberEvent : IGameEvent
    {
        public EmberType Type { get; }
        public EmberEvent(EmberType type) => Type = type;
    }

    private static void TestEventBusPublishSubscribe()
    {
        EventBus.Clear();
        var received = false;
        var receivedType = EmberType.Guard;

        EventBus.Subscribe<EmberEvent>(evt =>
        {
            received = true;
            receivedType = evt.Type;
        });

        EventBus.Publish(new EmberEvent(EmberType.Motion));

        if (!received) throw new Exception("Event was not received by subscriber");
        if (receivedType != EmberType.Motion) throw new Exception($"Expected {EmberType.Motion}, got {receivedType}");
    }

    private static void TestEventBusUnsubscribe()
    {
        EventBus.Clear();
        var count = 0;
        void Handler(EmberEvent evt) => count++;

        EventBus.Subscribe<EmberEvent>(Handler);
        EventBus.Publish(new EmberEvent(EmberType.Forge));
        if (count != 1) throw new Exception($"Expected count 1, got {count}");

        EventBus.Unsubscribe<EmberEvent>(Handler);
        EventBus.Publish(new EmberEvent(EmberType.Forge));
        if (count != 1) throw new Exception($"Expected count 1 after unsubscribe, got {count}");
    }

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

    private static void TestStateMachineLifecycle()
    {
        var fsm = new StateMachine();
        var idle = new IdleState();
        var attack = new AttackState();

        fsm.RegisterState(idle);
        fsm.RegisterState(attack);

        fsm.ChangeState<IdleState>();
        if (!idle.IsEntered) throw new Exception("Idle state was not entered");

        fsm.ChangeState<AttackState>();
        if (!idle.IsExited) throw new Exception("Idle state was not exited");
        if (!attack.IsEntered) throw new Exception("Attack state was not entered");
    }

    private static void TestSavePersistence()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"ashbinders_test_{Guid.NewGuid()}.json");

        try
        {
            var saveManager = new SaveManager();
            saveManager.CurrentSaveData.Player.PositionX = 250.0f;
            saveManager.CurrentSaveData.Player.PositionY = 180.5f;
            saveManager.CurrentSaveData.Player.CurrentHealth = 85;
            saveManager.CurrentSaveData.Player.SocketedEmbers.Add("ember_motion_01");
            saveManager.CurrentSaveData.World.ActivatedDevices.Add("ancient_ember_device_01");

            var saveSuccess = saveManager.SaveToFile(tempFile);
            if (!saveSuccess) throw new Exception("SaveToFile returned false");
            if (!File.Exists(tempFile)) throw new Exception("Save file was not written to disk");

            var reloadedManager = new SaveManager();
            var loadSuccess = reloadedManager.LoadFromFile(tempFile);
            if (!loadSuccess) throw new Exception("LoadFromFile returned false");

            if (reloadedManager.CurrentSaveData.Player.PositionX != 250.0f)
                throw new Exception($"Expected PositionX 250.0, got {reloadedManager.CurrentSaveData.Player.PositionX}");
            if (reloadedManager.CurrentSaveData.Player.CurrentHealth != 85)
                throw new Exception($"Expected CurrentHealth 85, got {reloadedManager.CurrentSaveData.Player.CurrentHealth}");
            if (!reloadedManager.CurrentSaveData.Player.SocketedEmbers.Contains("ember_motion_01"))
                throw new Exception("Socketed embers missing ember_motion_01");
            if (!reloadedManager.CurrentSaveData.World.ActivatedDevices.Contains("ancient_ember_device_01"))
                throw new Exception("Activated devices missing ancient_ember_device_01");
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
