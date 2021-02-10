using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class ProgressExtension : MonoBehaviour
    {
        [BoxGroup("Required")]
        [HideLabel]
        [Space(10)]
        [SerializeField]
        private ProgressValue progress;

        [BoxGroup("New Progress")]
        [SerializeField]
        private bool doNewEventOnStart = false;

        [BoxGroup("New Progress")]
        [SerializeField]
        [Space(10)]
        private UnityEvent onProgressNew;

        [BoxGroup("Exists Already")]
        [SerializeField]
        private bool doExistsEventOnStart = false;

        [BoxGroup("Exists Already")]
        [SerializeField]
        [Space(10)]
        private UnityEvent onProgressExists;

        [BoxGroup("Updating")]
        [SerializeField]
        private bool realTimeUpdate = false;

        [BoxGroup("Updating")]
        [ShowIf("realTimeUpdate")]
        [SerializeField]
        private float interval = 1f;

        [BoxGroup("Updating")]
        [ShowIf("realTimeUpdate")]
        [Space(10)]
        [SerializeField]
        private UnityEvent onProgressRemoved;

        private void Start()
        {
            if (doNewEventOnStart && !GameEvents.current.HasProgress(this.progress)) onProgressNew?.Invoke();
            if (doExistsEventOnStart && GameEvents.current.HasProgress(this.progress)) onProgressExists?.Invoke();

            if (this.realTimeUpdate) InvokeRepeating("Updating", 0, this.interval);
        }

        public void DoProgressEvent()
        {
            if (!GameEvents.current.HasProgress(this.progress)) this.onProgressNew?.Invoke();  
            else onProgressExists?.Invoke();            
        }

        public void AddProgress()
        {
            GameEvents.current.DoProgress(this.progress);
        }

        private void Updating()
        {
            if (!GameEvents.current.HasProgress(this.progress)) 
                this.onProgressRemoved?.Invoke();
        }
    }
}
