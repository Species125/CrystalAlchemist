using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class SkillMenu : MenuBehaviour
    {
        #region Attributes

        [BoxGroup("Mandatory")]
        [SerializeField]
        [Required]
        private CustomCursor cursor;

        [BoxGroup("Tabs")]
        [SerializeField]
        private GameObject physicalSkills;
        [BoxGroup("Tabs")]
        [SerializeField]
        private GameObject magicalSkills;
        [BoxGroup("Tabs")]
        [SerializeField]
        private GameObject itemSkills;

        [BoxGroup("Detail-Ansicht")]
        [SerializeField]
        private TextMeshProUGUI skillDetailsName;
        [BoxGroup("Detail-Ansicht")]
        [SerializeField]
        private TextMeshProUGUI skillDetailsStrength;
        [BoxGroup("Detail-Ansicht")]
        [SerializeField]
        private TextMeshProUGUI skillDetailsCost;
        [BoxGroup("Detail-Ansicht")]
        [SerializeField]
        private Image StatusEffects;

        private Ability selectedAbility;

        #endregion


        #region Unity Functions

        public override void Start()
        {
            base.Start();
            MenuEvents.current.OnAbilitySelected += SelectSkill;
            MenuEvents.current.OnAbilitySet += GetAbility;

            InitializePages(this.physicalSkills);
            InitializePages(this.magicalSkills);
            InitializePages(this.itemSkills);

            ShowCategory("physical");
        }

        private void test()
        {

        }

        private Ability GetAbility()
        {
            return this.selectedAbility;
        }

        private void InitializePages(GameObject parent)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                SkillPage page = parent.transform.GetChild(i).GetComponent<SkillPage>();
                if (page != null) page.Initialize();
            }
        }

        public override void OnDestroy()
        {
            MenuEvents.current.OnAbilitySelected -= SelectSkill;
            MenuEvents.current.OnAbilitySet -= GetAbility;
            base.OnDestroy();
        }

        public override void Cancel()
        {
            if (this.selectedAbility != null) SelectSkill(null);
            else base.Cancel();
        }

        #endregion


        #region OnClickTrigger

        public void ShowSkillDetails(SkillSlot slot)
        {
            showSkillDetails(slot.ability);
        }

        public void ShowSkillDetails(SkillMenuActiveSlots slot)
        {
            showSkillDetails(slot.ability);
        }

        private void showSkillDetails(Ability ability)
        {
            if (ability != null)
            {
                SkillTargetModule targetModule = ability.skill.GetComponent<SkillTargetModule>();
                SkillSenderModule senderModule = ability.skill.GetComponent<SkillSenderModule>();

                this.skillDetailsName.text = ability.GetName();
                float strength = 0;

                if (senderModule != null)
                {
                    this.skillDetailsCost.text = Mathf.Abs(senderModule.costs.amount).ToString("N1");
                }

                if (targetModule != null)
                {
                    if (targetModule.affectedResources.Count > 0) strength = Mathf.Abs(targetModule.affectedResources[0].amount);
                    this.skillDetailsStrength.text = strength + "";

                    if (targetModule.statusEffects.Count > 0)
                    {
                        this.StatusEffects.enabled = true;
                        this.StatusEffects.sprite = targetModule.statusEffects[0].iconSprite;
                    }
                    else this.StatusEffects.enabled = false;
                }
            }
            else
            {
                HideSkillDetails();
            }
        }

        public void HideSkillDetails()
        {
            this.skillDetailsName.text = "";
            this.skillDetailsStrength.text = "";
            this.skillDetailsCost.text = "";

            this.StatusEffects.enabled = false;
        }

        public void ShowCategory(string category)
        {
            this.physicalSkills.SetActive(false);
            this.magicalSkills.SetActive(false);
            this.itemSkills.SetActive(false);

            switch (category)
            {
                case "physical": this.physicalSkills.SetActive(true); break;
                case "magical": this.magicalSkills.SetActive(true); break;
                default: this.itemSkills.SetActive(true); break;
            }
        }

        public void SelectSkill(Ability ability)
        {
            this.selectedAbility = ability;
            if (ability != null) this.cursor.setSelectedGameObject(ability.info.icon);
            else this.cursor.setSelectedGameObject(null);
        }

        #endregion
    }
}
