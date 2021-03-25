using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class ShopMenuCategory : MonoBehaviour
    {
        [SerializeField]
        private ShopMenuPriceList priceList;

        [SerializeField]
        private ShopMenuItem template;

        [SerializeField]
        private Transform content;

        private List<ShopMenuItem> shopMenuItems = new List<ShopMenuItem>();

        private void OnEnable()
        {
            if (this.shopMenuItems.Count > 0) this.shopMenuItems[0].OnSelected();
            else ClearSelection();
        }

        public void SetCategory(List<CraftingRecipe> recipes)
        {
            this.shopMenuItems.Clear();

            foreach (CraftingRecipe recipe in recipes) 
            {
                ShopMenuItem newItem = Instantiate(this.template, content);
                newItem.SetMenuItem(recipe, this);
                newItem.gameObject.SetActive(true);
                shopMenuItems.Add(newItem);
            }
        }

        public void SetCategory(List<InventoryItem> items)
        {
            this.shopMenuItems.Clear();

            foreach (InventoryItem item in items)
            {
                if (!item.canConsume) continue;

                ShopMenuItem newItem = Instantiate(this.template, content);
                newItem.SetMenuItem(item, this);
                newItem.gameObject.SetActive(true);
                shopMenuItems.Add(newItem);
            }
        }

        public void ClearSelection()
        {
            this.shopMenuItems.RemoveAll(x => x == null);
            this.priceList.UpdatePrice();

            foreach (ShopMenuItem item in this.shopMenuItems) item.ShowSelection(false);            
        }

        public void UpdatePrice(ShopType type, int amount, CraftingRecipe recipe, 
                                                           InventoryItem currency, InventoryItem playerItem)
        {
            if (type == ShopType.sell && currency != null && playerItem != null)
                this.priceList.UpdatePrice(currency, playerItem.shopValue, amount);
            else if (type == ShopType.buy && recipe != null)
                this.priceList.UpdatePrice(recipe, amount);
            else
                this.priceList.UpdatePrice();
        }
    }
}
