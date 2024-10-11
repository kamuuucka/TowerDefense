using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<string, Delegate> EventTable = new Dictionary<string, Delegate>();

    public static void Subscribe<T>(string eventType, Action<T> listener)
    {
        if (!EventTable.ContainsKey(eventType))
        {
            EventTable[eventType] = listener;
        }
        else
        {
            EventTable[eventType] = Delegate.Combine(EventTable[eventType], listener);
        }
    }

    public static void Unsubscribe<T>(string eventType, Action<T> listener)
    {
        if (EventTable.ContainsKey(eventType))
        {
            EventTable[eventType] = Delegate.Remove(EventTable[eventType], listener);
        }
    }

    public static void Publish<T>(string eventType, T arg)
    {
        if (EventTable.ContainsKey(eventType) && EventTable[eventType] is Action<T> callback)
        {
            callback.Invoke(arg);
        }
    }
}