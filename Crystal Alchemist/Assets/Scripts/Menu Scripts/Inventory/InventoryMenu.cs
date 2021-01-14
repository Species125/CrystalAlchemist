using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class InventoryMenu : MenuBehaviour
    {
        [BoxGroup("Tabs")]
        [SerializeField]
        private GameObject top;

        [BoxGroup("Tabs")]
        [SerializeField]
        private GameObject bottom;

        [SerializeField]
        private List<InventoryPage> pages = new List<InventoryPage>();

        public override void Start()
        {
            base.Start();
            foreach (InventoryPage page in this.pages) page.LoadPage();
            ShowTopPage(true);
            MenuEvents.current.OnInventory += ExitMenu;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            MenuEvents.current.OnInventory -= ExitMenu;
        }

        public override void Cancel()
        {
            if (!this.top.activeInHierarchy) ShowTopPage(true);
            else base.Cancel();
        }

        public void switchCategory()
        {
            if (!this.top.activeInHierarchy) ShowTopPage(true);
            else ShowTopPage(false);
        }

        private void ShowTopPage(bool top)
        {
            if (top)
            {
                this.top.SetActive(true);
                this.bottom.SetActive(false);
            }
            else
            {
                this.top.SetActive(false);
                this.bottom.SetActive(true);
            }
        }

        public void OpenMap()
        {
            MenuEvents.current.OpenMap();
            ExitMenu();
        }

        public void OpenSkills()
        {
            MenuEvents.current.OpenSkillBook();
            ExitMenu();
        }

        public void OpenAttributes()
        {
            MenuEvents.current.OpenAttributes();
            ExitMenu();
        }
    }
}
