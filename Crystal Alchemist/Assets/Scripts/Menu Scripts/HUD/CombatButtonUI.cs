using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class CombatButtonUI : MonoBehaviour
    {
        [BoxGroup("Pflicht")]
        [SerializeField]
        [Required]
        private PlayerButtons buttons;

        [BoxGroup("Pflicht")]
        [SerializeField]
        [Required]
        private PlayerInventory inventory;

        [BoxGroup("Pflicht")]
        [SerializeField]
        [Required]
        private CharacterValues values;

        [BoxGroup("Pflicht")]
        [SerializeField]
        private enumButton buttonType;

        [BoxGroup("Pflicht")]
        [SerializeField]
        [Required]
        private FloatValue targetingTimeLeft;

        [BoxGroup("Buttons")]
        [SerializeField]
        private Image iconButton;

        [BoxGroup("Buttons")]
        [SerializeField]
        private Image iconButtonTrans;

        [BoxGroup("Texts")]
        [SerializeField]
        private TextMeshProUGUI cooldown;

        [BoxGroup("Texts")]
        [SerializeField]
        private TextMeshProUGUI ammo;

        [BoxGroup("State")]
        [SerializeField]
        private Image skillIconButton;

        [BoxGroup("State")]
        [SerializeField]
        private GameObject targeting;

        [BoxGroup("State")]
        [SerializeField]
        private Image filled;

        [BoxGroup("State")]
        [SerializeField]
        private GameObject disabled;

        private Color inactiveColor = new Color(1f, 1f, 1f, 0.2f);
        private Color activeColor = new Color(1f, 1f, 1f, 1f);

        void Start()
        {
            MenuEvents.current.OnAbilityAssigned += SetButtonSkillImages;            

            this.cooldown.color = this.activeColor;
            this.cooldown.outlineColor = new Color32(75, 75, 75, 255);
            this.cooldown.outlineWidth = 0.25f;

            this.iconButton.gameObject.SetActive(false);
            this.skillIconButton.gameObject.SetActive(false);
            this.ammo.gameObject.SetActive(false);

            this.disabled.SetActive(false);
            this.cooldown.gameObject.SetActive(false);
            this.targeting.SetActive(false);

            SetButtonSkillImages();
        }

        private void OnDestroy()
        {
            MenuEvents.current.OnAbilityAssigned -= SetButtonSkillImages;
        }

        private void SetButtonSkillImages()
        {
            SetButton(this.buttonType, this.skillIconButton, this.iconButton);
        }

        private void FixedUpdate()
        {
            //SetButton(this.buttonType, this.skillIconButton, this.iconButton);
            UpdateButton();
        }

        private void UpdateButton()
        {
            Ability ability = this.buttons.GetAbilityFromButton(this.buttonType);

            this.iconButton.gameObject.SetActive(false);
            this.ammo.gameObject.SetActive(false);

            this.disabled.SetActive(false);
            this.cooldown.gameObject.SetActive(false);
            this.targeting.SetActive(false);

            if (ability == null) return;

            this.iconButton.gameObject.SetActive(true);

            float cooldownLeft = ability.cooldownLeft / (this.values.timeDistortion * this.values.spellspeed);
            float cooldownValue = ability.cooldown / (this.values.timeDistortion * this.values.spellspeed);

            SkillSenderModule senderModule = ability.skill.GetComponent<SkillSenderModule>();

            if (senderModule != null && senderModule.costs.resourceType == CostType.item && senderModule.costs.item != null)
            {
                this.ammo.gameObject.SetActive(true);
                this.ammo.text = this.inventory.GetAmount(senderModule.costs.item) + "";
            }

            if (cooldownLeft > 0 && cooldownValue > 0.5f)
            {
                //Ist Skill in der Abklingzeit
                this.cooldown.gameObject.SetActive(true);
                this.cooldown.text = FormatUtil.setDurationToString(cooldownLeft);

                this.iconButton.fillAmount = 1 - (cooldownLeft / cooldownValue);
                this.skillIconButton.color = this.inactiveColor;
                this.iconButton.color = this.activeColor;
            }
            else if (ability.IsTargetRequired() && ability.state == AbilityState.lockOn)
            {
                //ist Skill in Zielerfassung
                this.targeting.SetActive(true);
                this.filled.fillAmount = this.targetingTimeLeft.GetValue();

                this.skillIconButton.color = this.inactiveColor;
                this.iconButton.color = this.inactiveColor;
            }
            else if (!ability.enabled
                     || !this.values.CanUseAbilities()
                     || !ability.HasEnoughResourceAndAmount()
            )
            {
                //ist Skill nicht einsetzbar (kein Mana oder bereits aktiv)
                this.disabled.SetActive(true);

                this.skillIconButton.color = this.inactiveColor;
                this.iconButton.color = this.inactiveColor;
            }
            else
            {
                this.skillIconButton.color = this.activeColor;
                this.iconButton.color = this.activeColor;
                this.iconButton.fillAmount = 1;
            }
        }

        private void SetButton(enumButton buttonInput, Image skillUI, Image buttonUI)
        {
            Ability ability = this.buttons.GetAbilityFromButton(buttonInput);

            if (ability == null || ability.skill == null)
            {
                skillUI.gameObject.SetActive(false);
                buttonUI.color = this.inactiveColor;
            }
            else
            {
                skillUI.gameObject.SetActive(true);
                if (ability.hasSkillBookInfo && ability.info != null) skillUI.sprite = ability.info.icon;
                buttonUI.color = this.activeColor;
                buttonUI.fillAmount = 1;
            }
        }
    }
}
