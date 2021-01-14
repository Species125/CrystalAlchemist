using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class TutorialBox : MenuBehaviour
    {
        [SerializeField]
        private TutorialInfo info;

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TutorialPage template;

        [SerializeField]
        private GameObject content;

        [SerializeField]
        private GameObject nextPage;

        [SerializeField]
        private GameObject previousPage;

        [SerializeField]
        private GameObject close;

        private List<TutorialPage> pages = new List<TutorialPage>();
        private int index = 0;
        private string buttonTextClose;
        private string buttonTextNext;

        public override void Start()
        {
            base.Start();
            this.title.text = this.info.GetTitle();

            foreach (TutorialProperty property in this.info.properties)
            {
                TutorialPage page = Instantiate(this.template, content.transform);
                page.Initialize(property.GetText(), property.firstImage, property.secondImage);
                pages.Add(page);
                page.gameObject.SetActive(false);
            }

            ShowNextPage(0);
            Destroy(this.template.gameObject);
        }

        public void ShowNextPage(int value)
        {
            pages[index].gameObject.SetActive(false);

            this.index += value;

            if (this.index == 0) this.previousPage.gameObject.SetActive(false);
            else this.previousPage.gameObject.SetActive(true);

            if (this.index < this.pages.Count)
            {
                if (index == this.pages.Count - 1) ShowButtonLast(true);
                else ShowButtonLast(false);

                pages[index].gameObject.SetActive(true);
            }
            else ExitMenu();
        }

        private void ShowButtonLast(bool last)
        {
            if (last)
            {
                this.nextPage.SetActive(false);
                this.close.SetActive(true);
            }
            else
            {
                this.nextPage.SetActive(true);
                this.close.SetActive(false);
            }
        }
    }
}
