using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public enum StatusEffectTriggerType
    {
        init,
        intervall,
        hit,
        life,
        mana,
        stacks,
        destroyed
    }

    public enum StatusEffectTriggerMode
    {
        equals,
        greater,
        less
    }

    [System.Serializable]
    public class StatusEffectTrigger
    {
        public StatusEffectTriggerType triggerType;

        [HideIf("triggerType", StatusEffectTriggerType.init)]
        [HideIf("triggerType", StatusEffectTriggerType.intervall)]
        [HideIf("triggerType", StatusEffectTriggerType.hit)]
        [HideIf("triggerType", StatusEffectTriggerType.destroyed)]
        public StatusEffectTriggerMode mode = StatusEffectTriggerMode.less;

        [HideIf("triggerType", StatusEffectTriggerType.destroyed)]
        [HideIf("triggerType", StatusEffectTriggerType.init)]
        public float value;

        private float elapsed;
        private int hitCounter;

        public void SetElapsed(float elapsed) => this.elapsed += elapsed;

        public bool IsTriggered(Character character)
        {
            switch (this.triggerType)
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
            if (this.elapsed >= this.value)
            {
                this.elapsed = 0;
                return true;
            }
            return false;
        }

        private bool LifeTrigger(Character character)
        {
            if (character != null) return ValueTrigger(character.values.life);
            return false;
        }

        private bool ManaTrigger(Character character)
        {
            if (character != null) return ValueTrigger(character.values.mana);
            return false;
        }

        private bool ValueTrigger(float value)
        {            
                if (this.mode == StatusEffectTriggerMode.less && value <= this.value) return true;
                else if (this.mode == StatusEffectTriggerMode.greater && value >= this.value) return true;
                else if (this.mode == StatusEffectTriggerMode.equals && value == this.value) return true;
            
            return false;
        }

        private bool HitTrigger(Character character)
        {
            if (character.values.cantBeHit) this.hitCounter++;
            if (this.hitCounter >= this.value) return true;
            return false;
        }
    }
}