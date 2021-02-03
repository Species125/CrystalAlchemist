



using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillProjectileHit : SkillHitTrigger
    {
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
                && !isReflected(hittedCharacter))
            {
                AbilityUtil.SetEffectOnHit(this.skill, this.transform.position);
                AnimatorUtil.SetAnimatorParameter(this.skill.animator, "Hit");
                this.skill.GetComponent<SkillProjectile>().stopVelocity();
            }
        }

        private bool isReflected(Collider2D hittedCharacter)
        {
            if (hittedCharacter.GetComponent<SkillCollider>() != null
                && hittedCharacter.GetComponent<SkillCollider>().skill.GetComponent<SkillReflector>() != null
                && this.canBeReflected) return true;

            return false;
        }
    }
}
