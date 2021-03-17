using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ShopWindowHandler : MenuBehaviour
    {
        [BoxGroup("Shop Windows")]
        [SerializeField]
        private ShopData data;

        [BoxGroup("Shop Windows")]
        [SerializeField]
        private List<ShopMenuWindow> menues = new List<ShopMenuWindow>();

        private void Awake()
        {
            foreach (ShopMenuWindow menu in menues)
            {
                menu.SetShopMenu(data.shopList);
                menu.gameObject.SetActive(menu.type == data.type );                
            }
        }
    }
}