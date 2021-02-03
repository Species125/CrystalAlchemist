using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class AutoScroll : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private float positionChange = 0.0005f;

        [SerializeField]
        private float positionDelay = 0.01f;

        [SerializeField]
        private float waitingDelay = 10f;

        private float pos = 1f;

        private void OnEnable()
        {
            this.pos = 1f;
            StartCoroutine(waitCo());
        }

        private IEnumerator waitCo()
        {
            this.scrollRect.verticalNormalizedPosition = pos;
            yield return new WaitForSeconds(this.waitingDelay);
            if (this.pos >= 1f) StartCoroutine(nextCo());
            else
            {
                this.pos = 1f;
                StartCoroutine(waitCo());
            }
        }

        private IEnumerator nextCo()
        {
            while (this.pos > 0f)
            {
                this.pos -= this.positionChange;
                this.scrollRect.verticalNormalizedPosition = pos;
                yield return new WaitForSeconds(this.positionDelay);
            }

            StartCoroutine(waitCo());
        }


    }
}
