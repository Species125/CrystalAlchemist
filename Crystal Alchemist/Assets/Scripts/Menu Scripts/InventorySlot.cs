﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI amount;

    [SerializeField]
    private int ID = 0;

    //[SerializeField]
    //private ItemFeature feature;

    private Item item;

    private void Awake()
    {
        if(this.ID <= 0) this.ID = this.gameObject.transform.GetSiblingIndex() + 1;
    }

    /*
    public ItemFeature getFeature()
    {
        return this.feature;
    }*/

    public int getID()
    {
        return this.ID;
    }

    public Item getItem()
    {
        return this.item;
    }

    public void openSkillMenu(InventoryMenu menu)
    {
        if(this.item != null) menu.openSkillMenu();
    }

    public void openMap(InventoryMenu menu)
    {
        if (this.item != null) menu.openMap();
    }

    public void setItemToSlot(Item item)
    {
        if (item == null)
        {
            this.image.gameObject.SetActive(false);
            //this.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.image.gameObject.SetActive(true);
            //this.GetComponent<Button>().interactable = true;
            this.item = item;

            if(!item.isKeyItem && item.amount > 0) this.amount.text = "x" + item.amount;

            this.image.sprite = this.item.itemSpriteInventory;

            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
