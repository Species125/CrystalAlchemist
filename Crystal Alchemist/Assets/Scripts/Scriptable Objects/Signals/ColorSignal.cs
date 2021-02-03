using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/ColorSignal")]
    public class ColorSignal : ScriptableObject
    {
        public List<ColorSignalListener> listeners = new List<ColorSignalListener>();

        public void Raise(Color color)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(color);
            }
        }

        public void RegisterListener(ColorSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(ColorSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
