using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillProjectile : SkillExtension
    {
        public float speed = 1;

        public override void Initialize()
        {
            if (this.skill.myRigidbody != null)
            {
                this.setVelocity();
            }
        }

        public void setVelocity()
        {
            if (this.skill.myRigidbody != null) this.skill.myRigidbody.velocity = this.skill.GetDirection() * this.speed * this.skill.getTimeDistortion();
        }

        public void stopVelocity()
        {
            if (this.skill.myRigidbody != null) this.skill.myRigidbody.velocity = Vector2.zero;
        }
    }
}
