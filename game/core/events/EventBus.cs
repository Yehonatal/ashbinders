using System;
using System.Collections.Generic;

namespace Ashbinders.Core.Events;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> Subscribers = new();

    public static void Subscribe<T>(Action<T> handler) where T : IGameEvent
    {
        var type = typeof(T);
        if (!Subscribers.TryGetValue(type, out var list))
        {
            list = new List<Delegate>();
            Subscribers[type] = list;
        }
        list.Add(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
    {
        var type = typeof(T);
        if (Subscribers.TryGetValue(type, out var list))
        {
            list.Remove(handler);
        }
    }

    public static void Publish<T>(T @event) where T : IGameEvent
    {
        var type = typeof(T);
        if (Subscribers.TryGetValue(type, out var list))
        {
            var handlers = list.ToArray();
            foreach (var handler in handlers)
            {
                ((Action<T>)handler).Invoke(@event);
            }
        }
    }

    public static void Clear()
    {
        Subscribers.Clear();
    }
}
