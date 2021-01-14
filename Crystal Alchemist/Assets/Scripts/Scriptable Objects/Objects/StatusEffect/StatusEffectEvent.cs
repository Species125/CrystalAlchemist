using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class StatusEffectEvent
    {
        [SerializeField]
        [BoxGroup("Trigger")]
        private StatusEffectTrigger trigger;

        [SerializeField]
        private List<StatusEffectAction> actions;

        private float elapsed;
        private int hitCounter;

        public void Initialize(Character character, StatusEffect effect)
        {
            if (this.trigger.triggerType == StatusEffectTriggerType.init) DoActions(character, effect);
        }

        public void Updating(float timeDistortion)
        {
            this.elapsed += (Time.deltaTime * timeDistortion);
        }

        private void DoActions(Character character, StatusEffect effect)
        {
            foreach (StatusEffectAction action in this.actions)
            {
                action.DoAction(character, effect);
            }
        }

        public void DoEvents(Character character, StatusEffect effect)
        {
            if (isTriggered(character)) DoActions(character, effect);
        }

        private bool isTriggered(Character character)
        {
            switch (this.trigger.triggerType)
            {
                case StatusEffectTriggerType.intervall: return IntervallTrigger();
                case StatusEffectTriggerType.life: return LifeTrigger(character);
                case StatusEffectTriggerType.mana: return ManaTrigger(character);
                case StatusEffectTriggerType.hit: return HitTrigger(character);
            }
            return false;
        }

        private bool IntervallTrigger()
        {
            if (this.elapsed >= trigger.value)
            {
                this.elapsed = 0;
                return true;
            }
            return false;
        }

        private bool LifeTrigger(Character character)
        {
            if (character != null && character.values.life <= trigger.value) return true;
            return false;
        }

        private bool ManaTrigger(Character character)
        {
            if (character != null && character.values.mana <= trigger.value) return true;
            return false;
        }

        private bool HitTrigger(Character character)
        {
            if (character.values.cantBeHit) this.hitCounter++;
            if (this.hitCounter >= this.trigger.value) return true;
            return false;
        }



        public void ResetEvent(Character character, StatusEffect effect)
        {
            if (this.trigger.triggerType == StatusEffectTriggerType.destroyed) DoActions(character, effect);

            foreach (StatusEffectAction action in this.actions)
            {
                action.ResetAction(character);
            }
        }
    }
}
