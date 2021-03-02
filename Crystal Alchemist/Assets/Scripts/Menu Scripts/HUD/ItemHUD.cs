using UnityEngine;

namespace CrystalAlchemist
{
    public class ItemHUD : MonoBehaviour
    {
        [SerializeField]
        private Transform content;

        [SerializeField]
        private ItemHUDElement template;

        private void Awake()
        {
            template.gameObject.SetActive(false);
        }

        private void Start()
        {
            GameEvents.current.OnCollect += ShowItem;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnCollect -= ShowItem;
        }

        private void ShowItem(ItemDrop drop)
        {
            if (drop.stats.itemType == ItemType.consumable) return;

            ItemHUDElement element = Instantiate(this.template, this.content);
            element.SetElement(drop);
            element.gameObject.SetActive(true);
        }
    }
}
