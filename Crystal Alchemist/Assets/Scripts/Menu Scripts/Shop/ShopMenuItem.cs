using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public enum ShopType
    {
        buy,
        sell
    }

    public class ShopMenuItem : MonoBehaviour
    {
        [SerializeField]
        private InventorySlot slot;

        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private PlayerInventory inventory;

        [SerializeField]
        private Selectable button;

        [SerializeField]
        private ItemDrop currency;

        [SerializeField]
        private GameObject selection;

        private int amount = 1;
        private CraftingRecipe recipe;
        private ShopType type;
        private InventoryItem item;
        private ShopMenuCategory category;

        public void SetMenuItem(CraftingRecipe recipe, ShopMenuCategory category)
        {
            this.type = ShopType.buy;
            this.category = category;
            this.recipe = recipe;
            this.slot.SetItem(recipe.GetDrop().stats);

            UpdateAmount(this.amount);
        }

        public void SetMenuItem(InventoryItem item, ShopMenuCategory category)
        {
            this.type = ShopType.sell;
            this.category = category;
            this.item = item;
            this.slot.SetItem(item);

            UpdateAmount(this.amount);
        }

        public void BuyIt()
        {
            if (this.type == ShopType.sell)
            {
                int value = this.item.shopValue;
                inventory.UpdateInventory(this.item, -amount);
                GameEvents.current.DoCollect(this.currency, amount*value);

                SetMenuItem(this.item, this.category);
            }
            else
            {   //Buy from NPC
                this.recipe.CraftIt(this.inventory, this.amount);
                UpdateAmount(this.amount);
            }
        }

        private void UpdateAmount(int value)
        {
            this.amount = value;

            if (this.type == ShopType.buy)
            {
                int playerAmount = this.inventory.GetAmount(this.recipe.GetDrop().stats.inventoryItem);
                int maxAmount = this.recipe.GetDrop().stats.inventoryItem.maxAmount;

                if (this.amount + playerAmount > maxAmount) this.amount = maxAmount - playerAmount;
                if (this.amount < 1) this.amount = 1;

                this.button.interactable = this.recipe.CanCraftIt(this.inventory, this.amount);
            }
            else
            {
                int playerAmount = this.inventory.GetAmount(this.item);

                if (this.amount > playerAmount) this.amount = playerAmount;
                if (this.amount < 1) this.amount = 1;

                //this.button.interactable = (this.inventory.GetAmount(this.item) > 0);
                if (this.inventory.GetAmount(this.item) <= 0) Destroy(this.gameObject);
            }

            this.inputField.text = this.amount + "";
            this.category.UpdatePrice(this.type, this.amount, this.recipe, this.currency.stats.inventoryItem, this.item);
        }

        public void OnSelected()
        {
            UpdateSelection();
            this.category.UpdatePrice(this.type, this.amount, this.recipe, this.currency.stats.inventoryItem, this.item);
        }

        private void UpdateSelection()
        {
            this.category.ClearSelection();
            ShowSelection(true);
        }

        public void ShowSelection(bool value) => this.selection?.SetActive(value);

        public void ChangeAmount(int value)
        {
            UpdateAmount(this.amount+value);            
        }

        public void OnValueChanged()
        {
            string value = this.inputField.text;
            bool isNumeric = int.TryParse(value, out int n);

            if (isNumeric) UpdateAmount(n);            
        }
    }
}
