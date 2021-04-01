using AssetIcons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum ItemType
    {
        consumable,
        inventory,       
        outfit,
        ability
    }

    public enum ItemRarity
    {
        common,
        uncommon,
        rare,
        epic,
        legendary,
        unique
    }

    [CreateAssetMenu(menuName = "Game/Items/Item Stats")]
    public class ItemStats : ScriptableObject
    {
        [BoxGroup("Attributes")]
        public ItemType itemType;

        [BoxGroup("Attributes")]
        public ItemRarity rarity = ItemRarity.common;
                
        [BoxGroup("Attributes")]
        [HideIf("itemType", ItemType.ability)]
        [HideIf("itemType", ItemType.outfit)]
        public Sprite icon;

        [BoxGroup("Attributes")]
        [ShowIf("itemType", ItemType.consumable)]
        public List<CharacterResource> resources = new List<CharacterResource>();

        [BoxGroup("Attributes")]
        [SerializeField]
        [ShowIf("itemType", ItemType.inventory)]
        private int value = 1;

        [BoxGroup("Inventory")]
        [SerializeField]
        [ShowIf("itemType", ItemType.inventory)]
        [Required]
        public InventoryItem inventoryItem;

        [BoxGroup("Inventory")]
        [ShowIf("itemType", ItemType.ability)]
        public Ability ability;

        [BoxGroup("Inventory")]
        [ShowIf("itemType", ItemType.outfit)]
        public CharacterCreatorProperty outfit;

        [HideInInspector]
        public int amount = 1;


        [AssetIcon]
        public Sprite GetSprite()
        {
            if (this.itemType == ItemType.ability) return this.ability.GetSprite();
            else if (this.itemType == ItemType.outfit) return this.ability.GetSprite();
            else return this.icon;
        }

        public void SetStats(int value, ItemType type, ItemRarity rarity, Sprite icon = null,
                             InventoryItem inventoryItem = null, Ability ability = null, CharacterCreatorProperty outfit = null)
        {
            this.value = value;
            this.itemType = type;
            this.rarity = rarity;
            this.icon = icon;

            this.inventoryItem = null;
            this.outfit = null;
            this.ability = null;

            if (this.itemType == ItemType.inventory)
            {
                this.inventoryItem = inventoryItem;
                if (this.icon == null) this.icon = this.inventoryItem.icon;
            }
            else if (this.itemType == ItemType.outfit)
            {                
                this.outfit = outfit;
                this.icon = this.outfit.GetSprite();
            }
            else if (this.itemType == ItemType.ability)
            {
                this.ability = ability;
                this.icon = this.ability.GetSprite();
            }
        }

        public void Initialize(int amount)
        {
            this.amount = amount;
        }

        public int GetTotalAmount()
        {
            return this.value * this.amount;
        }

        public string GetDescription()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Description", GetLocalisationType());
        }

        public string GetName()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Name", GetLocalisationType());
        }

        public LocalisationFileType GetLocalisationType()
        {
            if (this.itemType == ItemType.ability) return LocalisationFileType.skills;
            return LocalisationFileType.items;
        }
    }
}
