using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillHealHit : SkillHitTrigger
    {
        [SerializeField]
        private bool revive = false;

        private void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            Character target = hittedCharacter.GetComponent<Character>();

            if (revive && target != null && CollisionUtil.CheckCollisionDead(hittedCharacter, this.skill)) target.Revive();           

            if (target != null && CollisionUtil.CheckCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);            
        }
    }
}
