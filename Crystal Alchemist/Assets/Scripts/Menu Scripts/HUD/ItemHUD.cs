using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ItemHUD : MonoBehaviour
    {
        [SerializeField]
        private Transform content;

        [SerializeField]
        private ItemHUDElement template;

        private List<ItemHUDElement> elements = new List<ItemHUDElement>();

        private void Awake()
        {
            template.gameObject.SetActive(false);
        }

        private void Start()
        {
            GameEvents.current.OnCollect += ShowItems;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnCollect -= ShowItems;
        }

        private void ShowItems(ItemDrop drop, int amount)
        {
            this.elements.RemoveAll(x => x == null);

            foreach (ItemStats stats in drop.items)
            {
                if (stats.itemType == ItemType.consumable) continue;

                ItemHUDElement exists = GetElement(stats);
                if (exists == null) CreateNewHUDElement(stats, amount);
                else exists.UpdateElement(amount);
            }
        }

        private ItemHUDElement GetElement(ItemStats drop)
        {
            foreach(ItemHUDElement elem in this.elements)
            {
                if (elem.HasDrop(drop)) return elem;
            }
            return null;
        }

        private void CreateNewHUDElement(ItemStats drop, int amount)
        {
            ItemHUDElement element = Instantiate(this.template, this.content);
            element.SetElement(drop, amount);
            element.gameObject.SetActive(true);
            this.elements.Add(element);
        }
    }
}
