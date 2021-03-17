using System.Collections.Generic;
using UnityEngine;
using AssetIcons;

namespace CrystalAlchemist {

    [CreateAssetMenu(menuName = "Game/Shop/Shop Category")]
    public class ShopCategory : ScriptableObject
    {
        [AssetIcon]
        public Sprite icon;

        public List<CraftingRecipe> shopItems = new List<CraftingRecipe>();
    }
}
