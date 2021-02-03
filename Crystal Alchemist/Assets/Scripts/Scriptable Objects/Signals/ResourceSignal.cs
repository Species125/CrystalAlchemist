using System.Collections.Generic;


using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Signals/ResourceSignal")]
    public class ResourceSignal : ScriptableObject
    {
        public List<ResourceSignalListener> listeners = new List<ResourceSignalListener>();

        public void Raise(CostType type, float amount)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                this.listeners[i].OnSignalRaised(type, amount);
            }
        }

        public void RegisterListener(ResourceSignalListener listener)
        {
            this.listeners.Add(listener);
        }

        public void DeRegisterListener(ResourceSignalListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
