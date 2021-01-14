using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CrystalAlchemist
{
    public class PostProcessingHandler : MonoBehaviour
    {
        [BoxGroup]
        [SerializeField]
        private Volume volume;

        [BoxGroup]
        [SerializeField]
        private float fadingSteps = 0.025f;

        private ColorAdjustments colorGrading;

        private void Start()
        {
            if (MenuEvents.current != null) MenuEvents.current.OnPostProcessingFade += StartFading;
            this.volume.profile.TryGet(out this.colorGrading);
            ResetFading();
        }

        private void OnDestroy()
        {
            if (MenuEvents.current != null) MenuEvents.current.OnPostProcessingFade -= StartFading;
        }

        [Button]
        public void StartFading(Action action)
        {
            StartCoroutine(FadeOut(this.fadingSteps, action));
        }

        private IEnumerator FadeOut(float delay, Action action)
        {
            while (this.colorGrading.saturation.value > -100)
            {
                this.colorGrading.saturation.value -= 1f;
                yield return new WaitForSeconds(delay);
            }

            action?.Invoke();
        }

        [Button]
        private void ResetFading()
        {
            this.colorGrading.saturation.value = 0;
            this.colorGrading.colorFilter.value = Color.white;
        }
    }
}
