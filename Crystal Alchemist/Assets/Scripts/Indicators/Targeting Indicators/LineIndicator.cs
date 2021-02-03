
using UnityEngine;

namespace CrystalAlchemist
{
    public class LineIndicator : TargetingIndicator
    {
        [SerializeField]
        private float distance = 100f;

        public override void Start()
        {
            base.Start();
            this.GetSpriteRenderer().enabled = false;
        }

        public override void Update()
        {
            LineRenderUtil.RenderLine(this.GetSender(), this.GetTarget(), this.GetSender().values.direction, this.distance, this.GetSpriteRenderer(), this.GetSender().GetShootingPosition(), out Collider2D hitted, out Vector2 hitPoint);
        }
    }
}
