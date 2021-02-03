using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/BoolSignal")]
    public class BoolSignal : ScriptableObject
    {
        public List<BoolSignalListener> listeners = new List<BoolSignalListener>();

        public void Raise(bool value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(value);
            }
        }

        public void RegisterListener(BoolSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(BoolSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}