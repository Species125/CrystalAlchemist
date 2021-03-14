using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Items/Shop List")]
    public class ShopList : ScriptableObject
    {
        public enum AvailableDays
        {
            all,
            montag,
            dienstag,
            mittwoch,
            donnerstag,
            freitag,
            samstag,
            sonntag
        }

        [System.Serializable]
        public class SellValues
        {
            ItemRarity rarity;
            int price;
        }

        [System.Serializable]
        public class Offers
        {
            public List<AvailableDays> days = new List<AvailableDays>();               
            public List<CraftingRecipe> shopItems = new List<CraftingRecipe>();
        }

        [SerializeField]
        private List<SellValues> sellValues = new List<SellValues>();

        [SerializeField]
        private List<CraftingRecipe> shopItems = new List<CraftingRecipe>();

        [SerializeField]
        private List<Offers> special = new List<Offers>();
    }
}
