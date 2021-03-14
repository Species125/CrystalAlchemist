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

        [BoxGroup("Attributes")]
        [ShowIf("itemType", ItemType.ability)]
        public Ability ability;

        [BoxGroup("Attributes")]
        [ShowIf("itemType", ItemType.outfit)]
        public CharacterCreatorProperty outfit;

        [BoxGroup("Inventory")]
        [Tooltip("Info is need to load names, icons and discriptions")]
        [SerializeField]
        [Required]
        public ItemInfo info;

        [HideInInspector]
        public int amount = 1;

        [BoxGroup("Signals")]
        [SerializeField]
        private AudioClip collectSoundEffect;


        public ItemInfo getInfo()
        {
            //if (this.itemGroup != null) return this.itemGroup.info;
            return this.info;
        }

        public void SetStats(int value, InventoryType type, AudioClip soundEffect, ItemInfo info)
        {
            this.value = value;
            this.inventoryItem.inventoryType = type;
            this.collectSoundEffect = soundEffect;
            this.info = info;
        }

        public void Initialize(int amount)
        {
            this.amount = amount;
        }

        [AssetIcon]
        public Sprite getSprite()
        {
            if (this.info != null) return this.info.getSprite();
            return null;
        }

        public int GetTotalAmount()
        {
            return this.value * this.amount;
        }

        public AudioClip getSoundEffect()
        {
            return this.collectSoundEffect;
        }

        public string getName()
        {
            return this.info.getName();
        }

        public string getDescription()
        {
            return this.info.getDescription();
        }
    }
}
