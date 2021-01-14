
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class SpriteEvent : UnityEvent<Sprite>
    {
    }

    public class SpriteSignalListener : MonoBehaviour
    {
        public SpriteSignal signal;
        public SpriteEvent signalEventString;

        public void OnSignalRaised(Sprite sprite)
        {
            this.signalEventString.Invoke(sprite);
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