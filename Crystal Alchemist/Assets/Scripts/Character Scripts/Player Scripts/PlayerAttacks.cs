﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum enumButton
{
    AButton,
    BButton,
    XButton,
    YButton,
    RBButton
}

public class PlayerAttacks : MonoBehaviour
{
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private CastBar castbar;

    [SerializeField]
    private Player player;
    
    [HideInInspector]
    public string currentButtonPressed = "";

    private void Start()
    {
        List<Skill> tempSkillSet = new List<Skill>();

        foreach (Skill skill in this.player.skillSet)
        {
            tempSkillSet.Add(CustomUtilities.Skills.setSkill(this.player, skill));
        }

        this.player.skillSet = tempSkillSet;

        if (this.player.loadGame.getValue()) LoadSystem.loadPlayerSkills(this.player);
    }

    public void loadSkillsFromSkillSet(string name, enumButton button)
    {
        foreach (Skill skill in this.player.skillSet)
        {
            if (skill.skillName == name)
            {
                switch (button)
                {
                    case enumButton.AButton: this.player.AButton = skill; break;
                    case enumButton.BButton: this.player.BButton = skill; break;
                    case enumButton.XButton: this.player.XButton = skill; break;
                    case enumButton.YButton: this.player.YButton = skill; break;
                    case enumButton.RBButton: this.player.RBButton = skill; break;
                }

                break;
            }
        }
    }

    private Skill getSkillFromButton(string button)
    {
        //TODO: GEHT BESSER!

        switch (button)
        {
            case "A-Button": return this.player.AButton;
            case "B-Button": return this.player.BButton;
            case "X-Button": return this.player.XButton;
            case "Y-Button": return this.player.YButton;
            case "RB-Button": return this.player.RBButton;
            default: return null;
        }
    }

    public void updateSkillButtons(string button)
    {
        Skill skill = this.getSkillFromButton(button);

        if (skill != null)
        {
            int currentAmountOfSameSkills = CustomUtilities.Skills.getAmountOfSameSkills(skill, this.player.activeSkills, this.player.activePets);

            if (currentAmountOfSameSkills >= skill.maxAmounts
                && ((skill.deactivateByButtonUp || skill.deactivateByButtonDown)
                || skill.delay == CustomUtilities.maxFloatInfinite))
            {
                deactivateSkill(button, skill);
            }
            else
            {
                if (skill.cooldownTimeLeft > 0)
                {
                    skill.cooldownTimeLeft -= (Time.deltaTime * this.player.timeDistortion * this.player.spellspeed);
                }
                else if (this.player.currentState != CharacterState.interact
                     && this.player.currentState != CharacterState.inDialog
                     && this.player.currentState != CharacterState.respawning
                     && this.player.currentState != CharacterState.inMenu
                     && !CustomUtilities.StatusEffectUtil.isCharacterStunned(this.player))
                {
                    if (currentAmountOfSameSkills < skill.maxAmounts
                            && skill.isResourceEnough(this.player)
                            && skill.basicRequirementsExists)
                    {
                        if (isSkillReadyToUse(button, skill))
                        {
                            activateSkill(button, skill); //activate Skill or Target System
                        }

                        activateSkillFromTargetingSystem(skill); //if Target System is ready
                    }
                }
            }

            if (CustomUtilities.StatusEffectUtil.isCharacterStunned(this.player))
            {
                if (!skill.keepHoldTimer) skill.holdTimer = 0;
            }
        }
    }

    private bool isSkillReadyToUse(string button, Skill skill)
    {
        if (isButtonUsable(button))
        {
            if (Input.GetButtonDown(button) && (skill.isRapidFire || skill.cast == 0))
            {
                setLastButtonPressed(button);

                if (skill.isRapidFire)
                {
                    this.player.resetCast(skill);
                }

                //Instants only (kein Cast und kein Rapidfire)
                if (skill.cast == 0) return true;
            }
            else if (Input.GetButton(button))
            {
                setLastButtonPressed(button);

                if (skill.GetComponent<SkillSenderModule>() != null && skill.GetComponent<SkillSenderModule>().speedDuringCasting != 0)
                    this.player.updateSpeed(skill.GetComponent<SkillSenderModule>().speedDuringCasting);

                if (skill.holdTimer < skill.cast)
                {
                    skill.holdTimer += (Time.deltaTime * this.player.timeDistortion * this.player.spellspeed);

                    if (skill.GetComponent<SkillIndicatorModule>() != null) skill.GetComponent<SkillIndicatorModule>().showIndicator(); //Zeige Indikator beim Casten+
                    if (skill.GetComponent<SkillAnimationModule>() != null) skill.GetComponent<SkillAnimationModule>().showCastingAnimation();

                    skill.doOnCast();
                }

                if (skill.holdTimer >= skill.cast && skill.isRapidFire)
                {
                    //Rapidfire oder Cast Rapidfire                        
                    return true;
                }

                if (skill.cast > 0
                    && skill.holdTimer > 0
                    && skill.holdTimer < skill.cast
                    && this.player.activeCastbar == null
                    && this.castbar != null
                    && this.player.activeLockOnTarget == null)
                {
                    GameObject temp = Instantiate(this.castbar.gameObject, this.transform.position, Quaternion.identity, this.transform);
                    //temp.hideFlags = HideFlags.HideInHierarchy;
                    this.player.activeCastbar = temp.GetComponent<CastBar>();
                    this.player.activeCastbar.target = this.player;
                    this.player.activeCastbar.skill = skill;
                }
                else if (skill.cast > 0
                    && skill.holdTimer >= skill.cast
                    && this.player.activeCastbar != null
                    && skill.isRapidFire)
                {
                    this.player.hideCastBarAndIndicator(skill);
                }
                else if (skill.cast > 0 && this.player.activeCastbar != null && skill.holdTimer > 0)
                {
                    this.player.activeCastbar.showCastBar();
                    if (skill.GetComponent<SkillIndicatorModule>() != null) skill.GetComponent<SkillIndicatorModule>().showIndicator();
                    if (skill.GetComponent<SkillAnimationModule>() != null) skill.GetComponent<SkillAnimationModule>().showCastingAnimation();
                }
            }
            else if (Input.GetButtonUp(button))
            {
                setLastButtonPressed(button);
                if (skill.GetComponent<SkillSenderModule>() != null && skill.GetComponent<SkillSenderModule>().speedDuringCasting != 0) this.player.updateSpeed(0);

                //Cast only
                if (skill.holdTimer >= skill.cast && skill.cast > 0)
                {
                    return true;
                }

                this.player.resetCast(skill);
            }
        }

        return false;
    }

