using UnityEngine;

namespace CrystalAlchemist
{
    public class PlayerItems : PlayerComponent
    {
        [SerializeField]
        private PlayerInventory inventory;

        private void Awake() => GameEvents.current.OnItemAmount += GetAmount;        

        private void OnDestroy() => GameEvents.current.OnItemAmount -= GetAmount;        

        public override void Initialize()
        {
            base.Initialize();
            this.inventory.RemoveNulls(); //remove null objects    
        }

        public int GetAmount(InventoryItem group)
        {
            return this.inventory.GetAmount(group);
        }

        public PlayerInventory GetInventory()
        {
            return this.inventory;
        }

        public int GetAmount(Costs price)
        {
            if (price.resourceType == CostType.item && price.item != null) return this.GetAmount(price.item);
            else if (price.resourceType == CostType.keyItem && price.keyItem != null && price.keyItem.HasItemAlready()) return 1;
            return 0;
        }
    }
}
