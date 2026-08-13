using Xunit;
using Ashbinders.Core.Events;
using Ashbinders.Embers.Core;

namespace Ashbinders.Tests.Unit.Core;

public class EventBusTests
{
    private readonly struct TestEmberEvent : IGameEvent
    {
        public EmberType Type { get; }
        public TestEmberEvent(EmberType type) => Type = type;
    }

    [Fact]
    public void EventBus_Publish_DispatchesToSubscribers()
    {
        EventBus.Clear();
        var received = false;
        var receivedType = EmberType.Guard;

        EventBus.Subscribe<TestEmberEvent>(evt =>
        {
            received = true;
            receivedType = evt.Type;
        });

        EventBus.Publish(new TestEmberEvent(EmberType.Motion));

        Assert.True(received);
        Assert.Equal(EmberType.Motion, receivedType);
    }

    [Fact]
    public void EventBus_Unsubscribe_StopsReceivingEvents()
    {
        EventBus.Clear();
        var count = 0;
        void Handler(TestEmberEvent evt) => count++;

        EventBus.Subscribe<TestEmberEvent>(Handler);
        EventBus.Publish(new TestEmberEvent(EmberType.Forge));
        Assert.Equal(1, count);

        EventBus.Unsubscribe<TestEmberEvent>(Handler);
        EventBus.Publish(new TestEmberEvent(EmberType.Forge));
        Assert.Equal(1, count);
    }
}
