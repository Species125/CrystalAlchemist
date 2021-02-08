using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace CrystalAlchemist
{
    public class SkillMenuDetails : MonoBehaviour
    {
        [BoxGroup("Sprites")]
        [SerializeField]
        private Sprite health;

        [BoxGroup("Sprites")]
        [SerializeField]
        private Sprite mana;

        [BoxGroup("Sprites")]
        [SerializeField]
        private Sprite item;

        [BoxGroup("Sender")]
        [SerializeField]
        private Image senderIcon;

        [BoxGroup("Sender")]
        [SerializeField]
        private TextMeshProUGUI senderAmount;

        [BoxGroup("Target")]
        [SerializeField]
        private Image targetIcon;

        [BoxGroup("Target")]
        [SerializeField]
        private TextMeshProUGUI targetAmount;

        [BoxGroup("Targeting")]
        [SerializeField]
        private Image targetingIcon;

        [BoxGroup("Targeting")]
        [SerializeField]
        private TextMeshProUGUI targetingAmount;

        [BoxGroup("Effects")]
        [SerializeField]
        private List<Image> effects = new List<Image>();

        public void SetDetails(Ability ability)
        {
            if(ability == null)
            {
                this.senderIcon.sprite = this.mana;
                this.targetIcon.sprite = this.health;
                this.targetingIcon.gameObject.SetActive(false);

                this.senderAmount.text = "0";
                this.targetAmount.text = "0";
                this.targetingAmount.text = "";

                foreach (Image effect in this.effects) effect.gameObject.SetActive(false);

                return;
            }

            SetDataFromSkillModules(ability.skill.GetComponent<SkillSenderModule>(), this.senderIcon, this.senderAmount);
            SetDataFromSkillModules(ability.skill.GetComponent<SkillTargetModule>(), this.targetIcon, this.targetAmount);

            if (ability.useTargetSystem && ability.targetingProperty != null)
            {
                this.targetingIcon.gameObject.SetActive(true);
                int amount = ability.targetingProperty.maxAmountOfTargets;

                if (ability.targetingProperty.targetingMode == TargetingMode.auto) this.targetingAmount.text = ""+amount;
                else this.targetingAmount.text = "1-" + amount;
            }
            else
            {
                this.targetingIcon.gameObject.SetActive(false);
                this.targetingAmount.text = "";
            }
        }

        private void SetDataFromSkillModules(SkillModule module, Image icon, TextMeshProUGUI textField)
        {
            CharacterResource costs = null;

            if (module != null && module.GetType() == typeof(SkillSenderModule))
            {
                costs = module.GetComponent<SkillSenderModule>().costs.Convert();
                icon.sprite = this.mana;
                textField.text = "0";
            }
            else if (module != null && module.GetType() == typeof(SkillTargetModule))
            {
                costs = module.GetComponent<SkillTargetModule>().GetResourceForMenu();
                icon.sprite = this.health;
                textField.text = "0";

                List<StatusEffect> statusEffects = module.GetComponent<SkillTargetModule>().GetStatusEffects();

                for (int i = 0; i < this.effects.Count; i++)
                {
                    if (statusEffects.Count > i)
                    {
                        this.effects[i].gameObject.SetActive(true);
                        this.effects[i].sprite = statusEffects[i].iconSprite;
                    }
                    else this.effects[i].gameObject.SetActive(false);
                }
            }

            if (module != null && costs != null && costs.resourceType != CostType.none)
            {
                switch (costs.resourceType)
                {
                    case CostType.life: icon.sprite = this.health; break;
                    case CostType.mana: icon.sprite = this.mana; break;
                    case CostType.item: icon.sprite = this.item; break;
                }

                if (costs.resourceType == CostType.item) textField.text = ""+costs.amount;
                else textField.text = FormatUtil.ConvertToResourceValueMenu(costs.amount);
            }            
        }
    }
}
