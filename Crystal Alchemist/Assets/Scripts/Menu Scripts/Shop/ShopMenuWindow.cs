using UnityEngine;

namespace CrystalAlchemist
{
    public class ShopMenuWindow : MonoBehaviour
    {
        [SerializeField]
        private ShopMenuNPC shopMenu;

        public ShopWindowType type;

        public void SetShopMenu(ShopList list) => shopMenu.SetMenu(list);
    }
}
