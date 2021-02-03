using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpawnIndicator : GroundIndicator
    {
        [BoxGroup("Fading")]
        [SerializeField]
        private float fadeIn;

        [BoxGroup("Fading")]
        [SerializeField]
        private float fadeOut;

        private SpriteRenderer sprite;

        private void Start()
        {
            sprite = this.GetComponent<SpriteRenderer>();
            sprite.DOFade(sprite.color.a, this.fadeIn);
        }

        public void DestroyIt()
        {
            sprite.DOFade(0, this.fadeOut);
            Destroy(this.gameObject, this.fadeOut + 0.3f);
        }
    }
}
