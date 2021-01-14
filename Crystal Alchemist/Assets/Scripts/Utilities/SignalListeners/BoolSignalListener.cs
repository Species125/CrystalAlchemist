
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class Single2 : UnityEvent<bool>
    {
    }

    public class BoolSignalListener : MonoBehaviour
    {
        public BoolSignal signal;
        public Single2 signalEventBool;

        public void OnSignalRaised(bool value)
        {
            this.signalEventBool.Invoke(value);
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