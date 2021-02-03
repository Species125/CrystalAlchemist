using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGameSlider : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> stars = new List<GameObject>();

        private int value = 1;
        private int maxValue = 1;

        public void SetStars(int maxValue)
        {
            this.maxValue = maxValue;

            for (int i = 0; i < this.stars.Count; i++)
            {
                this.stars[i].SetActive(false);
                if (i < this.maxValue) this.stars[i].SetActive(true);
            }
        }

        public void SetDifficulty(int value)
        {
            if (this.value == value && value > 1) this.value--;
            else this.value = value;

            for (int i = 0; i < maxValue; i++)
            {
                this.stars[i].transform.GetChild(0).gameObject.SetActive(false);
                if (i < this.value) this.stars[i].transform.GetChild(0).gameObject.SetActive(true);
            }

            MiniGameEvents.current.SetDifficulty();
        }

        public int GetValue()
        {
            return this.value;
        }
    }
}
