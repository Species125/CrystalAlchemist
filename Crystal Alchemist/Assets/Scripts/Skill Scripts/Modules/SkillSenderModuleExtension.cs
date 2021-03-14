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
            invincible,
            speedOnDurationValue,
            speedOnDurationPercent,
            attachToSender,
            lockDirection
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

            [HideIf("type", Type.invincible)]
            [HideIf("type", Type.speedOnDurationValue)]
            [HideIf("type", Type.attachToSender)]
            [MinValue(0)]
            [Tooltip("Dauer")]
            public float duration = 0;

            [ShowIf("type", Type.speedOnDurationValue)]
            [MinValue(-100)]
            [MaxValue(100)]
            [Tooltip("Bewegungseschwindigkeit während des Skills")]
            public float value = 0;
        }

        [SerializeField]
        private List<SenderAttributes> attributes = new List<SenderAttributes>();

        public override void Initialize()
        {
            foreach(SenderAttributes attribute in this.attributes)
            {
                if (attribute.type == Type.invincible) this.skill.sender.values.isInvincible = true;
                else if (attribute.type == Type.knockback) Thrust(attribute.forward, attribute.thrust, attribute.duration);
                else if (attribute.type == Type.speedOnDurationValue) this.skill.sender.UpdateSpeedValue(attribute.value);
                else if (attribute.type == Type.attachToSender) Attach();
                else if (attribute.type == Type.lockDirection) this.skill.SetDirectionLock(attribute.duration);
            }
        }

        private void Attach()
        {
            if (this.skill.sender == null) return;
            this.skill.isAttached = true;
            this.skill.transform.parent = this.skill.sender.activeSkillParent.transform;
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
                else if (attribute.type == Type.speedOnDurationValue) this.skill.sender.UpdateSpeedPercent(0);
            }
        }
    }
}
