﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSystem : MonoBehaviour
{
    public static void loadPlayerData(Player player)
    {
        PlayerData data = SaveSystem.loadPlayer();

        if (data != null)
        {
            player.life = data.health;
            player.mana = data.mana;

            player.maxLife = data.maxHealth;
            player.maxMana = data.maxMana;
            player.lifeRegen = data.healthRegen;
            player.manaRegen = data.manaRegen;
            player.buffPlus = data.buffplus;
            player.debuffMinus = data.debuffminus;

            player.healthSignalUI.Raise();
            player.manaSignalUI.Raise();

            player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);

            player.GetComponent<PlayerTeleport>().setLastTeleport(data.scene, new Vector3(data.position[0], data.position[1], data.position[2]));

            if (data.inventory.Count > 0)
            {
                loadInventory(data, player);                
            }            
        }
    }

    public static void loadPlayerSkills(Player player)
    {
        PlayerData data = SaveSystem.loadPlayer();

        if (data.skills.Count > 0)
        {
            loadSkills(data, player);
        }
    }

    private static void loadInventory(PlayerData data, Player player)
    {
        player.inventory.Clear();

        foreach (string[] elem in data.inventory)
        {
            GameObject prefab = Resources.Load("Items/" + elem[0], typeof(GameObject)) as GameObject;

            if(prefab == null) prefab = Resources.Load("Items/Key Items/" + elem[0], typeof(GameObject)) as GameObject;

            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab);
                instance.name = prefab.name;
                Item item = instance.GetComponent<Item>();
                item.amount = Convert.ToInt32(elem[1]);
                player.collect(item, true, false);
            }
        }
    }

    private static void loadSkills(PlayerData data, Player player)
    {
        foreach (string[] elem in data.skills)
        {
            Skill skill = Utilities.Skills.getSkillByName(player.skillSet, elem[1]);
            string button = elem[0];

            switch (button)
            {
                case "A": player.AButton = skill; break;
                case "B": player.BButton = skill; break;
                case "X": player.XButton = skill; break;
                case "Y": player.YButton = skill; break;
                case "RB": player.RBButton = skill; break;
            }
        }
    }
}
