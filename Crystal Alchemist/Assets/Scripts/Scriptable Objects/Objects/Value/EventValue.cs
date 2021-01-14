using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Values/EventValue")]
    public class EventValue : ScriptableObject
    {
        [SerializeField]
        private UnityEvent value;

        public UnityEvent GetValue()
        {
            return this.value;
        }

        public void SetValue(UnityEvent value)
        {
            this.value = value;
        }

        public void OnAfterDeserialize() { }

        public void OnBeforeSerialize() { }
    }
}
