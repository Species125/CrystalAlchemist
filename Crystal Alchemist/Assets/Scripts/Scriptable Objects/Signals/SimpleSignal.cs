using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/SimpleSignal")]
    public class SimpleSignal : ScriptableObject
    {
        public List<SimpleSignalListener> listeners = new List<SimpleSignalListener>();

        public void Raise()
        {
            List<SimpleSignalListener> activelisteners = new List<SimpleSignalListener>();

            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                if (this.listeners[i] != null)
                {
                    this.listeners[i].OnSignalRaised();
                }
            }
        }

        public void RegisterListener(SimpleSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(SimpleSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}