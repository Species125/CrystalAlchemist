
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject>
    {
    }

    public class GameObjectSignalListener : MonoBehaviour
    {
        public GameObjectSignal signal;
        public GameObjectEvent signalEventgameObject;

        public void OnSignalRaised(GameObject gameObject)
        {
            this.signalEventgameObject.Invoke(gameObject);
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