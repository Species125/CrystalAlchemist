using AssetIcons;


using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Items/Item Group")]
    public class ItemGroup : NetworkScriptableObject
    {
        [BoxGroup("Inventory")]
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
        [Tooltip("Needed to show the item in the inventory")]
        [SerializeField]
        public ItemSlotInfo inventoryInfo;

        [BoxGroup("Inventory")]
        [SerializeField]
        private bool updateCurrencyUI;

        [BoxGroup("Inventory")]
        public AudioClip raiseSoundEffect;

        [BoxGroup("Shop Price")]
        public ShopPriceUI shopPrice;

        [BoxGroup("Debug")]
        [SerializeField]
        private int amount;

        [AssetIcon]
        public Sprite GetSprite()
        {
            if (this.info != null) return this.info.getSprite();
            return null;
        }

        public void SetGroup(int maxValue, bool canConsume, bool updateUI, AudioClip soundEffect, ShopPriceUI shop)
        {
            this.maxAmount = maxValue;
            this.canConsume = canConsume;
            this.updateCurrencyUI = updateUI;
            this.raiseSoundEffect = soundEffect;
            this.shopPrice = shop;
        }

        public bool isID(int ID)
        {
            if (this.inventoryInfo != null) return this.inventoryInfo.isID(ID);
            return false;
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
        }

        public void raiseCollectSignal()
        {
            if (this.updateCurrencyUI)
            {
                GameEvents.current.DoCurrencyChange(true);
                AudioUtil.playSoundEffect(raiseSoundEffect);
            }
        }
    }
}
