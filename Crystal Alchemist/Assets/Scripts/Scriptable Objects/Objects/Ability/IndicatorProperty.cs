using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Ability/Indicator Property")]
    public class IndicatorProperty : ScriptableObject
    {
        [BoxGroup("Indikator")]
        [SerializeField]
        private TargetingIndicator indicator;

        [BoxGroup("Indikator")]
        [SerializeField]
        private bool overrideColor = false;

        [BoxGroup("Indikator")]
        [ShowIf("overrideColor")]
        [SerializeField]
        [ColorUsage(true, true)]
        private Color color = Color.white;

        [BoxGroup("Indikator")]
        [SerializeField]
        private bool overrideSprite = false;

        [BoxGroup("Indikator")]
        [SerializeField]
        [ShowIf("overrideSprite")]
        private Sprite sprite;

        public void Instantiate(Character sender, Character target, List<TargetingIndicator> appliedIndicators)
        {
            if (!alreadyApplied(sender, target, appliedIndicators)) //check if already applied
            {
                TargetingIndicator indicator = Instantiate(this.indicator);
                indicator.Initialize(sender, target);
                indicator.name = this.indicator.name;
                if (this.overrideColor) indicator.SetColor(this.color);
                if (this.overrideSprite) indicator.SetSprite(this.sprite);
                appliedIndicators.Add(indicator);
            }
        }

        private bool alreadyApplied(Character sender, Character target, List<TargetingIndicator> appliedIndicators)
        {
            foreach (TargetingIndicator applied in appliedIndicators)
            {
                if (applied.GetSender() == sender && applied.GetTarget() == target) return true;
            }

            return false;
        }
    }
}
