using System.Collections.Generic;


using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillPageSelect : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textField;

        [SerializeField]
        private List<GameObject> pages = new List<GameObject>();

        [SerializeField]
        private GameObject nextButton;

        [SerializeField]
        private GameObject previousButton;

        private int index;

        private void Start()
        {
            GameEvents.current.OnPage += SetNextPage;
            UpdateButtons();
            for (int i = 1; i < this.pages.Count; i++) this.pages[i].SetActive(false);
        }

        private void OnDestroy()
        {
            GameEvents.current.OnPage -= SetNextPage;
        }

        private void UpdateButtons()
        {
            if (this.index == 0) this.previousButton.SetActive(false);
            else this.previousButton.SetActive(true);

            if (this.index == this.pages.Count - 1) this.nextButton.SetActive(false);
            else this.nextButton.SetActive(true);

            this.textField.text = (this.index + 1) + "/" + this.pages.Count;
        }

        public void SetNextPage(int value)
        {
            if (!this.gameObject.activeInHierarchy) return;

            for (int i = 0; i < this.pages.Count; i++)
            {
                if (pages[i].activeInHierarchy) this.index = i;
            }

            pages[this.index].SetActive(false);

            this.index += value;
            if (this.index < 0) this.index = 0;
            else if (this.index >= this.pages.Count) this.index = this.pages.Count - 1;

            pages[this.index].SetActive(true);

            UpdateButtons();

            if (!this.nextButton.activeInHierarchy) this.previousButton.GetComponent<ButtonExtension>().Select();
            else if (!this.previousButton.activeInHierarchy) this.nextButton.GetComponent<ButtonExtension>().Select();
        }
    }
}

