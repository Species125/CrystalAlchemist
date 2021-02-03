
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ColliderIndicator : GroundIndicator
    {
        public enum FillType
        {
            normal,
            radial,
            sideways,
            down
        }

        [BoxGroup("Main")]
        [SerializeField]
        [Required]
        private new Collider2D collider;

        [BoxGroup("Inner")]
        [SerializeField]
        private GameObject border;

        [BoxGroup("Inner")]
        [SerializeField]
        private float size = 0.25f;

        [BoxGroup("Inner")]
        [ShowIf("border")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [BoxGroup("Progress")]
        [ShowIf("border")]
        [SerializeField]
        private FillType filltype;

        [BoxGroup("Progress")]
        [ShowIf("border")]
        [Range(0, 1)]
        [SerializeField]
        private float progress = 1f;

        [BoxGroup("Progress")]
        [ShowIf("border")]
        [SerializeField]
        private Skill skill;

        public override void SetIndicator()
        {
            SetOuter();
            SetInner();
        }

        public override void SetOuter()
        {
            base.SetOuter();
            this.outline.SetCollider(this.collider);
        }

        public void LateUpdate()
        {
            if (this.skill != null)
            {
                this.progress = skill.GetProgress();
                SetInner();
            }
        }

        private void SetInner()
        {
            if (this.border != null && this.collider != null)
            {
                Vector3 innerSize = new Vector3(this.progress, this.progress, 1);

                if (this.filltype == FillType.down)
                {
                    innerSize = new Vector3(1, this.progress, 1);
                }
                else if (this.filltype == FillType.sideways)
                {
                    innerSize = new Vector3(this.progress, 1, 1);
                }

                this.border.transform.localScale = innerSize;

                Vector3 size = new Vector3(0, 0, 1);
                Vector2 position = this.collider.offset;

                if (this.collider.GetType() == typeof(CircleCollider2D))
                {
                    float radius = this.size * this.collider.GetComponent<CircleCollider2D>().radius;
                    size = new Vector3(radius, radius, 1);
                }
                if (this.collider.GetType() == typeof(BoxCollider2D))
                {
                    float x = this.collider.GetComponent<BoxCollider2D>().size.x;
                    float y = this.collider.GetComponent<BoxCollider2D>().size.y;
                    size = new Vector3(x, y, 1);

                    if (this.filltype == FillType.radial)
                    {
                        if (progress > 0) position /= progress;
                        else position = Vector3.zero;
                    }
                }

                if (this.spriteRenderer != null)
                {
                    if (this.spriteRenderer.drawMode == SpriteDrawMode.Simple)
                        this.spriteRenderer.transform.localScale = size;
                    else
                    {
                        this.spriteRenderer.transform.localScale = Vector3.one;
                        this.spriteRenderer.size = size;
                        innerSize = Vector3.one;

                        if (this.filltype == FillType.sideways)
                        {
                            this.spriteRenderer.size *= new Vector2(this.progress, 1);
                        }
                        else if (this.filltype == FillType.down)
                        {
                            this.spriteRenderer.size *= new Vector2(1, this.progress);
                        }
                        else
                        {
                            innerSize = new Vector3(1, this.progress, 1);
                            this.spriteRenderer.size *= (Vector2.one * this.progress);
                        }

                        this.border.transform.localScale = innerSize;
                    }

                    this.spriteRenderer.transform.localPosition = position;
                }
            }
        }
    }
}