    private void activateSkill(string button, Skill skill)
    {
        this.player.hideCastBarAndIndicator(skill);

        SkillTargetingSystemModule targetingSystemModule = skill.GetComponent<SkillTargetingSystemModule>();

        if (targetingSystemModule == null)
        {
            //Benutze Skill (ohne Zielerfassung)            
            skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown
            if (!skill.isRapidFire) skill.holdTimer = 0;

            CustomUtilities.Skills.instantiateSkill(skill, this.player);
        }
        else if (targetingSystemModule != null 
                && targetingSystemModule.lockOnGameObject != null 
                && this.player.activeLockOnTarget == null)
        {
            //Aktiviere Zielerfassung
            this.player.activeLockOnTarget = Instantiate(targetingSystemModule.lockOnGameObject, this.transform.position, Quaternion.identity, this.transform);
            this.player.activeLockOnTarget.setParameters(skill, button);
        }
    }

    private void activateSkillFromTargetingSystem(Skill skill)
    {
        if (this.player.activeLockOnTarget != null
            && this.player.activeLockOnTarget.isReadyToFire(skill))
        {
            //Benutze Skill (mit Zielerfassung)   
            if (!skill.isRapidFire) skill.holdTimer = 0;

            SkillTargetingSystemModule targetingSystemModule = skill.GetComponent<SkillTargetingSystemModule>();

            if (this.player.activeLockOnTarget.targets.Count == 0
                && targetingSystemModule.targetingMode != TargetingMode.auto)
            {
                this.player.activeLockOnTarget.DestroyIt();
            }
            else if (this.player.activeLockOnTarget.targets.Count > 0
                || targetingSystemModule.targetingMode == TargetingMode.auto)
            {
                skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown

                StartCoroutine(fireSkillToMultipleTargets(this.player.activeLockOnTarget, skill));
            }
        }
    }

    public void deactivateAllSkills()
    {
        for (int i = 0; i < this.player.activeSkills.Count; i++)
        {
            Skill activeSkill = this.player.activeSkills[i];
            activeSkill.durationTimeLeft = 0;
        }
    }

    private void deactivateSkill(string button, Skill skill)
    {
        //Skill deaktivieren
        bool destroyit = false;

        if (Input.GetButtonUp(button) && skill.deactivateByButtonUp)
        {
            destroyit = true;
        }
        else if (Input.GetButtonDown(button) && skill.deactivateByButtonDown)
        {
            destroyit = true;
        }

        if (destroyit)
        {
            for (int i = 0; i < this.player.activeSkills.Count; i++)
            {
                Skill activeSkill = this.player.activeSkills[i];
                if (activeSkill.skillName == skill.skillName)
                {
                    if (activeSkill.delay > 0) activeSkill.delayTimeLeft = 0; //C4
                    else activeSkill.durationTimeLeft = 0; //Schild
                }
            }
        }
    }

    private IEnumerator fireSkillToMultipleTargets(TargetingSystem targetingSystem, Skill skill)
    {
        float damageReduce = targetingSystem.targets.Count;
        int i = 0;

        foreach (Character target in targetingSystem.targets)
        {
            if (target.currentState != CharacterState.dead
                && target.currentState != CharacterState.respawning)
            {
                //bool playSoundEffect = false;
                //if (i == 0 || skill.GetComponent<SkillTargetingSystemModule>().multiHitDelay > 0.3f) playSoundEffect = true;

                fireSkillToSingleTarget(target, damageReduce, skill);

                yield return new WaitForSeconds(skill.GetComponent<SkillTargetingSystemModule>().multiHitDelay);
            }
            i++;
        }

        this.player.activeLockOnTarget.DestroyIt();
    }

    private void fireSkillToSingleTarget(Character target, float damageReduce, Skill skill)
    {
        Skill temp = CustomUtilities.Skills.instantiateSkill(skill, this.player, target, damageReduce);
        //Vermeidung, dass Audio zu stark abgespielt wird
        //if (!playSoundeffect) temp.dontPlayAudio = true;
    }

    public bool isButtonUsable(string button)
    {
        if (button == this.currentButtonPressed
            || this.currentButtonPressed == null
            || this.currentButtonPressed == "") return true;
        else return false;
    }

    private void setLastButtonPressed(string button)
    {
        if (this.currentButtonPressed == "") this.currentButtonPressed = button;
        /*
        if (this.lastButtonPressed != button)
        {
            //if (!skill.keepHoldTimer) skill.holdTimer = 0;
            this.lastButtonPressed = button;
        }*/
    }

}
