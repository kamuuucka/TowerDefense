using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<string, Delegate> EventTable = new Dictionary<string, Delegate>();

    /// <summary>
    /// Subscribe with one argument.
    /// </summary>
    /// <param name="eventType">Name of the event.</param>
    /// <param name="listener">Method that will be called on the event.</param>
    /// <typeparam name="T">The type of the argument.</typeparam>
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
    
    /// <summary>
    /// Subscribe without any arguments.
    /// </summary>
    /// <param name="eventType">Name of the event.</param>
    /// <param name="listener">Method that will be called on the event.</param>
    public static void Subscribe(string eventType, Action listener)
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

    /// <summary>
    /// Unsubscribe with one argument.
    /// </summary>
    /// <param name="eventType">Name of the event.</param>
    /// <param name="listener">Method that will be called on the event.</param>
    /// <typeparam name="T">The type of the argument.</typeparam>
    public static void Unsubscribe<T>(string eventType, Action<T> listener)
    {
        if (EventTable.ContainsKey(eventType))
        {
            EventTable[eventType] = Delegate.Remove(EventTable[eventType], listener);
        }
    }

    /// <summary>
    /// Unsubscribe without any arguments.
    /// </summary>
    /// <param name="eventType">Name of the event.</param>
    /// <param name="listener">Method that will be called on the event.</param>
    public static void Unsubscribe(string eventType, Action listener)
    {
        if (EventTable.ContainsKey(eventType))
        {
            EventTable[eventType] = Delegate.Remove(EventTable[eventType], listener);
        }
    }

    /// <summary>
    /// Publish an event with one argument.
    /// </summary>
    /// <param name="eventType">Name of the event.</param>
    /// <param name="arg">Type of the argument.</param>
    public static void Publish<T>(string eventType, T arg)
    {
        if (EventTable.ContainsKey(eventType) && EventTable[eventType] is Action<T> callback)
        {
            callback.Invoke(arg);
        }
    }
    
    /// <summary>
    /// Publish an event without any arguments.
    /// </summary>
    /// <param name="eventType">Name of the event.</param>
    public static void Publish(string eventType)
    {
        if (EventTable.ContainsKey(eventType) && EventTable[eventType] is Action callback)
        {
            callback.Invoke();
        }
    }
}