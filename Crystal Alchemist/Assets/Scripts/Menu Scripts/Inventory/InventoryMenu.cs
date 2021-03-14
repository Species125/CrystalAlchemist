using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class InventoryMenu : MenuBehaviour
    {
        [SerializeField]
        private List<GameObject> tabs = new List<GameObject>();

        private void Awake()
        {
            CloseTabs(tabs[0]);
        }

        public override void Cancel()
        {
            CloseTabs(tabs[0]);
            base.Cancel();
        }

        public void CloseTabs(GameObject openTab)
        {
            foreach(GameObject tab in this.tabs)
            {
                tab.SetActive(false);
            }

            openTab.SetActive(true);
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

        public void OpenOnlineMenu()
        {
            MenuEvents.current.OpenOnlineMenu();
            ExitMenu();
        }
    }
}
