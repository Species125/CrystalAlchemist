using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/GameObjectSignal")]
    public class GameObjectSignal : ScriptableObject
    {
        public List<GameObjectSignalListener> listeners = new List<GameObjectSignalListener>();

        public void Raise(GameObject gameObject)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(gameObject);
            }
        }

        public void RegisterListener(GameObjectSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(GameObjectSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
