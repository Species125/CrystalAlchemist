using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillTouch : SkillExtension
    {
        public override void Initialize()
        {
            CollisionUtil.AddColliderCopy(this.skill.sender, this.gameObject);         
        }       
    }
}
