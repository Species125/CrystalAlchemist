using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ShopMenu : MonoBehaviour
    {
        public ShopMenuTab tabTemplate;

        public Transform tabContent;

        public ShopMenuCategory categoryTemplate;

        public Transform categoryContent;

        [HideInInspector]
        public List<ShopMenuTab> tabs = new List<ShopMenuTab>();

        [HideInInspector]
        public List<ShopMenuCategory> categories = new List<ShopMenuCategory>();

        public void Refresh()
        {
            foreach (ShopMenuCategory category in this.categories) Destroy(category.gameObject);
            foreach (ShopMenuTab tab in this.tabs) Destroy(tab.gameObject);

            categories.Clear();
            tabs.Clear();
        }

        public void CloseTabs()
        {
            foreach(ShopMenuTab tab in this.tabs)
            {
                tab.SetActiveTab(false);
            }
        }
    }
}
