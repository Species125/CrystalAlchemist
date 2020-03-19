﻿using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class InventoryMenu : MenuControls
{
    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI regularItemsLabel;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI keyItemsLabel;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject regularItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject currencyItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject keyItems;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject quickmenu;

    public override void Start()
    {
        base.Start();
        showCategory(0);
        loadInventory();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        loadInventory();
    }

    private void loadInventory()
    {      
        setItemsToSlots(this.regularItems, false);
        setItemsToSlots(this.keyItems, true);
        setItemsToSlots(this.quickmenu, true);
    }

    public void switchCategory()
    {
        if (this.regularItems.activeInHierarchy) showCategory(1);
        else showCategory(0);
    }

    public void showCategory(int category)
    {
        this.regularItems.SetActive(false);
        this.regularItemsLabel.gameObject.SetActive(false);
        this.currencyItems.SetActive(false);
        this.keyItems.SetActive(false);
        this.keyItemsLabel.gameObject.SetActive(false);

        switch (category)
        {
            case 1: this.keyItems.SetActive(true); this.keyItemsLabel.gameObject.SetActive(true); this.currencyItems.SetActive(true); break;
            default: this.regularItems.SetActive(true); this.regularItemsLabel.gameObject.SetActive(true); break;
        }
    }

    private void setItemsToSlots(GameObject categoryGameobject, bool showKeyItems)
    {
        if (this.player != null)
        {
            for (int i = 0; i < categoryGameobject.transform.childCount; i++)
            {
                GameObject slot = categoryGameobject.transform.GetChild(i).gameObject;
                InventorySlot iSlot = slot.GetComponent<InventorySlot>();

                int ID = iSlot.getID();
                ItemGroup item = null;
                if(showKeyItems) item = this.player.GetComponent<PlayerUtils>().getKeyItems(ID);
                else item = this.player.GetComponent<PlayerUtils>().getInventoryItems(ID);

                slot.GetComponent<InventorySlot>().setItemToSlot(item);
            }
        }
    }
}
