using AssetIcons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


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
public class Ability : ScriptableObject
{
    public enum IndicatorType
    {
        None,
        OnCast,
        OnTargeting
    }

    [BoxGroup("Inspector")]
    [ReadOnly]
    public string path;

#if UNITY_EDITOR
    private void OnValidate()
    {
        this.path = UnityUtil.GetResourcePath(this);
    }
#endif

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

    [BoxGroup("Restrictions")]
    [SerializeField]
    public float cooldown;

    [BoxGroup("Restrictions")]
    [SerializeField]
    public bool hasMaxDuration;

    [BoxGroup("Restrictions")]
    [SerializeField]
    [ShowIf("hasMaxDuration")]
    [MinValue(0)]
    public float maxDuration = 1;

    [BoxGroup("Restrictions")]
    [SerializeField]
    public bool hasDelay;

    [BoxGroup("Restrictions")]
    [SerializeField]
    [ShowIf("hasDelay")]
    [MinValue(0)]
    public float delay = 1;

    [BoxGroup("Restrictions")]
    [OnValueChanged("OnCastTimeChange")]
    [SerializeField]
    public bool hasCastTime = false;

    [BoxGroup("Restrictions")]
    [ShowIf("hasCastTime")]
    [MinValue(0)]
    public float castTime;

    [BoxGroup("Restrictions")]
    [ShowIf("hasCastTime")]
    public bool showCastbar = true;

    [BoxGroup("Restrictions")]
    [ShowIf("hasCastTime")]
    [SerializeField]
    private CastingAnimation castAnimation;

    [BoxGroup("Restrictions")]
    [SerializeField]
    private bool hasMaxAmount = false;

    [BoxGroup("Restrictions")]
    [ShowIf("hasMaxAmount")]
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
    [Tooltip("Positions-Offset, damit es nicht im Character anfängt")]
    public float positionOffset = 1f;

    [BoxGroup("Behaviors")]
    [Tooltip("Folgt der Skill dem Charakter")]
    public bool attachToSender = false;

    [BoxGroup("Behaviors")]
    [Tooltip("Während des Skills schaut der Charakter in die gleiche Richtung")]
    public bool lockDirection = false;

    [BoxGroup("Behaviors")]
    [Tooltip("Soll der Skill einer Zeitstörung beeinträchtigt werden?")]
    public bool timeDistortion = true;

    [BoxGroup("Debug")]
    public float cooldownLeft;
    [BoxGroup("Debug")]
    public float holdTimer;
    [BoxGroup("Debug")]
    public AbilityState state;
    [BoxGroup("Debug")]
    public bool enabled = true;
    [BoxGroup("Debug")]
    public bool active = true;

    [BoxGroup("Debug")]
    [SerializeField]
    private Character sender;

    [BoxGroup("Debug")]
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
        setStartParameters();
    }

    public void Updating()
    {
        updateCooldown();
    }

    private void updateCooldown()
    {
        if (this.state == AbilityState.onCooldown)
        {
            if (this.cooldownLeft > 0) this.cooldownLeft -= Time.deltaTime;
            else setStartParameters();
        }
    }

    public void setStartParameters()
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
        if (this.useIndicator == IndicatorType.OnTargeting && this.indicator != null) this.indicator.UpdateIndicators(this.sender, selectedTargets);
    }

    public void ShowCastingIndicator(Character target)
    {
        if (this.useIndicator == IndicatorType.OnCast && this.indicator != null) this.indicator.UpdateIndicator(this.sender, target);
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

    public void ResetCoolDown()
    {
        this.cooldownLeft = this.cooldown;
        this.state = AbilityState.onCooldown;
    }

    public bool HasEnoughResourceAndAmount()
    {
        bool enoughResource = this.isResourceEnough();
        bool notToMany = true;
        bool granted = true;

        if (this.hasMaxAmount && sender != null)
            notToMany = (getAmountOfSameSkills(this.skill, sender.values.activeSkills, sender.values.activePets) < this.maxAmount);

        if (this.requirements != null) granted = this.requirements.Granted();

        return (notToMany && enoughResource && granted);
    }

    private int getAmountOfSameSkills(Skill skill, List<Skill> activeSkills, List<Character> activePets)
    {
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
                if (activePets[i] != null && activePets[i].stats.name == summonSkill.name)
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

    private bool isResourceEnough()
    {
        if (this.sender == null) return false;

        SkillSenderModule senderModule = this.skill.GetComponent<SkillSenderModule>();
        if (senderModule != null)
        {
            return this.sender.canUseIt(senderModule.costs);
        }
        else return true;
    }     

    #endregion

}
