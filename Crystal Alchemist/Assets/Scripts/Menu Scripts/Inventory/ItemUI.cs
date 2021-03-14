﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI amount;

        public bool preferInventoryIcon = true;

        private InventoryItem itemGroup;
        private ItemStats itemStat;

        public InventoryItem GetInventoryItem()
        {
            return this.itemGroup;
        }

        public ItemStats GetItemStat()
        {
            return this.itemStat;
        }

        public virtual void SetItem(InventoryItem item)
        {
            this.itemGroup = item;

            if (item == null)
            {
                this.image.gameObject.SetActive(false);
            }
            else
            {
                this.image.gameObject.SetActive(true);

                if (item.canConsume && item.GetAmount() > 1) this.amount.text = "x" + item.GetAmount();
                else if (this.amount != null) this.amount.text = "";

                if (this.preferInventoryIcon) this.image.sprite = item.info.getSprite();
                else this.image.sprite = item.info.getSprite();

                this.image.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        public virtual void SetItem(ItemStats item)
        {
            this.itemStat = item;

            if (item == null)
            {
                this.image.gameObject.SetActive(false);
            }
            else
            {
                this.image.gameObject.SetActive(true);

                if (item.inventoryItem.inventoryType == InventoryType.item && item.amount > 1) this.amount.text = "x" + item.amount;
                else if (this.amount != null) this.amount.text = "";

                if (this.preferInventoryIcon) this.image.sprite = item.getSprite();
                else this.image.sprite = item.getSprite();

                this.image.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
