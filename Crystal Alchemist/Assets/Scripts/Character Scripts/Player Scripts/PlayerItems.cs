



using UnityEngine;

namespace CrystalAlchemist
{
    public class PlayerItems : PlayerComponent
    {
        [SerializeField]
        private PlayerInventory inventory;

        private void Awake()
        {
            GameEvents.current.OnKeyItem += hasKeyItemAlready; //because keyItemCheck will be called very early
            GameEvents.current.OnItemAmount += GetAmount;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnKeyItem -= hasKeyItemAlready;
            GameEvents.current.OnItemAmount -= GetAmount;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.inventory.Initialize(); //remove null objects    
        }

        public int GetAmount(ItemGroup group)
        {
            return this.inventory.GetAmount(group);
        }

        public void CollectItem(ItemStats item) => this.inventory.collectItem(item);

        public void UpdateInventory(ItemGroup item, int amount) => this.inventory.UpdateInventory(item, amount);

        public bool hasKeyItemAlready(string name)
        {
            foreach (ItemStats elem in this.inventory.keyItems) if (elem != null && name == elem.name) return true;
            return false;
        }

        public PlayerInventory GetInventory()
        {
            return this.inventory;
        }

        public int GetAmount(Costs price)
        {
            if (price.resourceType == CostType.item && price.item != null) return this.GetAmount(price.item);
            else if (price.resourceType == CostType.keyItem && price.keyItem != null && GameEvents.current.HasKeyItem(price.keyItem.name)) return 1;
            return 0;
        }
    }
}
