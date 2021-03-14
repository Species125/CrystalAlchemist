using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class LoadSystem
    {
        public static void loadPlayerData(PlayerSaveGame saveGame, PlayerData data, Action callback)
        {
            if (data != null)
            {
                LoadPreset(data, saveGame.playerPreset);
                LoadBasicValues(data, saveGame.playerValue, saveGame.attributes);

                saveGame.time.Clear();
                saveGame.timePlayed.SetValue(data.timePlayed);
                saveGame.SetCharacterName(data.characterName);

                //LoadRecipes(data.recipes, saveGame.RecipesList);
                LoadSkillSet(data.skillset, saveGame.skillSet);

                LoadItems(data.treasureItems, saveGame.inventory);
                LoadItems(data.keyItems, saveGame.inventory);
                LoadItems(data.currencies, saveGame.inventory);
                LoadItems(data.inventoryItems, saveGame.inventory);
                LoadItems(data.craftingItems, saveGame.inventory);
                LoadItems(data.housingItems, saveGame.inventory);

                LoadPlayerSkills(data, saveGame.buttons, saveGame.skillSet);
                LoadProgress(data, saveGame.progress);
                LoadOutfits(data.outfits, saveGame.outfits);

                LoadTeleportList(data, saveGame.teleportList);

                Debug.Log("Savegame loaded");
            }

            callback?.Invoke();
        }

        private static void LoadProgress(PlayerData data, PlayerGameProgress progress)
        {
            progress.Clear();

            try
            {
                foreach (string[] elem in data.progress)
                {
                    string location = elem[0];
                    string key = elem[1];
                    UDateTime date = new UDateTime(elem[2]);
                    UTimeSpan span = new UTimeSpan(elem[3]);
                    bool parse = Enum.TryParse(elem[4], out ProgressType type);

                    if (parse) progress.AddProgress(location, key, type, date, span);
                }
            }
            catch { }
        }

        private static void LoadTeleportList(PlayerData data, PlayerTeleportList list)
        {
            if (data.teleportPoints == null) return;
            foreach (string[] name in data.teleportPoints)
            {
                TeleportStats teleport = MasterManager.GetTeleportStats(name);
                if (teleport != null) list.AddTeleport(teleport);
            }

            TeleportStats startTeleport = MasterManager.GetTeleportStats(data.startTeleport);
            if (startTeleport == null) startTeleport = MasterManager.defaultTeleport;

            list.SetNextTeleport(startTeleport, true, true);
            list.SetReturnTeleport(MasterManager.GetTeleportStats(data.lastTeleport));
        }

        private static void LoadBasicValues(PlayerData data, CharacterValues playerValue, PlayerAttributes attributes)
        {
            playerValue.life = data.health;
            playerValue.mana = data.mana;
            if (playerValue.mana < 0) playerValue.mana = 0;

            attributes.SetPoints(attributeType.lifeExpander, data.maxHealth);
            attributes.SetPoints(attributeType.lifeRegen, data.healthRegen);
            attributes.SetPoints(attributeType.manaExpander, data.maxMana);
            attributes.SetPoints(attributeType.manaRegen, data.manaRegen);
            attributes.SetPoints(attributeType.buffPlus, data.buffplus);
            attributes.SetPoints(attributeType.debuffMinus, data.debuffminus);

            attributes.SetValues();
        }

        public static void LoadPlayerSkills(PlayerData data, PlayerButtons buttons, PlayerSkillset skillSet)
        {
            if (data != null && data.abilities.Count > 0)
            {
                foreach (string[] elem in data.abilities)
                {
                    string name = elem[1];
                    string button = elem[0];

                    Ability ability = skillSet.getAbilityByName(name);
                    buttons.SetAbilityToButton(button, ability);
                }
            }
        }

        private static void LoadPreset(PlayerData data, CharacterPreset preset)
        {
            if (data != null && data.characterParts != null && data.characterParts.Length > 0)
            {
                SerializationUtil.SetPreset(preset, data.race, data.colorGroups, data.characterParts);
            }
        }

        private static void LoadSkillSet(List<string> abilites, PlayerSkillset skillset)
        {
            if (abilites != null)
            {
                foreach (string abilityName in abilites)
                {
                    Ability master = MasterManager.GetAbility(abilityName);
                    if (master == null)
                    {
                        Debug.Log(master + " is missing in master");
                        continue;
                    }

                    skillset.AddAbility(master);
                }
            }
        }

        private static void LoadOutfits(List<string[]> outfits, PlayerOutfits playerOutfits)
        {
            if (outfits != null)
            {
                foreach (string[] outfitdata in outfits)
                {
                    CharacterCreatorProperty master = MasterManager.GetOutfit(outfitdata);
                    if (master == null)
                    {
                        Debug.Log(master + " is missing in master");
                        continue;
                    }

                    playerOutfits.AddGear(master);
                }
            }
        }

        private static void LoadItems(List<string[]> items, PlayerInventory inventory)
        {
            if (items != null)  //Key Items
            {
                foreach (string[] item in items)
                {
                    InventoryItem master = MasterManager.GetItemGroup(item[0]);

                    if (master == null)
                    {
                        Debug.Log(item[0] + " is missing in master");
                        continue;
                    }
                    inventory.CollectItem(master, Convert.ToInt32(item[1]), inventory.GetInventory(master.inventoryType));
                }
            }
        }        
    }
}
