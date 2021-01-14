using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/AudioClipSignal")]
    public class AudioClipSignal : ScriptableObject
    {
        public List<AudioClipSignalListener> listeners = new List<AudioClipSignalListener>();

        public void Raise(AudioClip value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(value);
            }
        }

        public void RegisterListener(AudioClipSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(AudioClipSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
