using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist {
    public class ShopMenuPriceList : MonoBehaviour
    {
        [SerializeField]
        private ShopMenuPrice template;

        [SerializeField]
        private Transform content;

        [SerializeField]
        private PlayerInventory inventory;

        private List<ShopMenuPrice> priceList = new List<ShopMenuPrice>();

        private void Clear()
        {
            foreach (ShopMenuPrice price in this.priceList)
            {
                Destroy(price.gameObject);
            }

            this.priceList.Clear();
        }

        public void UpdatePrice(CraftingRecipe recipe, int amount)
        {
            Clear();

            foreach(Costs cost in recipe.GetMaterials())
            {
                ShopMenuPrice newPrice = Instantiate(this.template, this.content);
                newPrice.SetPrice(cost.item, (int)cost.amount, inventory, amount, true);
                newPrice.gameObject.SetActive(true);
                this.priceList.Add(newPrice);
            }
        }

        public void UpdatePrice()
        {
            Clear();
        }

        public void UpdatePrice(InventoryItem item, int value, int amount)
        {
            Clear();

            ShopMenuPrice newPrice = Instantiate(this.template, this.content);
            newPrice.SetPrice(item, value, inventory, amount, false);
            newPrice.gameObject.SetActive(true);
            this.priceList.Add(newPrice);
        }
    }
}
