
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillStandardHit : SkillHitTrigger
    {
        private void OnTriggerStay2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.CheckCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);
        }

        private void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.CheckCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);
        }
    }
}
