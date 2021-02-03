using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillIgnoreCollisionModule : SkillModule
    {
        [SerializeField]
        private Collider2D ignoredCollider2D;

        public override void Initialize() => Physics2D.IgnoreCollision(this.skill.sender.boxCollider, this.ignoredCollider2D);
    }
}
