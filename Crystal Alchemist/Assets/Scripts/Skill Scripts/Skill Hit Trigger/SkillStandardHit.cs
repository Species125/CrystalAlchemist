
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillStandardHit : SkillHitTrigger
    {
        public virtual void OnTriggerStay2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.checkCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);
        }

        public virtual void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.checkCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);
        }
    }
}
