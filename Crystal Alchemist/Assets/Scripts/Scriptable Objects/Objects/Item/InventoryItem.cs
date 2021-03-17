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

        [BoxGroup("Inventory")]
        [OnValueChanged("MaxValueChanged")]
        [Min(1)]
        public int maxAmount;

        [BoxGroup("Inventory")]
        [Tooltip("True, if the value can be changed by the player or shop")]
        public bool canConsume = true;

        [BoxGroup("Inventory")]
        [Tooltip("Info is need to load names, icons and discriptions")]
        [SerializeField]
        [Required]
        public ItemInfo info;

        [BoxGroup("Inventory")]
        [SerializeField]
        private bool updateCurrencyUI;

        [BoxGroup("Inventory")]
        public AudioClip raiseSoundEffect;

        [BoxGroup("Inventory")]
        public ItemRarity rarity = ItemRarity.common;

        [BoxGroup("Shop Price")]
        public bool canBeSold = true;

        [ShowIf("canBeSold")]
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

        [AssetIcon]
        public Sprite GetSprite()
        {
            if (this.info != null) return this.info.getSprite();
            return null;
        }

        private void MaxValueChanged() 
        {
            if (this.inventoryType == InventoryType.artifacts || this.inventoryType == InventoryType.treasure)
                this.maxAmount = 1;
        }


        public void SetGroup(int maxValue, bool canConsume, bool updateUI, AudioClip soundEffect, ShopPriceUI shop)
        {
            this.maxAmount = maxValue;
            this.canConsume = canConsume;
            this.updateCurrencyUI = updateUI;
            this.raiseSoundEffect = soundEffect;
            this.shopPrice = shop;
        }

        public string getName()
        {
            return this.info.getName();
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
