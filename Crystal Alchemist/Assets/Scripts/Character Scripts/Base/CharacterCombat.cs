using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterCombat : MonoBehaviour
{
    private CastBar activeCastBar;
    private TargetingSystem targetingSystem;

    [HideInInspector]
    public Character character;

    public virtual void Initialize()
    {
        this.character = this.GetComponent<Character>();
        this.targetingSystem = Instantiate(MasterManager.targetingSystem, this.character.transform.position, Quaternion.identity, this.character.transform);
        this.targetingSystem.Initialize(this.character);
        this.targetingSystem.name = MasterManager.targetingSystem.name;
        this.targetingSystem.gameObject.SetActive(false);
    }

    public virtual void Updating() { }

    public TargetingSystem GetTargetingSystem()
    {
        return this.targetingSystem;
    }

    public void SetTimeValue(FloatValue timeValue)
    {
        if(this.targetingSystem != null) this.targetingSystem.SetTimeValue(timeValue);
    }

    public void ChargeAbility(Ability ability)
    {
        ChargeAbility(ability, null);
    }

    public void ChargeAbility(Ability ability, Character target)
    {
        ability.Charge(); //charge Skill when not full        
        ShowCastBar(ability); //Show Castbar
        ability.ShowCastingAnimation(); //Show Animation and stuff
        setSpeedDuringCasting(ability); //Set Speed during casting
        ability.ShowCastingIndicator(target);
    }

    public void UnChargeAbility(Ability ability)
    {
        ability.ResetCharge(); //reset charge when not full  
        HideCastBar(); //Hide Castbar
        ability.HideCastingAnimation(); //Hide Animation and stuff
        deactivatePlayerButtonUp(ability); //deactivate Skill when button up, Player only
        resetSpeedAfterCasting(); //set Speed to normal
        ability.HideIndicator();
    }

    private void setSpeedDuringCasting(Ability ability)
    {
        SkillSenderModule senderModule = ability.skill.GetComponent<SkillSenderModule>();
        if (senderModule != null) character.updateSpeed(senderModule.speedDuringCasting, senderModule.affectAnimation);
    }

    private void resetSpeedAfterCasting()
    {
        character.updateSpeed(0); //Set Speed to normal
    }

    private void deactivatePlayerButtonUp(Ability ability)
    {
        if (ability.deactivateButtonUp 
         && character.GetComponent<Player>() != null)
            deactivateSkill(ability);
    }

    private void deactivateSkill(Ability ability)
    {
        int deactivatedSkills = DeactivateSkills(ability);
        if (deactivatedSkills > 0) ability.ResetCoolDown(); //prevent Cooldown when not used skill
    }

    private int DeactivateSkills(Ability ability)
    {
        int counter = 0;

        for(int i = 0; i < character.values.activeSkills.Count; i++)
        {
            if (character.values.activeSkills[i].name == ability.skill.name)
            {
                character.values.activeSkills[i].DeactivateIt();
                counter++;
            }
        }

        return counter;
    }


    public void ShowCastBar(Ability ability)
    {
        if (this.activeCastBar == null && ability.showCastbar && ability.hasCastTime)
        {
            this.activeCastBar = Instantiate(MasterManager.castBar, character.GetHeadPosition(), Quaternion.identity);
            this.activeCastBar.setCastBar(character, ability);
        }
    }

    public void HideCastBar()
    {
        if (this.activeCastBar != null) this.activeCastBar.destroyIt();
    }


    public virtual void ShowTargetingSystem(Ability ability)
    {
        if (this.targetingSystem != null &&  !this.targetingSystem.gameObject.activeInHierarchy)
        {
            this.targetingSystem.setParameters(ability);
            this.targetingSystem.gameObject.SetActive(true);
        }
    }

    public void HideTargetingSystem(Ability ability)
    {
        if (this.targetingSystem != null && this.targetingSystem.gameObject.activeInHierarchy)
        {
            this.targetingSystem.Deactivate();
        }
    }

    public float GetTargetingDelay()
    {
        if (this.targetingSystem != null) return this.targetingSystem.getDelay();
        return 0f;
    }

    public virtual List<Character> GetTargetsFromTargeting()
    {
        if (this.targetingSystem != null) return this.targetingSystem.getTargets();
        return null;
    }

    #region useAbility

    public void UseAbilityOnTarget(Ability ability, Character target)
    {
        if (ability.HasEnoughResourceAndAmount())
        {
            Skill skill = ability.InstantiateSkill(target);            
            if (!ability.deactivateButtonUp && !ability.remoteActivation) ability.ResetCoolDown();
        }
    }

    public void UseAbilityOnTargets(Ability ability)
    {
        List<Character> targets = new List<Character>();
        targets.AddRange(this.GetTargetsFromTargeting());

        if(targets.Count > 0) ability.ResetCoolDown();

        if (ability.HasEnoughResourceAndAmount())
        {
            StartCoroutine(useSkill(ability, targets));
        }
    }

    private IEnumerator useSkill(Ability ability, List<Character> targets)
    {
        float damageReduce = targets.Count;

        foreach (Character target in targets)
        {
            if (target.values.currentState != CharacterState.dead
                && target.values.currentState != CharacterState.respawning)
            {
                GameObject skill = ability.InstantiateSkill(target, damageReduce).gameObject;

                yield return new WaitForSeconds(this.GetTargetingDelay());
            }
        }
    }

    #endregion
}
