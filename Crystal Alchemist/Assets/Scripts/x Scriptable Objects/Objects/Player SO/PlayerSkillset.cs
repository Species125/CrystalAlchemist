﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Player Skillset")]
public class PlayerSkillset : ScriptableObject
{
    [SerializeField]
    private List<Ability> abilities = new List<Ability>();

    public void Start()
    {
        foreach (Ability ability in this.abilities)
        {
            ability.Initialize();
            //this.AButton = Instantiate(this.AButton);
        }
    }

    public void Update()
    {
        foreach(Ability ability in this.abilities)
        {
            ability.Update();
        }
    }

    public Ability getAbilityByName(string name)
    {
        foreach (Ability ability in this.abilities)
        {
            if (name == ability.skill.skillName) return ability;
        }

        return null;
    }

    public Ability getSkillByID(int ID, SkillType category)
    {
        foreach (Ability ability in this.abilities)
        {
            SkillBookInfo info = ability.info;

            if (info != null
                && category == info.category
                && ID == info.orderIndex) return ability;
        }

        return null;
    }

    public void AddAbility(Ability ability)
    {
        this.abilities.Add(ability);
    }
}
