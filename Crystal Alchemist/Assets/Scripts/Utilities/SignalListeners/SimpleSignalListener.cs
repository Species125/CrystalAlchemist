
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class SimpleSignalListener : MonoBehaviour
    {
        public SimpleSignal signal;
        public UnityEvent signalEvent;

        [Button]
        public void OnSignalRaised()
        {
            this.signalEvent.Invoke();
        }

        private void OnEnable()
        {
            if (signal != null) signal.RegisterListener(this);
            else Debug.Log(this.gameObject + " - " + this.transform.parent.gameObject);
        }

        private void OnDisable()
        {
            if (signal != null) signal.DeRegisterListener(this);
            else Debug.Log(this.gameObject + " - " + this.transform.parent.gameObject);
        }
    }
}