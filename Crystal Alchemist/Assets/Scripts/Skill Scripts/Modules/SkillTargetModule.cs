using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public struct ResourceModifier
    {
        public List<StatusEffectRequired> required;
        public List<CharacterResource> resources;
    }

    [System.Serializable]
    public struct StatusEffectRequired
    {
        public StatusEffect effect;
        public int stacks;
        public ShareType type;
    }

    public class SkillTargetModule : SkillModule
    {
        [BoxGroup("Ziel Attribute")]
        [Tooltip("Veränderung des Ziels. Negativ = Schaden, Positiv = Heilung")]
        public List<CharacterResource> affectedResources;

        [BoxGroup("Ziel Attribute")]
        [Tooltip("Statuseffekte")]
        public List<StatusEffect> statusEffects;

        [BoxGroup("Ziel Attribute")]
        public List<ResourceModifier> modifiers = new List<ResourceModifier>();

        [Space(10)]
        [BoxGroup("Ziel Attribute")]
        [MinValue(0)]
        [Tooltip("Stärke des Knockbacks")]
        public float thrust = 2;

        [BoxGroup("Ziel Attribute")]
        [MinValue(0)]
        [Tooltip("Dauer des Knockbacks")]
        [HideIf("thrust", 0f)]
        public float knockbackTime = 0.2f;

        [BoxGroup("Ziel Attribute")]
        [Required]
        public SkillAffections affections;

        public List<CharacterResource> GetAffectedResource(Character target)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                ResourceModifier modifier = modifiers[i];
                if (target.values.HasStatusEffects(modifier.required)) return modifier.resources;
            }

            return this.affectedResources;
        }
    }
}