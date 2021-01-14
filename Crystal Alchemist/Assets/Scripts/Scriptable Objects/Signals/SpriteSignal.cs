using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/SpriteSignal")]
    public class SpriteSignal : ScriptableObject
    {
        public List<SpriteSignalListener> listeners = new List<SpriteSignalListener>();

        public void Raise(Sprite sprite)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(sprite);
            }
        }

        public void RegisterListener(SpriteSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(SpriteSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
