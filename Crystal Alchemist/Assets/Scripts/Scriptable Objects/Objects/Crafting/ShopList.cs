using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Shop/Shop List")]
    public class ShopList : ScriptableObject
    {
        [System.Serializable]
        public class SellValues
        {
            public ItemRarity rarity;
            public int price;
        }

        public List<SellValues> sellValues = new List<SellValues>();

        public List<ShopCategory> categories = new List<ShopCategory>();
    }
}
