using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ShopPrice : MonoBehaviour
    {
        [BoxGroup("Text-Attribute")]
        [SerializeField]
        [Required]
        private GameObject child;

        [BoxGroup("Text-Attribute")]
        [SerializeField]
        private TextMeshPro priceText;

        [BoxGroup("Text-Attribute")]
        [SerializeField]
        private SpriteRenderer priceIcon;

        public void Initialize(Costs costs)
        {
            if (costs.resourceType == CostType.item && costs.item != null && costs.item.shopPrice != null)
            {
                this.child.SetActive(true);
                FormatUtil.set3DText(this.priceText, costs.amount + "", true, costs.item.shopPrice.color, costs.item.shopPrice.outline, 0.25f);
                this.priceIcon.sprite = costs.item.shopPrice.shopIcon;
            }
            else this.child.SetActive(false);
        }
    }
}
