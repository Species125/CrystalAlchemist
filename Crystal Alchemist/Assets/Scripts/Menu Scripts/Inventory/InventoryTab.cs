using System.Collections.Generic;
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

            foreach (InventoryItem item in inv)
            {
                InventorySlot slot = Instantiate(this.template, this.content);
                slot.SetItem(item);
                slot.preferInventoryIcon = this.preferInventoryIcon;
                slot.gameObject.SetActive(true);
            }
        }
    }
}
