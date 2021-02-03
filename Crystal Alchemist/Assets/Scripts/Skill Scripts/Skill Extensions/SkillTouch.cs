
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillTouch : SkillExtension
    {
        public override void Initialize()
        {
            Collider2D temp = this.skill.sender.boxCollider;

            Collider2D te = UnityUtil.CopyComponent(temp, this.gameObject);
            te.isTrigger = true;
        }
    }
}
