﻿using AssetIcons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

public enum AbilityRequirements
{
    none,
    teleport
}


[CreateAssetMenu(menuName = "Game/Ability/Ability")]
public class Ability : ScriptableObject
{    
    [BoxGroup("Texts")]
    [SerializeField]
    private string abilityName;

    [BoxGroup("Texts")]
    [SerializeField]
    private string abilityNameEnglish;

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
    public bool hasSkillBookInfo = false;

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
    public float maxDuration = 1;

    [BoxGroup("Restrictions")]
    [ShowIf("isRapidFire")]
    [SerializeField]
    private float rapidFireDelay = 1;

    [BoxGroup("Restrictions")]
    [OnValueChanged("OnCastTimeChange")]
    [SerializeField]
    public bool hasCastTime = false;

    [BoxGroup("Restrictions")]
    [ShowIf("hasCastTime")]
    public float castTime;

    [BoxGroup("Restrictions")]
    [ShowIf("hasCastTime")]
    public bool showCastbar = true;

    [BoxGroup("Restrictions")]
    [SerializeField]
    private bool hasMaxAmount = false;

    [BoxGroup("Restrictions")]
    [ShowIf("hasMaxAmount")]
    [SerializeField]
    private int maxAmount = 1;

    [BoxGroup("Restrictions")]
    [SerializeField]
    public AbilityRequirements requirements = AbilityRequirements.none;

    [BoxGroup("Booleans")]
    [SerializeField]
    public bool isRapidFire = false;

    [BoxGroup("Booleans")]
    [SerializeField]
    public bool remoteActivation = false;

    [BoxGroup("Booleans")]
    [HideIf("isRapidFire")]
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


#if UNITY_EDITOR
    [AssetIcon]
    private Sprite GetSprite()
    {
        if (this.hasSkillBookInfo && this.info != null) return this.info.icon;
        return null;
    }

    private void OnCastTimeChange()
    {
        if(this.hasCastTime) this.deactivateButtonUp = false;
    }
#endif




    public string GetName()
    {
        return FormatUtil.getLanguageDialogText(this.abilityName, this.abilityNameEnglish);
    }

    #region Update Functions

    public void Initialize()
    {
        setStartParameters();
    }

    public void Update()
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
        if (this.isRapidFire && this.deactivateButtonUp) this.deactivateButtonUp = false;

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

    public void SetLockOnState()
    {
        if (!this.HasHelper()) this.state = AbilityState.lockOn;
    }

    public void ResetLockOn()
    {
        if (!this.HasHelper()) this.state = AbilityState.onCooldown;
    }

    public void ResetCharge()
    {
        if (!this.keepCast) this.holdTimer = 0;
        else if (this.keepCast && this.holdTimer > this.castTime) this.holdTimer = 0;
    }

    public void ResetCoolDown()
    {
        this.cooldownLeft = this.cooldown;
        this.state = AbilityState.onCooldown;
    }

    public bool CheckResourceAndAmount(Character character)
    {
        bool enoughResource = this.isResourceEnough(character);

        bool notToMany = true;
        if (this.hasMaxAmount) notToMany = (getAmountOfSameSkills(this.skill, character.activeSkills, character.activePets) < this.maxAmount);        

        return (notToMany && enoughResource);
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
        if (this.targetingProperty != null && this.targetingProperty.targetingMode != TargetingMode.helper) return true;
        return false;
    }

    public bool HasHelper()
    {
        if (this.targetingProperty != null && this.targetingProperty.targetingMode == TargetingMode.helper) return true;
        return false;
    }

    private bool isResourceEnough(Character character)
    {
        SkillSenderModule senderModule = this.skill.GetComponent<SkillSenderModule>();
        if (senderModule != null)
        {
            return character.canUseIt(senderModule.costs);
        }
        else return true;        
    }

    #endregion

}
