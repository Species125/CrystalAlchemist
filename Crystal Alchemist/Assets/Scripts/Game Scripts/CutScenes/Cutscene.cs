using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField]
        [TextArea]
        private string notes;

        [SerializeField]
        private BoolValue CutSceneValue;

        [InfoBox("OnCompleted wont get called automatically, if false")]
        [SerializeField]
        private bool hasDuration = true;

        [ShowIf("hasDuration")]
        [SerializeField]
        [MinValue(0.1f)]
        private float maxDuration = 10f;

        [SerializeField]
        private UnityEvent OnStart;

        [SerializeField]
        private UnityEvent OnEnd;

        [ButtonGroup]
        public void Play() => Invoke("PlayIt", 0.1f);


        private void OnValidate()
        {
            if (!this.hasDuration) this.maxDuration = 0;
        }

        private void PlayIt()
        {
            float duration = this.maxDuration;

            this.CutSceneValue.SetValue(true);
            GameEvents.current.DoCutScene();
            this.OnStart?.Invoke();

            if (this.hasDuration) Invoke("Completed", duration); //AutoCompleted only when duration > 0
        }

        [ButtonGroup]
        public void Completed()
        {
            this.CutSceneValue.SetValue(false);
            GameEvents.current.DoCutScene();
            this.OnEnd?.Invoke();
        }

        public void RaiseSignal(SimpleSignal signal) => signal?.Raise();
    }
}
