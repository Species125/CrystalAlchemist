using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class IndicatorObject
    {
        [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
        public IndicatorProperty indicatorProperty;

        private List<TargetingIndicator> appliedIndicators = new List<TargetingIndicator>();

        private void Instantiate(Character sender, Character target)
        {
            if (this.indicatorProperty != null) this.indicatorProperty.Instantiate(sender, target, this.appliedIndicators);
        }

        public void UpdateIndicator(Character sender, Character target)
        {
            if (appliedIndicators.Count > 0
                && appliedIndicators[0].GetTarget() == target) return;

            ClearIndicator();
            Instantiate(sender, target);
        }

        public void UpdateIndicators(Character sender, List<Character> selectedTargets)
        {
            RemoveIndicator(selectedTargets);
            AddIndicator(sender, selectedTargets);
        }

        private void AddIndicator(Character sender, List<Character> selectedTargets)
        {
            foreach (Character target in selectedTargets)
            {
                this.Instantiate(sender, target);
            }
        }

        public void RemoveIndicator(List<Character> selectedTargets)
        {
            List<TargetingIndicator> tempAppliedList = new List<TargetingIndicator>();

            foreach (TargetingIndicator applied in this.appliedIndicators)
            {
                if (applied != null && !selectedTargets.Contains(applied.GetTarget())) Object.Destroy(applied.gameObject);
                else tempAppliedList.Add(applied);
            }

            this.appliedIndicators = tempAppliedList;
            this.appliedIndicators.RemoveAll(item => item == null);
        }

        public TargetingIndicator GetIndicator(Character target)
        {
            for (int i = 0; i < this.appliedIndicators.Count; i++)
            {
                if (this.appliedIndicators[i].GetTarget() == target) return this.appliedIndicators[i];
            }
            return null;
        }

        public void ClearIndicator()
        {
            for (int i = 0; i < this.appliedIndicators.Count; i++)
            {
                Object.Destroy(this.appliedIndicators[i].gameObject);
            }
            this.appliedIndicators.Clear();
        }

        public void ChangeIndicator(Character target, Color color)
        {
            TargetingIndicator result = GetIndicator(target);
            if (result != null) result.SetColor(color);
        }
    }
}
