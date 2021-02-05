using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ProgressHandler : MonoBehaviour
    {
        [BoxGroup("Updating")]
        [SerializeField]
        private PlayerGameProgress playerProgress;

        [BoxGroup("Updating")]
        [SerializeField]
        private bool realTimeUpdate = false;

        [BoxGroup("Updating")]
        [ShowIf("realTimeUpdate")]
        [SerializeField]
        private float interval = 1f;

        private void Awake()
        {
            GameEvents.current.OnProgressExists += HasProgress;
            GameEvents.current.OnProgress += AddProgress;
        }

        private void Start()
        {
            if (this.realTimeUpdate) InvokeRepeating("Updating", 0, this.interval);
        }

        private void AddProgress(ProgressValue value)
        {
            this.playerProgress.AddProgress(value);
        }

        private bool HasProgress(ProgressValue value)
        {            
            return this.playerProgress.Contains(value);
        }

        private void Updating()
        {
            this.playerProgress.Updating();            
        }

        private void OnDestroy()
        {
            GameEvents.current.OnProgressExists -= HasProgress;
            GameEvents.current.OnProgress -= AddProgress;
        }
    }
}
