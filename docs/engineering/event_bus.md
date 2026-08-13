# Engineering Guide: Event Bus Architecture

## 1. Motivation
Cross-domain coupling is the #1 cause of code rot in large game codebases. For example, when an enemy dies:
- The combat system shouldn't know about UI scoreboards.
- The enemy shouldn't know about the Quest objective tracker.
- The audio engine shouldn't be hardcoded into the enemy script.

An `EventBus` provides typed publish/subscribe mechanics to decouple producers from consumers.

---

## 2. Event Bus Implementation

```csharp
namespace Ashbinders.Core.Events;

public interface IGameEvent { }

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public static void Subscribe<T>(Action<T> handler) where T : IGameEvent
    {
        var type = typeof(T);
        if (!_subscribers.TryGetValue(type, out var list))
        {
            list = new List<Delegate>();
            _subscribers[type] = list;
        }
        list.Add(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
        {
            list.Remove(handler);
        }
    }

    public static void Publish<T>(T @event) where T : IGameEvent
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
        {
            // Snapshot list to allow modification during event dispatch
            var handlers = list.ToArray();
            foreach (var handler in handlers)
            {
                ((Action<T>)handler).Invoke(@event);
            }
        }
    }

    public static void Clear() => _subscribers.Clear();
}
```

---

## 3. Example Usage
```csharp
// Event Definition
public readonly struct EmberExtractedEvent : IGameEvent
{
    public EmberType Type { get; }
    public EmberExtractedEvent(EmberType type) => Type = type;
}

// Publisher (in EmberSource.cs)
EventBus.Publish(new EmberExtractedEvent(EmberType.Motion));

// Subscriber (in QuestManager.cs)
EventBus.Subscribe<EmberExtractedEvent>(OnEmberExtracted);
```
