using UnityEngine;

namespace CrystalAlchemist
{
    public class InventoryArtifactSlot : ItemUI
    {
        [SerializeField]
        private InventoryItem item;

        private void Start()
        {
            if (GameEvents.current.HasItemAlready(item)) SetItem(item);            
        }

        public void OnClick()
        {
            InventoryItem artifact = GetInventoryItem();
            if (artifact != null) artifact.RaiseMenuSignal();
        }
    }
}