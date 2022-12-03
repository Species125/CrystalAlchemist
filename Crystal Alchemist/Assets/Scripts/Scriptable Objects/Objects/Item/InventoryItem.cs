using AssetIcons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum InventoryType
    {
        item,
        artifacts,
        currency,
        housing,
        crafting,
        treasure
    }

    [CreateAssetMenu(menuName = "Game/Items/Inventory Item")]
    public class InventoryItem : NetworkScriptableObject
    {
        [BoxGroup("Attributes")]
        public InventoryType inventoryType;

        [AssetIcon]
        [Required]
        [BoxGroup("Attributes")]
        public Sprite icon;

        [BoxGroup("Inventory")]
        [OnValueChanged("MaxValueChanged")]
        [Min(1)]
        public int maxAmount;

        [BoxGroup("Inventory")]
        [Tooltip("True, if the value can be changed by the player or shop")]
        public bool canConsume = true;

        [BoxGroup("Inventory")]
        [SerializeField]
        private bool updateCurrencyUI;

        [BoxGroup("Inventory")]
        public AudioClip raiseSoundEffect;

        [BoxGroup("Inventory")]
        public ItemRarity rarity = ItemRarity.common;

        [ShowIf("canConsume")]
        [BoxGroup("Shop Price")]
        [Min(1)]
        public int shopValue = 1;

        [BoxGroup("Shop Price")]
        public ShopPriceUI shopPrice;

        [BoxGroup("Debug")]
        [SerializeField]
        private int amount;

        [BoxGroup("Signals")]
        [ShowIf("inventoryType", InventoryType.artifacts)]
        [SerializeField]
        private SimpleSignal keyItemSignal;

        private void MaxValueChanged() 
        {
            if (this.inventoryType == InventoryType.artifacts || this.inventoryType == InventoryType.treasure)
                this.maxAmount = 1;
        }


        public void SetGroup(int maxValue, bool canConsume, bool updateUI,
                             AudioClip soundEffect, ShopPriceUI shop, int shopValue,
                             ItemRarity rarity, InventoryType type, Sprite icon)
        {
            this.maxAmount = maxValue;
            this.canConsume = canConsume;
            this.updateCurrencyUI = updateUI;
            this.raiseSoundEffect = soundEffect;
            this.shopPrice = shop;
            this.shopValue = shopValue;
            this.rarity = rarity;
            this.inventoryType = type;
            this.icon = icon;
        }

        public string GetDescription()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Description", LocalisationFileType.items);
        }

        public string GetName()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Name", LocalisationFileType.items);
        }

        public int GetAmount()
        {
            return this.amount;
        }

        public string GetAmountString()
        {
            return FormatUtil.formatString(this.amount, this.maxAmount);
        }

        [Button]
        public void UpdateAmount(int amount)
        {
            if (!canConsume && amount < 0) return; //dont decrease amount if not consumeable

            this.amount += amount;
            if (this.amount > this.maxAmount) this.amount = this.maxAmount;
            if (this.amount < 0)
            {
                this.amount = 0;
                Destroy(this);
            }
        }

        public void RaiseCollectSignal()
        {
            if (this.updateCurrencyUI)
            {
                GameEvents.current.DoCurrencyChange(true);
                AudioUtil.playSoundEffect(raiseSoundEffect);
            }
        }

        public void RaiseMenuSignal()
        {
            if (this.keyItemSignal != null && this.inventoryType == InventoryType.artifacts) this.keyItemSignal.Raise();
        }
    }
}
