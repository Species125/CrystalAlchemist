using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class InventoryCategory
    {
        public InventoryType inventoryType;        
        public Sprite icon;
        public List<InventoryItem> inventory = new List<InventoryItem>();

        public void Clear() => this.inventory.Clear();        

        public void RemoveNulls() => inventory.RemoveAll(x => x == null);

        public bool HaveSoldableItems()
        {
            foreach(InventoryItem item in this.inventory)
            {
                if (item.canConsume) return true;
            }
            return false;
        }
    }

    [CreateAssetMenu(menuName = "Game/Player/Player Inventory")]
    public class PlayerInventory : ScriptableObject
    {
        public List<InventoryCategory> categories = new List<InventoryCategory>();

        public void Clear()
        {
            foreach (InventoryCategory category in this.categories) category.Clear();
            RemoveNulls();
        }

        public void RemoveNulls()
        {
            foreach (InventoryCategory category in this.categories) category.RemoveNulls();
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
            if (found == null) AddInventoryItem(group, amount, inventory); //add new InventoryItem
            else found.UpdateAmount(amount); //set amount of InventoryItem

            RemoveNulls();
        }

        private void AddInventoryItem(InventoryItem group, int amount, List<InventoryItem> inventory)
        {
            if (inventory == null) return;

            InventoryItem newGroup = Instantiate(group);
            newGroup.name = group.name;
            newGroup.UpdateAmount(amount);
            inventory.Add(newGroup);

            RemoveNulls();
        }

        public void UpdateInventory(InventoryItem InventoryItem, int value)
        {
            foreach (InventoryItem group in GetInventory(InventoryItem.inventoryType))
            {
                if (group != null && group.name == InventoryItem.name)
                {
                    group.UpdateAmount(value);
                    break;
                }
            }

            RemoveNulls();

            InventoryItem.RaiseCollectSignal();
        }

        public List<InventoryItem> GetInventory(InventoryType type)
        {
            foreach(InventoryCategory category in this.categories)
            {
                if (category.inventoryType == type) return category.inventory;
            }

            return null;
        }

        public int GetAmount(InventoryItem inventoryItem)
        {
            return GetAmount(inventoryItem, out int n);
        }

        public int GetAmount(InventoryItem inventoryItem, out int maxAmount)
        {
            int amount = 0;
            maxAmount = 1;

            if (inventoryItem != null)
            {
                InventoryItem found = GameUtil.GetInventoryItem(inventoryItem, GetInventory(inventoryItem.inventoryType));
                if (found != null)
                {
                    amount = found.GetAmount();
                    maxAmount = found.maxAmount;
                }
            }

            return amount;
        }

        public string GetAmountString(InventoryItem InventoryItem)
        {
            InventoryItem found = GameUtil.GetInventoryItem(InventoryItem, GetInventory(InventoryItem.inventoryType));
            if (found != null) return found.GetAmountString();
            else return FormatUtil.formatString(0, InventoryItem.maxAmount);
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
