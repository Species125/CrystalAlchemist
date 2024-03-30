using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class CustomEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent events;

        [Button]
        public void CallEvents()
        {
            this.events.Invoke();
        }
    }
}
