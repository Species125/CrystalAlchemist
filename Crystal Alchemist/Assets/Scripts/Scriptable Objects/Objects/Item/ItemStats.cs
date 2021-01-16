using AssetIcons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum ItemType
    {
        consumable,
        item,
        keyItem,
        outfit,
        ability
    }

    [CreateAssetMenu(menuName = "Game/Items/Item Stats")]
    public class ItemStats : ScriptableObject
    {
        [BoxGroup("Attributes")]
        public ItemType itemType;

        [BoxGroup("Attributes")]
        [ShowIf("itemType", ItemType.consumable)]
        public List<CharacterResource> resources = new List<CharacterResource>();

        [BoxGroup("Attributes")]
        [ShowIf("itemType", ItemType.item)]
        [SerializeField]
        private int value = 1;

        [BoxGroup("Inventory")]
        [SerializeField]
        [ShowIf("itemType", ItemType.item)]
        [Tooltip("Needed if an item have to be grouped. Normal items only!")]
        [Required]
        public ItemGroup itemGroup;

        [BoxGroup("Inventory")]
        [SerializeField]
        [Required]
        [ShowIf("itemType", ItemType.keyItem)]
        [Tooltip("Needed to show the item in the inventory. Only for key items!")]
        public ItemSlotInfo inventoryInfo;

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

        public void SetStats(int value, ItemType type, AudioClip soundEffect, ItemInfo info)
        {
            this.value = value;
            this.itemType = type;
            this.collectSoundEffect = soundEffect;
            this.info = info;
        }

        public bool isKeyItem()
        {
            return this.itemType == ItemType.keyItem;
        }

        public bool isID(int ID)
        {
            if (this.itemGroup != null) return this.itemGroup.isID(ID);
            else if (this.inventoryInfo != null) return this.inventoryInfo.isID(ID);
            return false;
        }

        public void Initialize(int amount)
        {
            this.amount = amount;
        }

        public string getItemGroup()
        {
            if (this.itemGroup != null) return this.itemGroup.getName();
            else return "";
        }

        public int getMaxAmount()
        {
            if (this.itemGroup != null) return this.itemGroup.maxAmount;
            return 0;
        }

        [AssetIcon]
        public Sprite getSprite()
        {
            if (this.info != null) return this.info.getSprite();
            return null;
        }

        public int getTotalAmount()
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
