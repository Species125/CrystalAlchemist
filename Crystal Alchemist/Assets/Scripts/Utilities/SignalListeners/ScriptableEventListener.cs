
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public abstract class ScriptableEventListener<T> : MonoBehaviour
    {
        protected abstract ScriptableEvent<T> sig { get; }
        protected abstract UnityEvent<T> action { get; }

        private void OnEnable() => sig.RegisterListener(this);

        private void OnDisable() => sig.UnregisterListener(this);

        public void OnEventRaised(T parameter) => action.Invoke(parameter);
    }

    public abstract class ScriptableEventListener : MonoBehaviour
    {
        protected abstract ScriptableEvent sig { get; }
        protected abstract UnityEvent action { get; }

        private void OnEnable() => sig.RegisterListener(this);

        private void OnDisable() => sig.UnregisterListener(this);

        public void OnEventRaised() => action.Invoke();
    }
}