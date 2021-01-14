
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillAnimationModule : SkillModule
    {
        [SerializeField]
        private string animationTriggerName = "";

        [SerializeField]
        private bool useColor = false;

        [ShowIf("useColor")]
        [SerializeField]
        [ColorUsage(true, true)]
        private Color targetColor;


        private CastingAnimation activeCastingAnimation;

        public override void Initialize()
        {
            this.skill.sender.startAttackAnimation(this.animationTriggerName);
            if (this.useColor) this.skill.sender.ChangeColor(this.targetColor);
        }

        private void OnDestroy()
        {
            if (this.useColor) this.skill.sender.removeColor(this.targetColor);
        }
    }
}