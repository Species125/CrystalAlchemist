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
        destroyed
    }

    [System.Serializable]
    public class StatusEffectTrigger
    {
        public StatusEffectTriggerType triggerType;

        [HideIf("triggerType", StatusEffectTriggerType.destroyed)]
        [HideIf("triggerType", StatusEffectTriggerType.init)]
        public float value;
    }
}