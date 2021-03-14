using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Items/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [SerializeField]
        private ItemDrop drop;

        [SerializeField]
        private bool isUnique = false;

        [SerializeField]
        private List<Costs> materials = new List<Costs>();

        public void CraftIt(Player player)
        {
            foreach (Costs cost in this.materials) player.ReduceResource(cost);
            GameEvents.current.DoCollect(this.drop);
        }

        private bool CanCraftIt(Player player)
        {
            foreach (Costs cost in this.materials) 
            {
                if (!player.HasEnoughCurrency(cost)) return false;
            }

            return true;
        }
    }
}
