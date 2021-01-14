using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillSelfHit : SkillHitTrigger
    {
        [InfoBox("Wirkt auf den Sender direkt (ohne Collider)")]
        [SerializeField]
        [MinValue(0)]
        private float invincibleTimer = 0;

        public override void Initialize()
        {
            if (this.invincibleTimer > 0) this.skill.sender.SetCannotHit(this.invincibleTimer, false);
            this.skill.sender.gotHit(this.skill);
        }
    }
}
