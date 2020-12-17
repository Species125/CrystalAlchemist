using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    [SerializeField]
    public List<ItemStats> keyItems = new List<ItemStats>();

    [SerializeField]
    public List<ItemGroup> inventoryItems = new List<ItemGroup>();    

    public void Clear()
    {
        this.keyItems.Clear();
        this.inventoryItems.Clear();
        Initialize();
    }

    public void Initialize()
    {
        this.keyItems.RemoveAll(item => item == null);
        this.inventoryItems.RemoveAll(item => item == null);
    }

    private void AddItemGroup(ItemGroup group, int amount)
    {
        ItemGroup newGroup = Instantiate(group);
        newGroup.name = group.name;
        newGroup.UpdateAmount(amount);
        this.inventoryItems.Add(newGroup);
    }
    
    public void collectItem(ItemGroup group, int amount)
    {
        ItemGroup found = getItemGroup(group);
        if (found == null) AddItemGroup(group, amount); //add new Itemgroup
        else found.UpdateAmount(amount); //set amount of itemgroup
    }

    [Button]
    public void collectItem(ItemStats item)
    {
        if (item.isKeyItem() && !GameEvents.current.HasKeyItem(item.name))
        {           
            //add Key Item
            ItemStats keyItem = Instantiate(item);
            keyItem.name = item.name;
            this.keyItems.Add(keyItem);
        }
        else if (!item.isKeyItem())
        {
            //add Inventory Item or change its amount
            collectItem(item.itemGroup, item.getTotalAmount());      
        }

        if(item.itemGroup != null) item.itemGroup.raiseCollectSignal();
    }

    public void UpdateInventory(ItemGroup itemGroup, int value)
    {
        foreach (ItemGroup group in this.inventoryItems)
        {
            if (group != null && group.name == itemGroup.name)
            {
                group.UpdateAmount(value);
                break;
            }
        }

        itemGroup.raiseCollectSignal();
    }       

    public ItemGroup getItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup != null)
        {
            foreach (ItemGroup group in this.inventoryItems)
            {
                if (group != null && group.name == itemGroup.name) return group;
            }
        }
        return null;
    }

    public ItemGroup GetInventoryItem(int ID)
    {
        foreach (ItemGroup item in this.inventoryItems)
        {
            if (item != null && item.isID(ID)) return item;
        }

        return null;
    }

    public ItemStats GetKeyItem(int ID)
    {
        foreach (ItemStats item in keyItems)
        {
            if (item != null && item.isID(ID)) return item;
        }

        return null;
    }

    public int GetAmount(ItemGroup itemGroup)
    {
        int amount = 0;

        ItemGroup found = this.getItemGroup(itemGroup);
        if (found != null) amount = found.GetAmount();
        else
        {
            foreach(ItemStats stats in this.keyItems)
            {
                if (stats != null && stats.itemGroup == itemGroup) amount++;
            }
        }

        return amount;
    }

    public string GetAmountString(ItemGroup itemGroup)
    {        
        ItemGroup found = this.getItemGroup(itemGroup);
        if (found != null) return found.GetAmountString();
        else return FormatUtil.formatString(0, itemGroup.maxAmount);
    }
}
