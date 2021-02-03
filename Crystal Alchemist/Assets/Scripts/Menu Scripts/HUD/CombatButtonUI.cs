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
        private Sprite buttonIcon;

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

        private void OnValidate()
        {
            /*
        this.iconButton.sprite = this.buttonIcon;
        this.iconButtonTrans.sprite = this.buttonIcon;
        this.gameObject.name = this.buttonType.ToString();
        */
        }

        void Start()
        {
            MenuEvents.current.OnAbilityAssigned += setButtonSkillImages;
            setButtonSkillImages();
        }

        private void OnDestroy()
        {
            MenuEvents.current.OnAbilityAssigned -= setButtonSkillImages;
        }

        private void setButtonSkillImages()
        {
            setButton(this.buttonType, this.skillIconButton, this.iconButton);
        }

        private void FixedUpdate()
        {
            setButton(this.buttonType, this.skillIconButton, this.iconButton);
            updateButton();
        }

        private void updateButton()
        {
            Ability ability = this.buttons.GetAbilityFromButton(this.buttonType);

            this.disabled.SetActive(false);
            this.targeting.SetActive(false);
            this.cooldown.gameObject.SetActive(false);

            if (ability != null)
            {
                this.iconButton.enabled = true;
                float cooldownLeft = ability.cooldownLeft / (this.values.timeDistortion * this.values.spellspeed);
                float cooldownValue = ability.cooldown / (this.values.timeDistortion * this.values.spellspeed);

                SkillSenderModule senderModule = ability.skill.GetComponent<SkillSenderModule>();

                if (senderModule != null && senderModule.costs.resourceType == CostType.item && senderModule.costs.item != null)
                    this.ammo.text = this.inventory.GetAmount(senderModule.costs.item) + "";
                else this.ammo.text = "";

                if (cooldownLeft > 0 && cooldownValue > 0.5f)
                {
                    //Ist Skill in der Abklingzeit
                    this.cooldown.gameObject.SetActive(true);
                    string cooldownText = FormatUtil.setDurationToString(cooldownLeft);
                    this.cooldown.text = cooldownText;

                    this.iconButton.fillAmount = 1 - (cooldownLeft / cooldownValue);
                    this.cooldown.color = new Color(1f, 1f, 1f, 1f);
                    this.cooldown.outlineColor = new Color32(75, 75, 75, 255);
                    this.cooldown.outlineWidth = 0.25f;
                    this.skillIconButton.color = new Color(1f, 1f, 1f, 0.2f);
                    this.iconButton.color = new Color(1f, 1f, 1f, 1f);
                }
                else if (ability.IsTargetRequired() && ability.state == AbilityState.lockOn)
                {
                    //ist Skill in Zielerfassung
                    this.targeting.SetActive(true);
                    this.filled.fillAmount = this.targetingTimeLeft.GetValue();

                    this.skillIconButton.color = new Color(1f, 1f, 1f, 0.2f);
                    this.iconButton.color = new Color(1f, 1f, 1f, 0.2f);
                }
                else if (!ability.enabled
                         || !this.values.CanUseAbilities()
                         || !ability.HasEnoughResourceAndAmount()
                )
                {
                    //ist Skill nicht einsetzbar (kein Mana oder bereits aktiv)
                    this.disabled.SetActive(true);

                    this.skillIconButton.color = new Color(1f, 1f, 1f, 0.2f);
                    this.iconButton.color = new Color(1f, 1f, 1f, 0.2f);
                }
                else
                {
                    this.skillIconButton.color = new Color(1f, 1f, 1f, 1f);
                    this.iconButton.color = new Color(1f, 1f, 1f, 1f);
                    this.iconButton.fillAmount = 1;
                }
            }
            else
            {
                this.iconButton.enabled = false;
                this.iconButtonTrans.enabled = true;
                this.ammo.text = "";
            }
        }

        private void setButton(enumButton buttonInput, Image skillUI, Image buttonUI)
        {
            Ability ability = this.buttons.GetAbilityFromButton(buttonInput);

            if (ability == null || ability.skill == null)
            {
                skillUI.gameObject.SetActive(false);
                buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
            }
            else
            {
                skillUI.gameObject.SetActive(true);
                if (ability.hasSkillBookInfo && ability.info != null) skillUI.sprite = ability.info.icon;
                buttonUI.color = new Color(1f, 1f, 1f, 1f);
                buttonUI.fillAmount = 1;
            }
        }
    }
}
