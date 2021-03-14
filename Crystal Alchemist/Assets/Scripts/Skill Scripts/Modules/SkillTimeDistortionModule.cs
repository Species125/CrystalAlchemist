using UnityEngine;

namespace CrystalAlchemist {
    public class SkillTimeDistortionModule : SkillModule
    {
        [SerializeField]
        private bool timeDistortable = false;

        public override void Initialize()
        {
            this.skill.canAffectedBytimeDistortion = this.timeDistortable;
        }
    }
}
