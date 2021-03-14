




using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class InfoBox : MonoBehaviour
    {
        [SerializeField]
        private Image previewImage;

        [SerializeField]
        private TextMeshProUGUI nameField;

        [SerializeField]
        private TextMeshProUGUI descriptionField;

        [SerializeField]
        private GameObject additionalInfo;

        [SerializeField]
        private Image statusEffectPreviewImage;

        [SerializeField]
        private TextMeshProUGUI statusEffectNameField;

        [SerializeField]
        private TextMeshProUGUI statusEffectDescriptionField;


        #region setInfo

        private void setInfo(Ability ability)
        {
            this.additionalInfo.SetActive(false);

            this.nameField.text = ability.GetName();

            if (ability.hasSkillBookInfo && ability.info != null)
            {
                this.previewImage.sprite = ability.info.icon;
                this.descriptionField.text = ability.info.getDescription();
            }

            SkillTargetModule module = ability.skill.GetComponent<SkillTargetModule>();

            if(module != null)
            {
                StatusEffect statusEffect = module.GetStatusEffect();
                if (statusEffect == null) return;

                this.additionalInfo.SetActive(true);
                this.statusEffectPreviewImage.sprite = statusEffect.iconSprite;
                this.statusEffectNameField.text = statusEffect.GetName();
                this.statusEffectDescriptionField.text = statusEffect.GetDescription();
            }
        }

        private void setInfo(InventoryItem item)
        {
            this.additionalInfo.SetActive(false);

            this.previewImage.sprite = item.info.getSprite();

            this.nameField.text = item.getName();
            this.descriptionField.text = item.info.getDescription();
        }


        private void setInfo(ItemStats item)
        {
            this.additionalInfo.SetActive(false);

            this.previewImage.sprite = item.getSprite();

            this.nameField.text = item.getName();
            this.descriptionField.text = item.getDescription();
        }

        private void setInfo(CharacterAttributeStats stats)
        {
            this.additionalInfo.SetActive(false);

            this.previewImage.sprite = stats.icon.sprite;
            this.nameField.text = stats.GetName();
            this.descriptionField.text = stats.GetDescription();
        }


        #endregion


        #region show and hide

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void Show(Ability ability)
        {
            this.gameObject.SetActive(true);
            setInfo(ability);
        }

        public void Show(ItemStats item)
        {
            this.gameObject.SetActive(true);
            setInfo(item);
        }

        public void Show(InventoryItem item)
        {
            this.gameObject.SetActive(true);
            setInfo(item);
        }

        public void Show(CharacterAttributeStats stats)
        {
            this.gameObject.SetActive(true);
            setInfo(stats);
        }
        #endregion
    }
}
