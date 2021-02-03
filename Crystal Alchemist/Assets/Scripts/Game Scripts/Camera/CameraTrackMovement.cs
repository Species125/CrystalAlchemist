
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraTrackMovement : MonoBehaviour
    {
        [SerializeField]
        [MinValue(0.05)]
        private float speed = 0.1f;

        [Required]
        [SerializeField]
        [MinValue(0)]
        [MaxValue(99)]
        private float delay = 1;

        [SerializeField]
        private UnityEvent OnCompleted;

        private CinemachineVirtualCamera vcam;
        private CinemachineTrackedDolly dolly;

        private void OnEnable()
        {
            this.vcam = this.GetComponent<CinemachineVirtualCamera>();
            this.dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
            this.dolly.m_PathPosition = 0;
        }

        private void LateUpdate()
        {
            this.dolly.m_PathPosition += speed * Time.deltaTime;
            if (this.dolly.m_PathPosition >= 1) Invoke("Completed", this.delay);
        }

        public void Completed() => this.OnCompleted?.Invoke();

        public void RaiseSignal(SimpleSignal signal) => signal?.Raise();
    }
}
