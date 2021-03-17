using System.Collections.Generic;
using UnityEngine;
using AssetIcons;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Shop/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [SerializeField]
        private ItemDrop drop;

        [SerializeField]
        private bool isUnique = false;

        [SerializeField]
        private List<Costs> materials = new List<Costs>();

        [AssetIcon]
        public Sprite GetSprite() => this.drop?.GetSprite();        

        public void CraftIt(PlayerInventory inventory, int amount)
        {
            foreach (Costs cost in this.materials) inventory.UpdateInventory(cost.item, -(int)(cost.amount*amount));
            GameEvents.current.DoCollect(this.drop, amount);
        }

        public bool CanCraftIt(PlayerInventory inventory, int amount)
        {
            foreach (Costs cost in this.materials) 
            {
                if (cost.resourceType != CostType.item || cost.item == null) continue;
                if (inventory.GetAmount(cost.item) < (cost.amount * amount)) return false;
            }

            return true;
        }

        public ItemDrop GetDrop()
        {
            return this.drop;
        }

        public List<Costs> GetMaterials()
        {
            return this.materials;
        }
    }
}
