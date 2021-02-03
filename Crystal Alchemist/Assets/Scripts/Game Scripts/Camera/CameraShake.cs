
using Cinemachine;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShake : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCamera;
        private float strength;
        private float speed = 1f;
        private float elapsed;
        private bool isRunning = false;
        private float value;
        private CinemachineBasicMultiChannelPerlin noise;

        private void Start()
        {
            GameEvents.current.OnCameraShake += ActivateShake;
            GameEvents.current.OnCameraStill += DeactivateShake;

            this.virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
            if (this.virtualCamera != null) this.noise = this.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void OnDestroy()
        {
            GameEvents.current.OnCameraShake -= ActivateShake;
            GameEvents.current.OnCameraStill -= DeactivateShake;
        }

        private void Update()
        {
            if (this.noise == null || !this.isRunning) return;

            if (this.elapsed > 0)
            {
                this.elapsed -= Time.deltaTime;
                if (this.value < this.strength) this.value += (Time.deltaTime * this.speed);
                else if (this.value > this.strength) this.value = this.strength;
                noise.m_AmplitudeGain = this.value;
                noise.m_FrequencyGain = 1f;
            }
            else
            {
                this.elapsed = 0;

                if (this.value > 0) this.value -= (Time.deltaTime * this.speed);
                else
                {
                    this.value = 0;
                    this.isRunning = false;
                }
                this.noise.m_AmplitudeGain = this.value;
            }
        }

        private void ActivateShake(float strength, float duration, float speed)
        {
            this.elapsed = duration;
            this.strength = strength;
            this.speed = speed;
            this.isRunning = true;
        }

        private void DeactivateShake(float speed)
        {
            this.elapsed = 0;
            this.speed = speed;
        }
    }
}
