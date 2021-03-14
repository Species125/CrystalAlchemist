using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrystalAlchemist
{
    public class InventoryTab : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory inventory;

        [SerializeField]
        private InventoryType inventoryType;

        [SerializeField]
        private bool preferInventoryIcon = true;

        [SerializeField]
        private InventorySlot template;

        [SerializeField]
        private Transform content;

        private void Start()
        {
            List<InventoryItem> inv = inventory.GetInventory(inventoryType);
            if (inv == null) return;

            List<InventoryItem> sorted = inv.OrderByDescending(x => (int)(x.rarity)).ThenBy(x => x.name).ToList();

            foreach (InventoryItem item in sorted)
            {
                InventorySlot slot = Instantiate(this.template, this.content);
                slot.SetItem(item);
                slot.preferInventoryIcon = this.preferInventoryIcon;
                slot.gameObject.SetActive(true);
            }
        }
    }
}
