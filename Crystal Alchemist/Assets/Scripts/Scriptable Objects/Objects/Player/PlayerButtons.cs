﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum enumButton
{
    AButton,
    BButton,
    XButton,
    YButton,
    RBButton,
    LBButton
}

[System.Serializable]
public class PlayerButton
{
    public enumButton buttonType;
    public Ability ability;
}

[CreateAssetMenu(menuName = "Game/Player/Player Buttons")]
public class PlayerButtons : ScriptableObject
{
    [SerializeField]
    private List<PlayerButton> buttons = new List<PlayerButton>();

    [BoxGroup]
    public Ability currentAbility;

    public void Clear()
    {
        foreach(PlayerButton button in this.buttons) button.ability = null;        
        this.currentAbility = null;
    }

    public void Updating(Player player)
    {        
        bool canFight = player.values.CanUseAbilities();

        foreach (PlayerButton playerButton in this.buttons)
        {
            if (playerButton.ability != null)
            {
                if ((this.currentAbility == null
                  || this.currentAbility == playerButton.ability)
                    && playerButton.ability.HasEnoughResourceAndAmount()
                    && canFight) playerButton.ability.enabled = true;
                else if (this.currentAbility != playerButton.ability
                    || !canFight
                    || !playerButton.ability.HasEnoughResourceAndAmount())
                    playerButton.ability.enabled = false;
            }
        }
    }

    public Ability GetAbilityFromButton(enumButton button)
    {
        foreach (PlayerButton playerButton in this.buttons)
        {
            if (playerButton.buttonType == button) return playerButton.ability;
        }

        return null;
    }

    public void SetAbilityToButton(Ability ability, enumButton button)
    {
        foreach (PlayerButton playerButton in this.buttons)
        {
            if (playerButton.buttonType == button) playerButton.ability = ability;
        }
    }

    public void SetAbilityToButton(string button, Ability ability)
    {
        foreach (PlayerButton playerButton in this.buttons)
        {
            if (playerButton.buttonType.ToString().Replace("Button", "") == button) playerButton.ability = ability;
        }
    }

    public List<string[]> saveButtonConfig()
    {
        List<string[]> result = new List<string[]>();

        foreach (PlayerButton playerButton in this.buttons)
        {
            if (playerButton.ability != null)
                result.Add(new string[] { playerButton.buttonType.ToString().Replace("Button", ""), playerButton.ability.name });
        }

        return result;
    }
}
