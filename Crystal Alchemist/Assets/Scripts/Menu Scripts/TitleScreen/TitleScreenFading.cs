using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Image))]
    public class TitleScreenFading : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 1)]
        private float startValue = 0f;

        [SerializeField]
        [Range(0, 1)]
        private float endValue = 1f;

        [SerializeField]
        [Range(0, 60)]
        private float duration;

        [SerializeField]
        private UnityEvent OnAfterFade;

        private void Start() => StartCoroutine(fadeCo());

        private IEnumerator fadeCo()
        {
            this.GetComponent<Image>().DOFade(startValue, 0);
            this.GetComponent<Image>().DOFade(endValue, duration);
            yield return new WaitForSeconds(duration);
            this.OnAfterFade?.Invoke();
        }
    }
}
