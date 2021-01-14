


using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillReflector : SkillHitTrigger
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<SkillCollider>() != null)
            {
                Skill skill = collision.GetComponent<SkillCollider>().skill;

                if (skill != null && isReflected(skill))
                {
                    skill.sender = this.skill.sender;

                    if (skill.myRigidbody != null)
                    {
                        skill.SetDirection(Vector2.Reflect(skill.GetDirection(), this.skill.GetDirection()));
                        skill.GetComponent<SkillProjectile>().setVelocity();
                        skill.transform.rotation = RotationUtil.getRotation(skill.GetDirection());
                    }
                }
            }
        }

        private bool isReflected(Skill skill)
        {
            if (skill.GetComponent<SkillProjectileHit>() != null
                && skill.GetComponent<SkillProjectileHit>().canBeReflected) return true;

            return false;
        }

    }
}
