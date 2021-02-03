
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class Vector2Event : UnityEvent<Vector2>
    {
    }

    public class Vector2SignalListener : MonoBehaviour
    {
        public Vector2Signal signal;
        public Vector2Event signalEventVector2;

        public void OnSignalRaised(Vector2 vector)
        {
            this.signalEventVector2.Invoke(vector);
        }

        private void OnEnable()
        {
            signal.RegisterListener(this);
        }

        private void OnDisable()
        {
            signal.DeRegisterListener(this);
        }
    }
}