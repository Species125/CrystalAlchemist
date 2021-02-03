using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class ProgressExtension : MonoBehaviour
    {
        [BoxGroup("Required")]
        [Required]
        [SerializeField]
        private PlayerGameProgress playerProgress;

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

        [HorizontalGroup("Updating/Temp")]
        [SerializeField]
        private bool realTimeUpdate = false;

        [HorizontalGroup("Updating/Temp")]
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
            if (doNewEventOnStart && !this.progress.ContainsProgress(this.playerProgress)) onProgressNew?.Invoke();
            if (doExistsEventOnStart && this.progress.ContainsProgress(this.playerProgress)) onProgressExists?.Invoke();

            if (this.realTimeUpdate) InvokeRepeating("Updating", 0, this.interval);
        }

        public void DoProgressEvent()
        {
            if (!this.progress.ContainsProgress(this.playerProgress)) this.onProgressNew?.Invoke();  
            else onProgressExists?.Invoke();            
        }

        public void AddProgress()
        {
            if (!this.progress.ContainsProgress(this.playerProgress)) this.progress.AddProgress(this.playerProgress);
        }

        private void Updating()
        {            
            bool removed = this.playerProgress.Updating(this.progress);
            if (removed) this.onProgressRemoved?.Invoke();
        }
    }
}
