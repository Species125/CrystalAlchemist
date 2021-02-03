using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillOrbital : SkillExtension
    {

        public override void Initialize()
        {
            setPosition();
        }

        private void setPosition()
        {
            if (this.skill.target != null)
            {
                float x = this.skill.target.transform.position.x;
                float y = this.skill.target.transform.position.y;

                if (this.skill.target.boxCollider != null)
                {
                    x += this.skill.target.boxCollider.offset.x;
                    y += this.skill.target.boxCollider.offset.y;
                }
                this.skill.transform.position = new Vector2(x, y);
            }
        }
    }
}
