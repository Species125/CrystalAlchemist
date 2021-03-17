using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Shop/Crafting List")]
    public class CraftingList : ScriptableObject
    {
        [SerializeField]
        private List<CraftingRecipe> recipes = new List<CraftingRecipe>();
    }
}
