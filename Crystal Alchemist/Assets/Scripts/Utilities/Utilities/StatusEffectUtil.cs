using System.Collections.Generic;




using UnityEngine;

namespace CrystalAlchemist
{
    public static class StatusEffectUtil
    {
        public static void RemoveAllStatusEffects(List<StatusEffect> statusEffects)
        {
            foreach (StatusEffect effect in statusEffects) effect.DestroyIt();
            statusEffects.Clear();
        }

        public static void RemoveStatusEffect(StatusEffect statusEffect, bool allTheSame, Character character)
        {
            List<StatusEffect> statusEffects = null;

            if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = character.values.debuffs;
            else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = character.values.buffs;

            //Store in temp List to avoid Enumeration Exception
            foreach (StatusEffect effect in statusEffects)
            {
                if (effect.name == statusEffect.name)
                {
                    effect.DestroyIt();
                    if (!allTheSame) break;
                }
            }
        }

        public static bool GetImmunity(StatusEffect statusEffect, Character character)
        {
            if (character.stats.isImmuneToAllDebuffs
                && statusEffect.statusEffectType == StatusEffectType.debuff) return true;
            else
            {
                for (int i = 0; i < character.stats.immunityToStatusEffects.Count; i++)
                {
                    StatusEffect immunityEffect = character.stats.immunityToStatusEffects[i];
                    if (immunityEffect != null && statusEffect.name == immunityEffect.name) return true;
                }
            }

            return false;
        }

        public static List<StatusEffect> GetSameEffects(StatusEffect statusEffect, Character character)
        {
            List<StatusEffect> result = new List<StatusEffect>();
            List<StatusEffect> statusEffects = character.values.debuffs;
            if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = character.values.buffs;

            for (int i = 0; i < statusEffects.Count; i++)
            {
                if (statusEffects[i].name == statusEffect.name) result.Add(statusEffects[i]);
            }

            return result;
        }

        public static void AddStatusEffect(StatusEffect statusEffect, Character character)
        {
            if (statusEffect != null && character.values.characterType != CharacterType.Object)
            {
                bool isImmune = GetImmunity(statusEffect, character);

                if (!isImmune)
                {
                    List<StatusEffect> sameEffects = GetSameEffects(statusEffect, character);

                    if (sameEffects.Count < statusEffect.maxStacks)
                    {
                        //Wenn der Effekte die maximale Anzahl Stacks nicht überschritten hat -> Hinzufügen
                        Instantiate(statusEffect, character);
                    }
                    else
                    {
                        if (statusEffect.mode == StatusEffectMode.overrideIt) sameEffects[0].Initialize(character);
                        else if (statusEffect.mode == StatusEffectMode.destroyIt) RemoveAllStatusEffects(sameEffects);                        
                    }
                }
            }
        }

        private static void Instantiate(StatusEffect statusEffect, Character character)
        {
            StatusEffect effect = Object.Instantiate(statusEffect);

            effect.name = statusEffect.name;
            character.AddStatusEffect(effect);
        }
    }
}
