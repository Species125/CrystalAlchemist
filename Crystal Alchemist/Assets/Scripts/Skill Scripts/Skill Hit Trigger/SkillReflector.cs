using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillReflector : SkillHitTrigger
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Skill hittedSkill = collision.GetComponentInParent<Skill>();

            if (hittedSkill != null && CanBeReflected(hittedSkill))
            {
                hittedSkill.sender = this.skill.sender;

                if (hittedSkill.myRigidbody != null)
                {
                    hittedSkill.SetDirection(Vector2.Reflect(hittedSkill.GetDirection(), this.skill.GetDirection()));
                    hittedSkill.GetComponent<SkillProjectile>().setVelocity();
                    hittedSkill.transform.rotation = RotationUtil.getRotation(hittedSkill.GetDirection());
                }
            }
        }

        private bool CanBeReflected(Skill skill)
        {
            if (skill.GetComponent<SkillProjectileHit>() != null
                && skill.GetComponent<SkillProjectileHit>().canBeReflected) return true;

            return false;
        }

    }
}
