using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScreenTextOverlay : MonoBehaviour
    {
        public enum Mode
        {
            none,
            fade,
            scale
        }

        [Space(10)]
        [SerializeField]
        private float startDelay;

        [SerializeField]
        private Mode startMode;

        [HideIf("startMode", Mode.none)]
        [SerializeField]
        private float startLength;

        [Space(10)]
        [SerializeField]
        private float waitLength;

        [Space(10)]
        [SerializeField]
        private Mode endMode;

        [HideIf("endMode", Mode.none)]
        [SerializeField]
        private float endLength;

        [Space(10)]
        [SerializeField]
        private AudioClip clip;

        private TextMeshProUGUI textfield;

        private void Awake()
        {
            this.textfield = this.GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            StartCoroutine(animateCo());
        }

        private IEnumerator animateCo()
        {
            animate(0, startMode, 0);

            yield return new WaitForSeconds(startDelay);

            animate(startLength, startMode, 1);
            AudioUtil.playSoundEffect(this.clip);

            yield return new WaitForSeconds(startLength + waitLength);

            animate(endLength, endMode, 0);
        }

        private void animate(float length, Mode mode, float endValue)
        {
            if (mode == Mode.fade) textfield.DOFade(endValue, length);
            else if (mode == Mode.scale) textfield.transform.DOScale(endValue, length);
        }
    }
}
