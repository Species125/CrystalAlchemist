using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum StatusEffectActionType
    {
        changeResource,
        effect,
        destroy,
        speed,
        time,
        module,
        immortal,
        dispell
    }

    [System.Serializable]
    public class StatusEffectAction
    {
        public StatusEffectActionType actionType;

        [ShowIf("actionType", StatusEffectActionType.changeResource)]
        [SerializeField]
        private List<CharacterResource> resources;

        [ShowIf("actionType", StatusEffectActionType.speed)]
        [SerializeField]
        private int speed;

        [ShowIf("actionType", StatusEffectActionType.time)]
        [SerializeField]
        private float time;

        [ShowIf("actionType", StatusEffectActionType.effect)]
        [SerializeField]
        private List<StatusEffect> effects;

        [ShowIf("actionType", StatusEffectActionType.dispell)]
        [SerializeField]
        private StatusEffectType StatusEffectType;

        public void DoAction(Character character, StatusEffect effect)
        {
            switch (this.actionType)
            {
                case StatusEffectActionType.changeResource: changeResource(character); break;
                case StatusEffectActionType.speed: character.UpdateSpeedPercent(this.speed); break;
                case StatusEffectActionType.time: character.updateTimeDistortion(this.time); break;
                case StatusEffectActionType.module: effect.doModule(); break;
                case StatusEffectActionType.immortal: if (character != null) character.setCannotDie(true); break;
                case StatusEffectActionType.destroy: effect.DestroyIt(); break;
                case StatusEffectActionType.dispell: DispellIt(character); break;
                case StatusEffectActionType.effect: AddStatusEffect(character); break;
            }
        }

        private void changeResource(Character character)
        {
            foreach (CharacterResource resource in this.resources)
            {
                character.UpdateResource(resource);
            }
        }

        private void DispellIt(Character character)
        {
            if (this.StatusEffectType == StatusEffectType.debuff) StatusEffectUtil.RemoveAllStatusEffects(character.values.debuffs);
            else StatusEffectUtil.RemoveAllStatusEffects(character.values.buffs);
        }

        private void AddStatusEffect(Character character)
        {
            foreach (StatusEffect eff in this.effects)
            {
                StatusEffectUtil.AddStatusEffect(eff, character);
            }
        }

        public void ResetAction(Character character)
        {
            if (this.actionType == StatusEffectActionType.speed) character.UpdateSpeedPercent(0);
            else if (this.actionType == StatusEffectActionType.time) character.updateTimeDistortion(0);
            else if (this.actionType == StatusEffectActionType.immortal) character.setCannotDie(false);
        }
    }
}