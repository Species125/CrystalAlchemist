﻿using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSystem
{
    public static void loadPlayerData(PlayerSaveGame saveGame, PlayerData data)
    {
        if (data != null)
        {
            LoadPreset(data, saveGame.playerPreset);
            LoadBasicValues(data, saveGame.playerValue, saveGame.attributes);

            saveGame.time.Clear();
            saveGame.timePlayed.SetValue(data.timePlayed);
            saveGame.SetCharacterName(data.characterName);

            loadInventory(data, saveGame.inventory);
            loadPlayerSkills(data, saveGame.buttons, saveGame.skillSet);
            LoadProgress(data, saveGame.progress);

            LoadTeleportList(data, saveGame.teleportList);
        }
    }

    private static void LoadProgress(PlayerData data, PlayerGameProgress progress)
    {
        progress.Clear();
        progress.SetPermanent(data.progress);
    }

    private static void LoadTeleportList(PlayerData data, PlayerTeleportList list)
    {
        if (data.teleportPoints == null) return;
        foreach(string name in data.teleportPoints)
        {
            TeleportStats teleport = MasterManager.GetTeleportStats(name);
            if (teleport != null) list.AddTeleport(teleport);
        }

        list.nextTeleport = MasterManager.GetTeleportStats(data.startTeleport);
        list.lastTeleport = MasterManager.GetTeleportStats(data.lastTeleport);
    }

    private static void LoadBasicValues(PlayerData data, CharacterValues playerValue, PlayerAttributes attributes)
    {
        playerValue.life = data.health;
        playerValue.mana = data.mana;

        attributes.SetPoints(attributeType.lifeExpander, data.maxHealth);
        attributes.SetPoints(attributeType.lifeRegen, data.healthRegen);
        attributes.SetPoints(attributeType.manaExpander, data.maxMana);
        attributes.SetPoints(attributeType.manaRegen, data.manaRegen);
        attributes.SetPoints(attributeType.buffPlus, data.buffplus);
        attributes.SetPoints(attributeType.debuffMinus, data.debuffminus);
    }

    public static void loadPlayerSkills(PlayerData data, PlayerButtons buttons, PlayerSkillset skillSet)
    {
        skillSet.Initialize();

        if (data != null && data.abilities.Count > 0)
        {
            foreach(string[] elem in data.abilities)
            {
                string name = elem[1];
                string button = elem[0];

                Ability ability = skillSet.getAbilityByName(name);
                buttons.SetAbilityToButton(button, ability);
            }
        }
    }

    private static void LoadPreset(PlayerData data, CharacterPreset savedPreset)
    { 
        if (data != null && data.characterParts != null && data.characterParts.Count > 0)
        {
            loadPresetData(data, savedPreset); //set Preset
        }
    }

    private static void loadPresetData(PlayerData data, CharacterPreset newPreset)
    {
        CharacterPreset preset = newPreset;

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


    private static void loadInventory(PlayerData data, PlayerInventory inventory)
    {
        if (data.keyItems != null)
        {
            foreach (string keyItem in data.keyItems)
            {
                ItemDrop master = MasterManager.getItemDrop(keyItem);
                if (master != null)
                {
                    ItemDrop drop = master.Instantiate(1);
                    inventory.collectItem(drop.stats);
                    MonoBehaviour.Destroy(drop);
                }
            }
        }

        if (data.inventoryItems != null)
        {
            foreach (string[] item in data.inventoryItems)
            {
                ItemGroup master = MasterManager.getItemGroup(item[0]);
                if (master != null) inventory.collectItem(master, Convert.ToInt32(item[1]));                
            }
        }
    }    
}
