using UnityEngine;

namespace CrystalAlchemist
{
    public class ShopMenuPlayer : ShopMenu
    {
        [SerializeField]
        private PlayerInventory inventory;

        private void OnEnable()
        {
            Refresh();

            foreach (InventoryCategory category in this.inventory.categories)
            {
                if (!category.HaveSoldableItems()) continue;

                ShopMenuCategory newCategory = Instantiate(this.categoryTemplate, this.categoryContent);
                newCategory.SetCategory(category.inventory);
                this.categories.Add(newCategory);

                ShopMenuTab newTab = Instantiate(this.tabTemplate, this.tabContent);
                newTab.SetTab(category.icon, newCategory.gameObject, this);
                newTab.gameObject.SetActive(true);
                this.tabs.Add(newTab);                
            }

            if(this.tabs.Count > 0) this.tabs[0].SwitchTab();
        }
    }
}
