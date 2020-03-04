﻿using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemUI;

    [SerializeField]
    private int ID = 0;

    private void Awake()
    {
        if(this.ID <= 0) this.ID = this.gameObject.transform.GetSiblingIndex() + 1;
    }

    public void openKeyItem(InventoryMenu menu)
    {
        if (this.itemUI.getItem() != null 
         && this.itemUI.getItem().keyItemSignal != null)
        {
            this.itemUI.getItem().keyItemSignal.Raise();
            menu.exitMenu();
        }
    }

    public int getID()
    {
        return this.ID;
    }
    
    public void setItemToSlot(Item item)
    {
        this.itemUI.setItem(item);
    }
}
