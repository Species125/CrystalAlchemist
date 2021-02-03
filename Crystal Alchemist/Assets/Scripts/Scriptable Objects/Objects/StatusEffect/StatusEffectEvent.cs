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

        public void Initialize(Character character, StatusEffect effect)
        {
            if (this.trigger.triggerType == StatusEffectTriggerType.init) DoActions(character, effect);
        }

        public void Updating(float timeDistortion)
        {
            if (this.trigger.triggerType == StatusEffectTriggerType.intervall) this.trigger.SetElapsed(Time.deltaTime * timeDistortion);
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
            if (this.trigger.IsTriggered(character)) DoActions(character, effect);
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
