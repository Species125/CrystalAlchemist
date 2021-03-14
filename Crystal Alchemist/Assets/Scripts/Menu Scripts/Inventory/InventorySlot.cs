using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class InventorySlot : ItemUI
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        [Min(0)]
        private float transparency = 0.3f;

        public override void SetItem(InventoryItem item)
        {
            base.SetItem(item);

            Color color = GameUtil.GetRarity(item.rarity);
            this.background.color = new Color(color.r, color.g, color.b, this.transparency);
        }
    }
}
