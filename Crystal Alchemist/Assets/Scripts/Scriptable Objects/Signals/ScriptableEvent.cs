using System;
using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    public abstract class ScriptableEvent<T> : ScriptableObject
    {
        private List<ScriptableEventListener<T>> listeners = new List<ScriptableEventListener<T>>();
        private List<Action<T>> eventListeners = new List<Action<T>>();

        public void Raise(T parameter)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--) eventListeners[i].Invoke(parameter);
            for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnEventRaised(parameter);
        }

        public void RegisterListener(Action<T> listener)
        {
            if (!eventListeners.Contains(listener)) eventListeners.Add(listener);
        }

        public void UnregisterListener(Action<T> listener) => eventListeners.Remove(listener);

        public void RegisterListener(ScriptableEventListener<T> listener)
        {
            if (!listeners.Contains(listener)) listeners.Add(listener);
        }

        public void UnregisterListener(ScriptableEventListener<T> listener)
        {
            if (listeners.Contains(listener)) listeners.Remove(listener);
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() { }
    }

    public abstract class ScriptableEvent : MonoBehaviour
    {
        private List<ScriptableEventListener> listeners = new List<ScriptableEventListener>();
        private List<Action> eventListeners = new List<Action>();

        public void Raise()
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--) eventListeners[i].Invoke();
            for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnEventRaised();
        }

        public void RegisterListener(Action listener)
        {
            if (!eventListeners.Contains(listener)) eventListeners.Add(listener);
        }

        public void UnregisterListener(Action listener) => eventListeners.Remove(listener);

        public void RegisterListener(ScriptableEventListener listener)
        {
            if (!listeners.Contains(listener)) listeners.Add(listener);
        }

        public void UnregisterListener(ScriptableEventListener listener)
        {
            if (listeners.Contains(listener)) listeners.Remove(listener);
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() { }
    }
}