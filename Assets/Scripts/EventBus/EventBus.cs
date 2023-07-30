using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBus")]
public class EventBus : ScriptableObject
{
    private Dictionary<string, List<CallbackWithPriority>> _signalCallbacks = new Dictionary<string, List<CallbackWithPriority>>();

    public void Subscribe<T>(Action<T> callback, int priority = 0) 
    {
        string key = typeof(T).Name;
        if (_signalCallbacks.ContainsKey(key))
        {
            _signalCallbacks[key].Add(new CallbackWithPriority(priority, callback));
        }
        else 
        {
            _signalCallbacks.Add(key, new List<CallbackWithPriority>() { new CallbackWithPriority(priority, callback) });
        }
        _signalCallbacks[key] = _signalCallbacks[key].OrderByDescending(x => x.Priority).ToList();
    }

    public void Invoke<T>(T signal) 
    {
        string key = typeof(T).Name;
        if (_signalCallbacks.ContainsKey(key))
        {
            foreach (var obj in _signalCallbacks[key])
            {
                var callback = obj.Callback as Action<T>;
                callback?.Invoke(signal);
            }
        }
    }

    public void Unsubscribe<T>(Action<T> callback) 
    {
        string key = typeof(T).Name; 
        if (_signalCallbacks.ContainsKey(key))
        {
            var callbackToDelete = _signalCallbacks[key].FirstOrDefault(x => x.Callback.Equals(callback));
            if (callbackToDelete != null) 
            {
                _signalCallbacks[key].Remove(callbackToDelete);
            }
        }
        else
        {
            Debug.LogError($"Trying to unsubscribe for not existing key! {key}");
        }
    }
}
