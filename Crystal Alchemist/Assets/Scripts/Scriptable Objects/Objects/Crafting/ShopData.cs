using UnityEngine;

namespace CrystalAlchemist
{
    public enum ShopWindowType
    {
        shop,
        crafting
    }

    [CreateAssetMenu(menuName = "Game/Shop/Shop Data")]
    public class ShopData : ScriptableObject
    {
        [HideInInspector]
        public ShopWindowType type;

        [HideInInspector]
        public ShopList shopList;
        
        public void SetData(ShopWindowType type, ShopList list)
        {
            this.type = type;
            this.shopList = list;
        }
    }
}
