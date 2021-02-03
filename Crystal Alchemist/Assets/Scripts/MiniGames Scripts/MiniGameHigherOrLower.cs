using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGameHigherOrLower : MiniGameRound
    {
        [SerializeField]
        private List<int> maxRandomNumbers = new List<int>();

        [SerializeField]
        private List<MiniGameCard> cards = new List<MiniGameCard>();

        [SerializeField]
        private GameObject controls;

        private List<int> randomNumbers = new List<int>();
        private int value;

        public override void Start()
        {
            base.Start();
            this.controls.SetActive(false);
            setRandomNumbers();
            this.cards[0].show();
        }

        public override string GetDescription(string text, int difficulty)
        {
            string result = text;
            result = result.Replace("<min>", "1");
            result = result.Replace("<max>", (this.maxRandomNumbers[difficulty - 1]) + "");
            return result;
        }

        private void setRandomNumbers()
        {
            int max = (this.maxRandomNumbers[this.GetDifficulty() - 1] + 1);
            int start = 0;

            if (max == 4)
            {
                this.randomNumbers.Add(2);
                this.cards[0].setValue(2);
                start = 1;
            }

            for (int i = start; i < this.cards.Count; i++)
            {
                int rand = 0;
                do
                {
                    rand = Random.Range(1, max);
                }
                while (this.randomNumbers.Contains(rand));
                this.randomNumbers.Add(rand);
                this.cards[i].setValue(rand);
            }
        }

        public void OnClick(int value)
        {
            this.cards[1].show();
            this.value = value;
            this.StopTimer();
            this.controls.SetActive(false);
        }

        public override void Check()
        {
            if (this.value != 0)
            {
                if ((this.randomNumbers[0] < this.randomNumbers[1] && this.value == 1)
                    || (this.randomNumbers[0] > this.randomNumbers[1] && this.value == -1)) this.EndRound(true);
                else this.EndRound(false);
            }
            else
            {
                this.controls.SetActive(true);
                StartTimer();
            }
        }
    }
}
