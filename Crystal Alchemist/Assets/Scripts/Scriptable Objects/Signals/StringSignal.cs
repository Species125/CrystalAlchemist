using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/StringSignal")]
    public class StringSignal : ScriptableObject
    {
        public List<StringSignalListener> listeners = new List<StringSignalListener>();

        public void Raise(string text)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(text);
            }
        }

        public void RegisterListener(StringSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(StringSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
