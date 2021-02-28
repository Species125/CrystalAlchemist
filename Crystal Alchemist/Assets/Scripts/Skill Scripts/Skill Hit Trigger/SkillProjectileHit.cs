using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillProjectileHit : SkillHitTrigger
    {
        [InfoBox("Use Trigger 'Hit' when hit something")]
        public bool canBeReflected = false;

        private void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            stopProjectile(hittedCharacter);
        }

        private void OnTriggerStay2D(Collider2D hittedCharacter)
        {
            stopProjectile(hittedCharacter);
        }

        private void stopProjectile(Collider2D hittedCharacter)
        {
            //Stop Arrow on Hit
            if (
                hittedCharacter.gameObject != this.skill.sender.gameObject //not self
                && (CollisionUtil.CheckCollision(hittedCharacter, this.skill) //Character
                    || (hittedCharacter.GetComponent<Character>() == null && !hittedCharacter.isTrigger)) //Wall
                && !IsReflectedBySkill(hittedCharacter))
            {
                AbilityUtil.SetEffectOnHit(this.skill, this.transform.position);
                AnimatorUtil.SetAnimatorParameter(this.skill.animator, "Hit");
                this.skill.GetComponent<SkillProjectile>().stopVelocity();
            }
        }

        private bool IsReflectedBySkill(Collider2D collision)
        {
            return this.canBeReflected && collision.GetComponentInParent<SkillReflector>() != null;
        }
    }
}
