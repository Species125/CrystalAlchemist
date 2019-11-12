﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI amount;

    [SerializeField]
    private bool preferInventoryIcon = true;

    private Item item;

    public Item getItem()
    {
        return this.item;
    }

    public void setItem(Item item)
    {
        this.item = item;

        if (this.item == null)
        {
            this.image.gameObject.SetActive(false);
            //this.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.image.gameObject.SetActive(true);
            //this.GetComponent<Button>().interactable = true;            

            if (!item.isKeyItem && item.amount > 1) this.amount.text = "x" + item.amount;
            else this.amount.text = "";

            if(this.preferInventoryIcon) Utilities.Items.setItemImage(this.image, item);
            else this.image.sprite = item.itemSprite;

            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
