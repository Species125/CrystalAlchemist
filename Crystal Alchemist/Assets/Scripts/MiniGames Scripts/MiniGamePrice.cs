




using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class MiniGamePrice : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI textField;

        [SerializeField]
        private TextMeshProUGUI textLabel;

        public bool CheckPrice(PlayerInventory inventory, Costs price)
        {
            this.image.enabled = false;

            if (price.item != null && price.resourceType == CostType.item)
            {
                this.image.enabled = true;

                int inventoryAmount = inventory.GetAmount(price.item);

                this.image.sprite = price.item.info.getSprite();
                this.textField.text = price.amount + " / " + inventoryAmount;
                this.textLabel.text = FormatUtil.GetLocalisedText("Kosten", LocalisationFileType.menues);
            }
            else if (price.keyItem != null && price.resourceType == CostType.keyItem)
            {
                this.image.enabled = true;

                int amount = 0;
                if (GameEvents.current.HasKeyItem(price.keyItem.name)) amount = 1;

                this.image.sprite = price.keyItem.stats.info.getSprite();
                this.textField.text = "1 / " + amount;
                this.textLabel.text = FormatUtil.GetLocalisedText("Benötigt", LocalisationFileType.menues);
            }
            else if (price.resourceType == CostType.none)
            {
                this.textField.text = "";
            }

            bool canStart = GameEvents.current.HasEnoughCurrency(price);
            setColor(canStart);

            return canStart;
        }

        private void setColor(bool canStart)
        {
            if (canStart) this.textField.color = Color.white;
            else this.textField.color = Color.red;
        }
    }
}
