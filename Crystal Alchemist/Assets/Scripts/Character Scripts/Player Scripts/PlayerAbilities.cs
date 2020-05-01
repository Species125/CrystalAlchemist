﻿using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using System;

public class PlayerAbilities : CharacterCombat
{
    public PlayerSkillset skillSet;
    public PlayerButtons buttons;

    [SerializeField]
    [Required]
    private FloatValue timeLeftValue;

    private Player player;
    private bool isPressed;
    private float timer;

    public void Initialize(Player player)
    {
        this.player = player;
        InitializeTargeting(this.player);
        this.SetTimeValue(this.timeLeftValue);
        this.skillSet.SetSender(player);
    }

    public void Updating()
    {
        this.skillSet.Updating();
        this.buttons.Updating(this.player);

        if (this.isPressed) ButtonHold(this.buttons.currentAbility);
    }

    public void SelectTargetInput(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) this.GetTargetingSystem().SetTargetChange(ctx.ReadValue<Vector2>());
    }

    public void OnHoldingCallback(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            this.isPressed = true;
            this.buttons.currentAbility = GetAbility(context);
            ButtonDown(this.buttons.currentAbility);
        }
        else if (context.canceled)
        {
            this.isPressed = false;
            ButtonUp(this.buttons.currentAbility);
            this.buttons.currentAbility = null;
        }
    }
       
    private Ability GetAbility(InputAction.CallbackContext context)
    {
        foreach (enumButton item in Enum.GetValues(typeof(enumButton)))
        {
            if (item.ToString().ToUpper() == context.action.name.ToString().ToUpper())
                return this.buttons.GetAbilityFromButton(item);
        }

        return null;
    }

    private void ButtonHold(Ability ability)
    {
        if (ability == null || !ability.enabled) return;
        if (ability.state == AbilityState.notCharged)
        {
            ChargeAbility(ability, this.player);
        }
        else if (ability.isRapidFire)
        {
            if (ability.state == AbilityState.charged) UseAbilityOnTarget(ability, null); //use rapidFire when charged
            else if (ability.state == AbilityState.ready) UseAbilityOnTarget(ability, null); //use rapidFire
            else if (ability.state == AbilityState.targetRequired) ShowTargetingSystem(ability); //show TargetingSystem
            else if (ability.state == AbilityState.lockOn) UseAbilityOnTargets(ability); //use TargetingSystem rapidfire  
        }
    }

    private void ButtonUp(Ability ability)
    {
        if (ability == null || !ability.enabled) return;
        if (ability.state == AbilityState.charged && !ability.isRapidFire) UseAbilityOnTarget(ability, null); //use Skill when charged
        else if (ability.state == AbilityState.lockOn && ability.isRapidFire) HideTargetingSystem(ability); //hide Targeting System when released

        UnChargeAbility(ability, this.player);
    }

    private void ButtonDown(Ability ability)
    {
        if (ability == null || !ability.enabled) return;
        if (ability.state == AbilityState.ready) UseAbilityOnTarget(ability, null); //use Skill
        else if (ability.state == AbilityState.targetRequired) ShowTargetingSystem(ability); //activate Targeting System
        else if (ability.state == AbilityState.lockOn)
        {
            UseAbilityOnTargets(ability);//use Skill on locked Targets and hide Targeting System 
            HideTargetingSystem(ability);
        }
    }
}
