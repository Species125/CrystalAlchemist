using AssetIcons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum AbilityState
    {
        disabled,
        onCooldown,
        notCharged,
        charged,
        targetRequired,
        lockOn,
        ready
    }


    [CreateAssetMenu(menuName = "Game/Ability/Ability")]
    public class Ability : NetworkScriptableObject
    {
        public enum IndicatorType
        {
            None,
            OnCast,
            OnTargeting
        }

        [BoxGroup("Objects")]
        [Required]
        public Skill skill;

        [BoxGroup("Objects")]
        [SerializeField]
        public bool useTargetSystem = false;

        [BoxGroup("Objects")]
        [ShowIf("useTargetSystem")]
        [SerializeField]
        public TargetingProperty targetingProperty;

        [BoxGroup("Objects")]
        [SerializeField]
        public IndicatorType useIndicator = IndicatorType.None;

        [BoxGroup("Objects")]
        [HideIf("useIndicator", IndicatorType.None)]
        [HideLabel]
        [SerializeField]
        private IndicatorObject indicator;

        /*
    [BoxGroup("Objects")]
    [ShowIf("useIndicator", IndicatorType.OnCast)]
    [SerializeField]
    private bool showOnTarget = false;
    */

        [BoxGroup("Objects")]
        [SerializeField]
        public bool hasSkillBookInfo = false;

        [BoxGroup("Objects")]
        [HideIf("hasSkillBookInfo")]
        [SerializeField]
        private Sprite icon;

        [BoxGroup("Objects")]
        [ShowIf("hasSkillBookInfo")]
        [SerializeField]
        public SkillBookInfo info;

        [HorizontalGroup("Restrictions/GCD")]
        [SerializeField]
        public float cooldown;

        [HorizontalGroup("Restrictions/GCD")]
        [SerializeField]
        public float globalCooldown;
        
        [SerializeField]
        [HorizontalGroup("Restrictions/Duration")]
        public bool hasMaxDuration;
        
        [SerializeField]
        [ShowIf("hasMaxDuration")]
        [HorizontalGroup("Restrictions/Duration")]
        [MinValue(0)]
        public float maxDuration = 1;

        [HorizontalGroup("Restrictions/Delay")]
        [SerializeField]
        public bool hasDelay;

        [HorizontalGroup("Restrictions/Delay")]
        [SerializeField]
        [ShowIf("hasDelay")]
        [MinValue(0)]
        public float delay = 1;

        [OnValueChanged("OnCastTimeChange")]
        [HorizontalGroup("Restrictions/Casttime")]
        [SerializeField]
        public bool hasCastTime = false;

        [ShowIf("hasCastTime")]
        [HorizontalGroup("Restrictions/Casttime")]
        [MinValue(0)]
        public float castTime;

        [HorizontalGroup("Restrictions/Casttime2")]
        [ShowIf("hasCastTime")]
        public bool showCastbar = true;

        [HorizontalGroup("Restrictions/Casttime2")]
        [ShowIf("hasCastTime")]
        [SerializeField]
        private CastingAnimation castAnimation;

        [HorizontalGroup("Restrictions/maxAmount")]
        [SerializeField]
        private bool hasMaxAmount = false;

        [ShowIf("hasMaxAmount")]
        [HorizontalGroup("Restrictions/maxAmount")]
        [SerializeField]
        private int maxAmount = 1;

        [BoxGroup("Restrictions")]
        [SerializeField]
        public SkillRequirement requirements;

        [BoxGroup("Booleans")]
        [SerializeField]
        public bool isRapidFire = false;

        [BoxGroup("Booleans")]
        [SerializeField]
        public bool remoteActivation = false;

        [BoxGroup("Booleans")]
        [SerializeField]
        public bool deactivateButtonUp = false;

        [BoxGroup("Booleans")]
        [SerializeField]
        [HideIf("castTime", 0f)]
        public bool keepCast = false;

        [BoxGroup("Behaviors")]
        [SerializeField]
        public bool shareDamage = false;

        [BoxGroup("Behaviors")]
        [Tooltip("Folgt der Skill dem Charakter")]
        public bool attachToSender = false;

        [HorizontalGroup("Behaviors/Lock")]
        [Tooltip("Während des Skills schaut der Charakter in die gleiche Richtung")]
        public bool lockDirection = false;

        [HorizontalGroup("Behaviors/Lock")]
        [ShowIf("lockDirection")]
        [Tooltip("Während des Skills schaut der Charakter in die gleiche Richtung")]
        public float lockDuration = 0.15f;

        [BoxGroup("Behaviors")]
        [Tooltip("Soll der Skill einer Zeitstörung beeinträchtigt werden?")]
        public bool timeDistortion = true;

        [BoxGroup("Debug")]
        [ReadOnly]
        public float cooldownLeft;
        [BoxGroup("Debug")]
        [ReadOnly]
        public float holdTimer;
        [BoxGroup("Debug")]
        [ReadOnly]
        public AbilityState state;
        [BoxGroup("Debug")]
        [ReadOnly]
        public bool enabled = true;
        [BoxGroup("Debug")]
        [ReadOnly]
        public bool active = true;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private Character sender;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private CastingAnimation activeAnimation;

        [AssetIcon]
        public Sprite GetSprite()
        {
            if (this.hasSkillBookInfo && this.info != null) return this.info.icon;
            return this.icon;
        }

        private void OnCastTimeChange()
        {
            if (this.hasCastTime) this.deactivateButtonUp = false;
        }


        public void SetSender(Character sender)
        {
            this.sender = sender;
        }

        public Character GetSender()
        {
            return this.sender;
        }

        public string GetName()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Name", LocalisationFileType.skills);
        }

        #region Update Functions

        public void Initialize()
        {
            SetStartParameters();
        }

        public void Updating()
        {
            if (this.state == AbilityState.onCooldown)
            {
                if (this.cooldownLeft > 0) this.cooldownLeft -= Time.deltaTime;
                else SetStartParameters();
            }
        }

        public void SetStartParameters()
        {
            if (!this.hasCastTime) this.castTime = 0;

            this.cooldownLeft = 0;

            if (this.hasCastTime && this.holdTimer < this.castTime) this.state = AbilityState.notCharged;
            else if (this.IsTargetRequired()) this.state = AbilityState.targetRequired;
            else this.state = AbilityState.ready;
        }

        #endregion


        #region functions

        public void Charge()
        {
            if (this.holdTimer <= this.castTime)
            {
                this.holdTimer += Time.deltaTime;
                this.state = AbilityState.notCharged; //?
            }
            else
            {
                if (this.IsTargetRequired()) this.state = AbilityState.targetRequired; //aufgeladen, aber Ziel benötigt!
                else this.state = AbilityState.charged; //aufgeladen!
            }
        }

        public void SetLockOnState() => this.state = AbilityState.lockOn;

        public void ResetLockOn() => this.state = AbilityState.onCooldown;

        public void ResetCharge()
        {
            if (!this.keepCast) this.holdTimer = 0;
            else if (this.keepCast && this.holdTimer > this.castTime) this.holdTimer = 0;
        }

        public void HideIndicator()
        {
            if (this.useIndicator != IndicatorType.None && this.indicator != null) this.indicator.ClearIndicator();
        }

        public void ShowTargetingIndicator(List<Character> selectedTargets) //Sender needed?
        {
            if (this.useIndicator == IndicatorType.OnTargeting && this.indicator != null) this.indicator.UpdateTargetingIndicators(this.sender, selectedTargets);
        }

        public void ShowCastingIndicator(Character target)
        {
            if (this.useIndicator == IndicatorType.OnCast && this.indicator != null) this.indicator.UpdateCastingIndicator(this.sender, target);
        }

        public void ShowCastingIndicator(List<Character> targets)
        {
            if (this.useIndicator == IndicatorType.OnCast && this.indicator != null) this.indicator.UpdateCastingIndicator(this.sender, targets);
        }

        public void ShowCastingAnimation()
        {
            this.sender.PlayCastingAnimation(true);

            if (this.castAnimation != null && this.activeAnimation == null)
            {
                this.activeAnimation = Instantiate(this.castAnimation, sender.GetGroundPosition(), Quaternion.identity, sender.transform);
                this.activeAnimation.Initialize(this.castTime);
            }
        }

        public void HideCastingAnimation()
        {
            this.sender.PlayCastingAnimation(false);

            if (this.activeAnimation != null)
            {
                this.activeAnimation.DestroyIt();
                this.activeAnimation = null;
            }
        }

        public void ResetCoolDown(float coolDown)
        {
            this.cooldownLeft = coolDown;
            this.state = AbilityState.onCooldown;
        }

        public void ResetCoolDown() => ResetCoolDown(this.cooldown);

        public bool HasEnoughResourceAndAmount()
        {
            bool enoughResource = this.IsResourceEnough();
            bool notToMany = true;
            bool granted = true;

            if (this.hasMaxAmount && sender != null)
                notToMany = (getAmountOfSameSkills(this.skill, sender.values.activeSkills, sender.values.activePets) < this.maxAmount);

            if (this.requirements != null) granted = this.requirements.Granted();

            return (notToMany && enoughResource && granted);
        }

        private int getAmountOfSameSkills(Skill skill, List<Skill> activeSkills, List<Character> activePets)
        {
            activeSkills.RemoveAll(item => item == null);

            int result = 0;
            SkillSummon summonSkill = skill.GetComponent<SkillSummon>();

            if (summonSkill == null)
            {
                for (int i = 0; i < activeSkills.Count; i++)
                {
                    Skill activeSkill = activeSkills[i];
                    if (activeSkill.name == skill.name) result++;
                }
            }
            else
            {
                for (int i = 0; i < activePets.Count; i++)
                {
                    if (activePets[i] != null && activePets[i].GetCharacterName() == summonSkill.GetPetName())
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public bool IsTargetRequired()
        {
            if (this.useTargetSystem && this.targetingProperty != null) return true;
            return false;
        }

        private bool IsResourceEnough()
        {
            if (this.sender == null) return false;

            SkillSenderModule senderModule = this.skill.GetComponent<SkillSenderModule>();
            if (senderModule != null)
            {
                return this.sender.CanUseSkill(senderModule.costs);
            }
            else return true;
        }

        #endregion

    }
}