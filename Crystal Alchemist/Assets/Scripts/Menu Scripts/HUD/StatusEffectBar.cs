using System.Collections.Generic;



using UnityEngine;

namespace CrystalAlchemist
{
    public class StatusEffectBar : MonoBehaviour
    {
        private List<StatusEffectUI> activeStatusEffectUIs = new List<StatusEffectUI>();
        private List<StatusEffect> activeStatusEffects = new List<StatusEffect>();

        [SerializeField]
        private GameObject statusEffectHolder;
        [SerializeField]
        private StatusEffectUI statusEffectGameObject;

        [SerializeField]
        private CharacterValues values;

        private void Start()
        {
            this.statusEffectGameObject.gameObject.SetActive(false);
            GameEvents.current.OnEffectUpdate += updateStatusEffect;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnEffectUpdate -= updateStatusEffect;
        }

        public void setCharacter(CharacterValues characterValues)
        {
            if (this.values == null) this.values = characterValues;
        }

        public void updateStatusEffect()
        {
            if (this.values != null)
            {
                //füge ggf. beide Listen hinzu oder selektiere nur eine
                List<StatusEffect> activeStatusEffects = new List<StatusEffect>();

                activeStatusEffects.AddRange(this.values.buffs);
                activeStatusEffects.AddRange(this.values.debuffs);

                foreach (StatusEffect statusEffect in activeStatusEffects)
                {
                    if (!this.activeStatusEffects.Contains(statusEffect))
                    {
                        StatusEffectUI statusEffectGUI = Instantiate(this.statusEffectGameObject, this.statusEffectHolder.transform);
                        statusEffectGUI.setUI(statusEffect);
                        statusEffectGUI.gameObject.SetActive(true);

                        this.activeStatusEffects.Add(statusEffect);
                        this.activeStatusEffectUIs.Add(statusEffectGUI);
                    }
                }

                foreach (StatusEffectUI statusEffectUI in this.activeStatusEffectUIs)
                {
                    statusEffectUI.updateUI();
                }
            }
        }
    }
}
