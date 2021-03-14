using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Items/Crafting List")]
    public class CraftingList : ScriptableObject
    {
        //CRAFTS:
        //list of items to craft

        [SerializeField]
        private List<CraftingRecipe> recipes = new List<CraftingRecipe>();
    }
}
