using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Player/Player Inventory")]
    public class PlayerInventory : ScriptableObject
    {
        public List<InventoryItem> artifacts = new List<InventoryItem>();

        public List<InventoryItem> treasures = new List<InventoryItem>();

        public List<InventoryItem> inventoryItems = new List<InventoryItem>();

        public List<InventoryItem> currencies = new List<InventoryItem>();

        public List<InventoryItem> craftingItems = new List<InventoryItem>();

        public List<InventoryItem> housingItems = new List<InventoryItem>();

        public void Clear()
        {
            this.treasures.Clear();
            this.artifacts.Clear();
            this.currencies.Clear();
            this.craftingItems.Clear();
            this.housingItems.Clear();
            this.inventoryItems.Clear();
            Initialize();
        }

        public void Initialize()
        {
            this.treasures.RemoveAll(item => item == null);
            this.artifacts.RemoveAll(item => item == null);
            this.inventoryItems.RemoveAll(item => item == null);
            this.craftingItems.RemoveAll(item => item == null);
            this.currencies.RemoveAll(item => item == null);
            this.housingItems.RemoveAll(item => item == null);
        }

        public bool KeyItemExists(InventoryItem item)
        {
            if (item.inventoryType != InventoryType.artifacts 
             && item.inventoryType != InventoryType.treasure) return false;

            List<InventoryItem> inventory = GetInventory(item.inventoryType);
            if (inventory == null) return false;

            foreach (InventoryItem elem in inventory)
                if (elem != null && item.name == elem.name) return true;

            return false;
        }

        [Button]
        public void CollectItem(ItemStats item)
        {
            List<InventoryItem> inventory = GetInventory(item.inventoryItem.inventoryType);
            if (inventory == null) return;

            //add Inventory Item or change its amount
            CollectItem(item.inventoryItem, item.GetTotalAmount(), inventory);
            GameEvents.current.DoSaveGame();

            if (item.inventoryItem != null) item.inventoryItem.RaiseCollectSignal();
        }

        public void CollectItem(InventoryItem group, int amount, List<InventoryItem> inventory)
        {
            if (inventory == null) return;

            InventoryItem found = GameUtil.GetInventoryItem(group, inventory);
            if (found == null) AddItemGroup(group, amount, inventory); //add new Itemgroup
            else found.UpdateAmount(amount); //set amount of itemgroup
        }

        private void AddItemGroup(InventoryItem group, int amount, List<InventoryItem> inventory)
        {
            if (inventory == null) return;

            InventoryItem newGroup = Instantiate(group);
            newGroup.name = group.name;
            newGroup.UpdateAmount(amount);
            inventory.Add(newGroup);
        }

        public void UpdateInventory(InventoryItem itemGroup, int value)
        {
            foreach (InventoryItem group in this.inventoryItems)
            {
                if (group != null && group.name == itemGroup.name)
                {
                    group.UpdateAmount(value);
                    break;
                }
            }

            itemGroup.RaiseCollectSignal();
        }

        public List<InventoryItem> GetInventory(InventoryType type)
        {
            switch (type)
            {
                case InventoryType.item: return this.inventoryItems;
                case InventoryType.crafting: return this.craftingItems;
                case InventoryType.housing: return this.housingItems;
                case InventoryType.currency: return this.currencies;
                case InventoryType.treasure: return this.treasures;
                case InventoryType.artifacts: return this.artifacts;
            }

            return null;
        }

        public int GetAmount(InventoryItem itemGroup)
        {
            int amount = 0;

            InventoryItem found = GameUtil.GetInventoryItem(itemGroup, GetInventory(itemGroup.inventoryType));
            if (found != null) amount = found.GetAmount();            

            return amount;
        }

        public string GetAmountString(InventoryItem itemGroup)
        {
            InventoryItem found = GameUtil.GetInventoryItem(itemGroup, GetInventory(itemGroup.inventoryType));
            if (found != null) return found.GetAmountString();
            else return FormatUtil.formatString(0, itemGroup.maxAmount);
        }

        public List<string[]> GetItemsList(InventoryType type)
        {
            List<string[]> result = new List<string[]>();

            List<InventoryItem> inventory = GetInventory(type);
            if (inventory == null) return result; 

            foreach (InventoryItem item in inventory)
            {
                string[] temp = new string[2];
                temp[0] = item.name;
                temp[1] = item.GetAmount() + "";
                result.Add(temp);
            }
            return result;
        }
    }
}
