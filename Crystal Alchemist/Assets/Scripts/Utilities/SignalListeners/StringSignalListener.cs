
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class Single : UnityEvent<string>
    {
    }

    public class StringSignalListener : MonoBehaviour
    {
        public StringSignal signal;
        public Single signalEventString;

        public void OnSignalRaised(string text)
        {
            this.signalEventString.Invoke(text);
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