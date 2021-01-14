

using UnityEngine;

namespace CrystalAlchemist
{
    public class InventoryPage : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory inventory;

        [SerializeField]
        private bool isKeyItemPage = false;

        public void LoadPage()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                InventorySlot slot = this.transform.GetChild(i).GetComponent<InventorySlot>();

                if (slot == null) continue;

                int ID = slot.Initialize();

                if (this.isKeyItemPage)
                {
                    ItemStats item = this.inventory.GetKeyItem(ID);
                    slot.setItemToSlot(item);
                }
                else
                {
                    ItemGroup item = this.inventory.GetInventoryItem(ID);
                    slot.setItemToSlot(item);
                }
            }
        }
    }
}
