using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillAnimationModule : SkillModule
    {
        [SerializeField]
        [InfoBox("Dont forget to revert your events in your animations")]
        [ColorUsage(true, true)]
        private Color targetColor;

        private bool changedColor = false;

        public void TriggerAnimation(string trigger) => this.skill.sender.TriggerAnimation(trigger);        

        public void SetAnimationToTrue(string name) => this.skill.sender.SetAnimationToBool(name, true);        

        public void SetAnimationToFalse(string name) => this.skill.sender.SetAnimationToBool(name, false);

        public void SetSpeedPercent(int percent) => this.skill.sender.UpdateSpeedPercent(percent);

        public void ChangeTint()
        {
            this.skill.sender.ChangeColor(this.targetColor);
            this.changedColor = true;
        }

        private void OnDestroy()
        {
            if (this.changedColor) this.skill.sender.RemoveColor(this.targetColor);
        }
    }
}