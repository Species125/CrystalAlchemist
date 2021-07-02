using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Affections/Ability Affection")]
    public class SkillAffections : Affections
    {
        [BoxGroup("Wirkungsbereich")]
        [Tooltip("Sich selbst")]
        [SerializeField]
        private bool self = false;

        [BoxGroup("Wirkungsbereich")]
        [Tooltip("Skills")]
        [SerializeField]
        private bool skills = false;

        protected override bool IsAffected(Character sender, Character target)
        {
            return checkMatrix(sender, target, other, same, neutral, self);
        }

        public bool isSkillAffected(Skill origin, Skill hittedSkill)
        {
            if (hittedSkill != null && this.skills && hittedSkill != origin) return true;
            return false;
        }
    }
}
