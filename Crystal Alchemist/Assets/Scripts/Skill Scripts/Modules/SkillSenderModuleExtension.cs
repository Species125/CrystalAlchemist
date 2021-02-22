using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillSenderModuleExtension : SkillModule
    {
        public enum Type
        {
            knockback,
            invincible
        }

        [System.Serializable]
        public class SenderAttributes
        {
            public Type type;

            [ShowIf("type", Type.knockback)]
            [Tooltip("True = nach vorne, False = Knockback")]
            public bool forward = false;

            [ShowIf("type", Type.knockback)]
            [MinValue(0)]
            [Tooltip("Stärke des Knockbacks")]
            public float thrust = 0;

            [ShowIf("type", Type.knockback)]
            [MinValue(0)]
            [Tooltip("Dauer des Knockbacks")]
            [HideIf("thrust", 0f)]
            public float duration = 0;
        }


        [SerializeField]
        private List<SenderAttributes> attributes = new List<SenderAttributes>();


        public override void Initialize()
        {
            foreach(SenderAttributes attribute in this.attributes)
            {
                if (attribute.type == Type.invincible) this.skill.sender.values.isInvincible = true;
                else if (attribute.type == Type.knockback) Thrust(attribute.forward, attribute.thrust, attribute.duration);
            }
        }

        private void Thrust(bool forward, float thrust, float duration)
        {
            if (thrust > 0)
            {
                int trustdirection = -1; //knockback
                if (forward) trustdirection = 1; //dash

                this.skill.sender.KnockBack(duration, thrust, (this.skill.GetDirection() * trustdirection));
            }
        }

        private void OnDestroy()
        {
            foreach (SenderAttributes attribute in this.attributes)
            {
                if (attribute.type == Type.invincible) this.skill.sender.values.isInvincible = false;
            }
        }
    }
}
