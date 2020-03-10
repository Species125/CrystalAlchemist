﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerAbilities : MonoBehaviour
{
    //[SerializeField]
    private List<Ability> abilities = new List<Ability>();

    [SerializeField]
    [Required]
    private Player player;

    [SerializeField]
    private Ability AButton;

    [SerializeField]
    private CastBar castbar;

    [SerializeField]
    private TargetingSystem targetingSystem;

    public CastBar activeCastbar;

    private void Start()
    {
        this.targetingSystem.gameObject.SetActive(false);

        this.AButton.state = AbilityState.ready;
        //this.AButton = Instantiate(this.AButton);
    }

    private void Update()
    {
        this.AButton.Update();

        if (this.player.currentState != CharacterState.interact
                     && this.player.currentState != CharacterState.inDialog
                     && this.player.currentState != CharacterState.respawning
                     && this.player.currentState != CharacterState.inMenu
                     && !CustomUtilities.StatusEffectUtil.isCharacterStunned(this.player))
        {
            useSkill(this.AButton, "AButton");
        }
    }

    private void useSkill(Ability ability, string button)
    {
        if (Input.GetButton(button)) //hold Button
        {
            if (ability.state == AbilityState.notCharged) ChargeAbility(ability);
            else if (ability.isRapidFire)
            {
                if (ability.state == AbilityState.charged) UseAbility(ability); //use rapidFire when charged
                else if (ability.state == AbilityState.ready) UseAbility(ability); //use rapidFire
                else if (ability.state == AbilityState.targetRequired) showTargetingSystem(ability); //show TargetingSystem
                else if (ability.state == AbilityState.lockOn) UseAbilityOnTargets(ability, false); //use TargetingSystem rapidfire
            }
        }
        if (Input.GetButtonUp(button)) //release Button
        {
            UnChargeAbility(ability);

            if (ability.state == AbilityState.charged && !ability.isRapidFire) UseAbility(ability); //use Skill when charged
            else if (ability.state == AbilityState.lockOn && ability.isRapidFire) hideTargetingSystem(ability, 1); //hide Targeting System when released         
        }
        else if (Input.GetButtonDown(button)) //press Button
        {
            if (ability.state == AbilityState.ready) UseAbility(ability); //use Skill
            else if (ability.state == AbilityState.targetRequired) showTargetingSystem(ability); //activate Targeting System
            else if (ability.state == AbilityState.lockOn) UseAbilityOnTargets(ability, true);//use Skill on locked Targets and hide Targeting System
        }
    }















    private void ChargeAbility(Ability ability)
    {
        ability.Charge(); //charge Skill when not full

        if (this.activeCastbar == null && ability.castTime > 0)
        {
            this.activeCastbar = Instantiate(this.castbar, this.player.transform.position, Quaternion.identity);
            this.activeCastbar.setCastBar(this.player, ability);
        }

        //Speed
        //Indicators
        //Animations
    }

    private void UnChargeAbility(Ability ability)
    {
        ability.ResetCharge(); //reset charge when not full   
        if (this.activeCastbar != null) this.activeCastbar.destroyIt();

        //Speed
        //Indicators
        //Animations
    }

    private void showTargetingSystem(Ability ability)
    {
        if (!this.targetingSystem.gameObject.activeInHierarchy)
        {
            ability.state = AbilityState.lockOn;
            this.targetingSystem.setParameters(ability);
            this.targetingSystem.gameObject.SetActive(true);
        }
    }


    #region useAbility

    private void UseAbility(Ability ability)
    {
        if (amountOfAbilities(ability))
        {
            CustomUtilities.Skills.instantiateSkill(ability.skill, this.player);
            ability.ResetCoolDown();
        }
    }

    private void UseAbilityOnTargets(Ability ability, bool hideTargetingSystem)
    {
        List<Character> targets = new List<Character>();
        targets.AddRange(this.targetingSystem.getTargets());
        if (hideTargetingSystem) this.hideTargetingSystem(ability, targets.Count);

        if(amountOfAbilities(ability)) StartCoroutine(useSkill(ability, targets));
    }

    private IEnumerator useSkill(Ability ability, List<Character> targets)
    {
        float damageReduce = targets.Count;
        int i = 0;

        foreach (Character target in targets)
        {
            if (target.currentState != CharacterState.dead
                && target.currentState != CharacterState.respawning)
            {
                CustomUtilities.Skills.instantiateSkill(ability.skill, this.player, target, damageReduce);
                yield return new WaitForSeconds(this.targetingSystem.getDelay());
            }
            i++;
        }
    }

    private void hideTargetingSystem(Ability ability, int count)
    {
        this.targetingSystem.gameObject.SetActive(false);

        if (count > 0) ability.ResetCoolDown(); //reset cooldown only when targets attacked
        else ability.state = AbilityState.targetRequired;
    }

    #endregion


    private bool amountOfAbilities(Ability ability)
    {
        return (CustomUtilities.Skills.getAmountOfSameSkills(ability.skill, this.player.activeSkills, this.player.activePets) <= ability.maxAmount);
    }
}
