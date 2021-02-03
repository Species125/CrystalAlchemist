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

                loadInventory(data.keyItems, data.inventoryItems, saveGame.inventory);
                loadPlayerSkills(data, saveGame.buttons, saveGame.skillSet);
                LoadProgress(data, saveGame.progress);

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
            foreach (string name in data.teleportPoints)
            {
                TeleportStats teleport = MasterManager.GetTeleportStats(name);
                if (teleport != null) list.AddTeleport(teleport);
            }

            list.SetNextTeleport(MasterManager.GetTeleportStats(data.startTeleport));
            list.SetReturnTeleport(MasterManager.GetTeleportStats(data.lastTeleport));
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

            attributes.SetValues();
        }

        public static void loadPlayerSkills(PlayerData data, PlayerButtons buttons, PlayerSkillset skillSet)
        {
            skillSet.Initialize();

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

        private static void loadInventory(List<string> keyItems, List<string[]> inventoryItems, PlayerInventory inventory)
        {
            if (keyItems != null)
            {
                foreach (string keyItem in keyItems)
                {
                    ItemDrop master = MasterManager.getItemDrop(keyItem);
                    if (master == null)
                    {
                        Debug.Log(keyItem + " is missing in master");
                        continue;
                    }

                    ItemDrop drop = master.Instantiate(1);
                    inventory.collectItem(drop.stats);
                    UnityEngine.Object.Destroy(drop);
                }
            }

            if (inventoryItems != null)
            {
                foreach (string[] item in inventoryItems)
                {
                    ItemGroup master = MasterManager.getItemGroup(item[0]);

                    if (master == null)
                    {
                        Debug.Log(item[0] + " is missing in master");
                        continue;
                    }
                    inventory.collectItem(master, Convert.ToInt32(item[1]));
                }
            }
        }
    }
}
