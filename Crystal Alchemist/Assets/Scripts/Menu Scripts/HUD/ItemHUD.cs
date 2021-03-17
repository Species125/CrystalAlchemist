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
            if (drop.stats.itemType == ItemType.consumable) return;

            ItemHUDElement exists = GetElement(drop);
            if (exists == null) CreateNewHUDElement(drop, amount);
            else exists.UpdateElement(amount);            
        }

        private ItemHUDElement GetElement(ItemDrop drop)
        {
            foreach(ItemHUDElement elem in this.elements)
            {
                if (elem.HasDrop(drop)) return elem;
            }
            return null;
        }

        private void CreateNewHUDElement(ItemDrop drop, int amount)
        {
            ItemHUDElement element = Instantiate(this.template, this.content);
            element.SetElement(drop, amount);
            element.gameObject.SetActive(true);
            this.elements.Add(element);
        }
    }
}
