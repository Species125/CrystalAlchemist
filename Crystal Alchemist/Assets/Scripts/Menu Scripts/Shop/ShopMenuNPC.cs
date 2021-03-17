using UnityEngine;

namespace CrystalAlchemist {
    public class ShopMenuNPC : ShopMenu
    {
        private ShopList shopList;

        public void SetMenu(ShopList shopList) => this.shopList = shopList;

        private void OnEnable()
        {
            Refresh();

            foreach (ShopCategory category in this.shopList.categories)
            {
                ShopMenuCategory newCategory = Instantiate(this.categoryTemplate, this.categoryContent);
                newCategory.SetCategory(category.shopItems);
                this.categories.Add(newCategory);

                ShopMenuTab newTab = Instantiate(this.tabTemplate, this.tabContent);
                newTab.SetTab(category.icon, newCategory.gameObject, this);
                newTab.gameObject.SetActive(true);
                this.tabs.Add(newTab);
            }

            this.tabs[0].SwitchTab();
        }
    }
}
