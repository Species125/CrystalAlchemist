using UnityEngine;
using TMPro;

namespace CrystalAlchemist
{
    public class ShopMenuPrice : InventorySlot
    {
        [SerializeField]
        private TextMeshProUGUI priceText;

        public void SetPrice(InventoryItem item, int value, PlayerInventory inventory, int amount, bool colorIt)
        {
            if (item == null) return;

            SetItem(item);

            int needed = value*amount;
            int player = inventory.GetAmount(item);

            this.priceText.text = needed + "/" + player;

            if (colorIt) SetColor(needed <= player);
            else SetColor(true);
        }

        private void SetColor(bool canStart)
        {
            if (canStart) this.priceText.color = Color.white;
            else this.priceText.color = Color.red;
        }
    }
}
