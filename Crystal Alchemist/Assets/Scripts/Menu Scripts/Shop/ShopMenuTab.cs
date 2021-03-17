using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class ShopMenuTab : MonoBehaviour
    {
        [SerializeField]
        private Image icon;

        [SerializeField]
        private Image bigIcon;

        [SerializeField]
        private GameObject big;

        private GameObject tab;
        private ShopMenu main;

        public void SetTab(Sprite icon, GameObject tab, ShopMenu main)
        {
            this.icon.sprite = icon;
            this.bigIcon.sprite = icon;
            this.tab = tab;
            this.main = main;
        }

        public void SetActiveTab(bool value)
        {
            this.big.SetActive(value);
            this.tab.SetActive(value);
        }

        public void SwitchTab()
        {
            this.main.CloseTabs();
            SetActiveTab(true);
        }
    }
}
