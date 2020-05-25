using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem
{
    public class EventManager
    {
        private static readonly Dictionary<CustomEventType,List<Action<object>>> _subscriptions = new Dictionary<CustomEventType, List<Action<object>>>();

        public static void Raise(CustomEventType eventType,object eventArg=null)
        {
            if (!_subscriptions.ContainsKey(eventType))
            {
                Debug.LogWarning($"Trying to raise {eventType} event which has no subscriptions");
                return;
            }
            foreach (var action in _subscriptions[eventType])
            {
                action.Invoke(eventArg);
            }

        }

        public static void RegisterHandler(CustomEventType eventType, Action<object> eventHandler)
        {
            if (!_subscriptions.ContainsKey(eventType))
            {
                _subscriptions.Add(eventType,new List<Action<object>>());
            }
            _subscriptions[eventType].Add(eventHandler);
        }
        public static void UnregisterHandler(CustomEventType eventType, Action<object> eventHandler)
        {
            if (!_subscriptions.ContainsKey(eventType))
            {
               Debug.LogError("Trying to unregister a event that is not subscribed!");
               return;
            }
            _subscriptions[eventType].Remove(eventHandler);
        }

        public static void Dispose()
        {
            _subscriptions.Clear();
        }

   
    }
}