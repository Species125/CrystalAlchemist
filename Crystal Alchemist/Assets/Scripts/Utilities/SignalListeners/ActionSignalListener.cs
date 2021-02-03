using System;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class ActionEvent : UnityEvent<Action>
    {
    }

    public class ActionSignalListener : MonoBehaviour
    {
        public ActionSignal signal;
        public ActionEvent signalEventAction;

        public void OnSignalRaised(Action action)
        {
            this.signalEventAction.Invoke(action);
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