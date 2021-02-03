using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/ActionSignal")]
    public class ActionSignal : ScriptableObject
    {
        public List<ActionSignalListener> listeners = new List<ActionSignalListener>();

        public void Raise(Action action)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(action);
            }
        }

        public void RegisterListener(ActionSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(ActionSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
