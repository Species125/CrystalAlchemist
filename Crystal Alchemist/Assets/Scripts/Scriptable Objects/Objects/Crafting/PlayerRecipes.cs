using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist {

    [CreateAssetMenu(menuName = "Game/Player/Player Recipes")]
    public class PlayerRecipes : ScriptableObject
    {
        private List<CraftingRecipe> recipes = new List<CraftingRecipe>();

        public void AddRecipe(CraftingRecipe recipe)
        {
            this.recipes.Add(recipe);
        }
    }
}
