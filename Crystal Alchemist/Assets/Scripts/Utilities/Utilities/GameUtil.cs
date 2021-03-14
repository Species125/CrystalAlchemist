using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public static class GameUtil
    {
        public static ItemDrop GetHighestDrop(List<ItemDrop> itemDrops)
        {
            ItemDrop current = null;

            foreach (ItemDrop drop in itemDrops)
            {
                if (current == null || current.stats.rarity < drop.stats.rarity) current = drop;
            }

            return current;
        }

        public static bool IsInvincibleNPC(Character target)
        {
            return target.GetComponent<Player>() == null && target.values.isInvincible;
        }

        public static InventoryItem GetInventoryItem(InventoryItem itemGroup, List<InventoryItem> inventory)
        {
            if (itemGroup != null && inventory != null)
            {
                foreach (InventoryItem group in inventory)
                {
                    if (group != null && group.name == itemGroup.name) return group;
                }
            }
            return null;
        }

        public static void SetPreset(CharacterPreset source, CharacterPreset target)
        {
            target.setRace(source.getRace());
            target.AddColorGroupRange(source.GetColorGroupRange());
            target.AddProperty(source.GetProperties());
        }

        public static float setResource(float resource, float max, float addResource)
        {
            if (addResource != 0)
            {
                if (resource + addResource > max) addResource = max - resource;
                else if (resource + addResource < 0) resource = 0;

                resource += addResource;
            }

            return resource;
        }

        public static Color GetRarity(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.uncommon: return MasterManager.globalValues.uncommon;
                case ItemRarity.epic: return MasterManager.globalValues.epic;
                case ItemRarity.rare: return MasterManager.globalValues.rare;
                case ItemRarity.legendary: return MasterManager.globalValues.legendary;
                case ItemRarity.unique: return MasterManager.globalValues.unique;
                default: return MasterManager.globalValues.common;
            }
        }
    }
}

