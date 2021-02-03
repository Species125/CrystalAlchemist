using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/Vector2 Signal")]
    public class Vector2Signal : ScriptableObject
    {
        public List<Vector2SignalListener> listeners = new List<Vector2SignalListener>();

        public void Raise(Vector2 vector)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(vector);
            }
        }

        public void RegisterListener(Vector2SignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(Vector2SignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
