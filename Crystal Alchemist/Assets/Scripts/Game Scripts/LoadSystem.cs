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

            player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);

            if (data.inventory.Count > 0)
            {
                loadInventory(data, player);
                player.currencySignalUI.Raise();
            }

            if (data.skills.Count > 0)
            {
                loadSkills(data, player);
            }
        }
    }

    private static void loadInventory(PlayerData data, Player player)
    {
        foreach (string[] elem in data.inventory)
        {
            GameObject prefab = Resources.Load("Items/" + elem[0], typeof(GameObject)) as GameObject;
            GameObject instance = Instantiate(prefab);
            instance.name = prefab.name;
            Item item = instance.GetComponent<Item>();
            item.amount = Convert.ToInt32(elem[1]);
            player.collect(item, true);
        }
    }

    private static void loadSkills(PlayerData data, Player player)
    {
        foreach (string[] elem in data.skills)
        {
            StandardSkill skill = Utilities.getSkillByName(player.skillSet, elem[1]);
            string button = elem[0];

            switch (button)
            {
                case "A": player.AButton = skill; break;
                case "B": player.BButton = skill; break;
                case "X": player.XButton = skill; break;
                case "Y": player.YButton = skill; break;
            }
        }
    }
}