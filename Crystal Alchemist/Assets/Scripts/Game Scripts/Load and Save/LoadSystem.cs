﻿using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSystem : MonoBehaviour
{
    public static void loadPlayerData(Player player, string saveGameSlot)
    {
        PlayerData data = SaveSystem.loadPlayer(saveGameSlot);
        PlayerAbilities playerAbilities = player.GetComponent<PlayerAbilities>();
        PlayerTeleport playerTeleport = player.GetComponent<PlayerTeleport>();

        playerAbilities.skillSet.Start();

        playerAbilities.buttons.ClearAbilities();
        loadPreset(player, saveGameSlot);
        player.presetSignal.Raise();

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

            player.stats.characterName = data.characterName;
            player.secondsPlayed.setValue(data.timePlayed);            

            loadInventory(data, player);
            loadPlayerSkills(playerAbilities, saveGameSlot);

            playerTeleport.setLastTeleport(data.scene, new Vector3(data.position[0], data.position[1], data.position[2]), true);
            playerTeleport.teleportPlayerToLastSavepoint(true); //letzter Savepoint, no Scene Loading
        }
        else
        {
            playerTeleport.setLastTeleport("", Vector2.zero, false);
            playerTeleport.teleportPlayerToScene(true); //Starpunkt, no Scene Loading
        }
    }

    public static void loadPlayerSkills(PlayerAbilities playerAbilities, string saveGameSlot)
    {
        PlayerData data = SaveSystem.loadPlayer(saveGameSlot);

        if (data != null && data.abilities.Count > 0)
        {
            foreach(string[] elem in data.abilities)
            {
                string name = elem[1];
                string button = elem[0];

                Ability ability = playerAbilities.skillSet.getAbilityByName(name);
                playerAbilities.buttons.SetAbilityToButton(button, ability);
            }
        }
    }

    private static void loadPreset(Player player, string saveGameSlot)
    {
        PlayerData data = SaveSystem.loadPlayer(saveGameSlot);

        if (data != null && data.characterParts != null && data.characterParts.Count > 0)
        {
            loadPresetData(data, player);
        }
        else
        {
            GameUtil.setPreset(player.defaultPreset, player.preset);
        }
    }

    private static void loadPresetData(PlayerData data, Player player)
    {
        player.stats.characterName = data.characterName;

        CharacterPreset preset = player.preset;
        preset.characterName = data.characterName;

        if (Enum.TryParse(data.race, out Race race)) preset.setRace(race);

        List<ColorGroupData> colorGroups = new List<ColorGroupData>();

        foreach(string[] colorGroup in data.colorGroups)
        {
            string colorGroupName = colorGroup[0];
            float r = float.Parse(colorGroup[1]);
            float g = float.Parse(colorGroup[2]);
            float b = float.Parse(colorGroup[3]);
            float a = float.Parse(colorGroup[4]);

            Color color = new Color(r,g,b,a);
            if (Enum.TryParse(colorGroup[0], out ColorGroup group)) colorGroups.Add(new ColorGroupData(group, color));
        }

        preset.AddColorGroupRange(colorGroups);

        List<CharacterPartData> parts = new List<CharacterPartData>();

        foreach (string[] characterPart in data.characterParts)
        {
            string parentName = characterPart[0];
            string name = characterPart[1];

            parts.Add(new CharacterPartData(parentName, name));
        }

        preset.AddCharacterPartDataRange(parts);
    }


    private static void loadInventory(PlayerData data, Player player)
    {
        if (data.keyItems != null)
        {
            foreach (string keyItem in data.keyItems)
            {
                ItemDrop drop = Resources.Load("Scriptable Objects/Items/Item Drops/Key Items/" + keyItem, typeof(ItemDrop)) as ItemDrop;
                ItemStats stats = Instantiate(drop.stats);
                stats.name = drop.name;
                stats.CollectIt(player);
            }
        }

        if (data.inventoryItems != null)
        {
            foreach (string[] item in data.inventoryItems)
            {
                ItemGroup group = Resources.Load("Scriptable Objects/Items/Item Groups/Inventory Items/" + item[0], typeof(ItemGroup)) as ItemGroup;
                if (group == null) group = Resources.Load("Scriptable Objects/Items/Item Groups/Currencies/" + item[0], typeof(ItemGroup)) as ItemGroup;
                if (group != null) player.GetComponent<PlayerItems>().CollectInventoryItem(group, Convert.ToInt32(item[1]));
            }
        }
    }

    
}
