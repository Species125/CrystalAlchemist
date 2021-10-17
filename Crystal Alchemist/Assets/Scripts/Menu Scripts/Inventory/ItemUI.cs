using TMPro;
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

        private InventoryItem inventoryItem;
        private ItemStats itemStat;
        private ItemDrop itemDrop;

        public InventoryItem GetInventoryItem()
        {
            return this.inventoryItem;
        }

        public ItemStats GetItemStat()
        {
            return this.itemStat;
        }

        public ItemDrop GetItemDrop()
        {
            return this.itemDrop;
        }

        public void EmptyItem()
        {
            this.itemDrop = null;
            this.inventoryItem = null;
            this.image.gameObject.SetActive(false);
            this.amount.text = "";
        }

        public virtual void SetItem(ItemDrop item)
        {
            this.itemDrop = item;

            if (item == null)
            {
                this.image.gameObject.SetActive(false);
            }
            else
            {
                this.image.gameObject.SetActive(true);

                if (this.amount != null) this.amount.text = "";                

                this.image.sprite = item.GetSprite();
                this.image.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        public virtual void SetItem(InventoryItem item)
        {
            this.inventoryItem = item;

            if (item == null)
            {
                this.image.gameObject.SetActive(false);
            }
            else
            {
                this.image.gameObject.SetActive(true);

                if (this.amount != null)
                {
                    if (item.canConsume && item.GetAmount() > 1) this.amount.text = "x" + item.GetAmount();
                    else this.amount.text = "";
                }

                this.image.sprite = item.icon;
                this.image.color = new Color(1f, 1f, 1f, 1f);
            }
        }        
    }
}
