using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ShadowAnimation : MonoBehaviour
    {
        [SerializeField]
        private bool scaleShadow = false;

        [ShowIf("scaleShadow")]
        [SerializeField]
        private GameObject reference;

        [ShowIf("scaleShadow")]
        [SerializeField]
        private float maxDistance = 5f;

        [SerializeField]
        private bool cloneShadow = false;

        [ShowIf("cloneShadow")]
        [SerializeField]
        private SpriteRenderer origin;

        private SpriteRenderer spriteRenderer;

        private void Start() => this.spriteRenderer = this.GetComponent<SpriteRenderer>();

        void Update()
        {
            if (this.cloneShadow && this.spriteRenderer != null && this.origin != null) this.spriteRenderer.sprite = this.origin.sprite;

            if (this.scaleShadow && this.reference != null)
            {
                float distance = (1 + ((reference.transform.position.y - this.transform.position.y) / this.maxDistance));
                this.transform.localScale = Vector2.one * (1 / distance);
            }
        }
    }
}
