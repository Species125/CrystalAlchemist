
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class ColorEvent : UnityEvent<Color>
    {
    }

    public class ColorSignalListener : MonoBehaviour
    {
        public ColorSignal signal;
        public ColorEvent signalEventColor;

        public void OnSignalRaised(Color color)
        {
            this.signalEventColor.Invoke(color);
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