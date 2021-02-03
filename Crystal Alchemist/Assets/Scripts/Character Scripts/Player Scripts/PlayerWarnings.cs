using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum WarningType
    {
        lookAtIt,
        lookAway,
        goAway,
        hide,
        stack,
        spread,
        warning
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerWarnings : MonoBehaviour
    {
        [System.Serializable]
        public struct Warning
        {
            [PreviewField]
            public Sprite sprite;
            public WarningType type;
        }

        [SerializeField]
        private List<Warning> warnings = new List<Warning>();

        [SerializeField]
        private float duration = 3f;

        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            GameEvents.current.OnWarning += ShowWarning;
        }

        private void ShowWarning(WarningType type)
        {
            Sprite sprite = GetSprite(type);

            if (sprite != null)
            {
                StopCoroutine(disableCo());
                this.spriteRenderer.sprite = sprite;
                StartCoroutine(disableCo());
            }
        }

        private Sprite GetSprite(WarningType type)
        {
            for (int i = 0; i < this.warnings.Count; i++)
            {
                if (warnings[i].type == type) return warnings[i].sprite;
            }

            return null;
        }

        private IEnumerator disableCo()
        {
            yield return new WaitForSeconds(this.duration);
            this.spriteRenderer.sprite = null;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnWarning -= ShowWarning;
        }
    }
}