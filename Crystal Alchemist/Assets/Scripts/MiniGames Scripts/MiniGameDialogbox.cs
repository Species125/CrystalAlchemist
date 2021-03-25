using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class MiniGameDialogbox : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory inventory;

        [SerializeField]
        private MiniGameSlider slider;

        [SerializeField]
        private MiniGamePrice priceUI;

        [SerializeField]
        private ItemUI itemUI;

        [SerializeField]
        private ItemUI winUI;

        [SerializeField]
        private Selectable startButton;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private MiniGameTrys trySlots;

        [SerializeField]
        private CustomCursor cursor;

        private MiniGameInfo info;
        private string text;

        private void Start()
        {
            MiniGameEvents.current.OnDifficultyChanged += UpdateDialogBox;
        }

        private void OnEnable()
        {
            this.cursor.gameObject.SetActive(true);
        }

        private void OnDisable() => this.cursor.gameObject.SetActive(false);

        private void OnDestroy() => MiniGameEvents.current.OnDifficultyChanged -= UpdateDialogBox;

        public void Show(MiniGameInfo info)
        {
            this.info = info;
            this.info.matches.Initialize(); //Set Items

            this.text = this.info.GetDescription();
            this.slider.SetStars(this.info.matches.GetCount());
            this.slider.SetDifficulty(1);

            UpdateDialogBox();
        }

        public void UpdateDialogBox()
        {
            int difficulty = this.slider.GetValue();
            MiniGameMatch match = this.info.matches.GetMatch(difficulty);

            this.itemUI.SetItem(match.GetItem());
            this.winUI.SetItem(match.GetItem());

            bool canStart = this.priceUI.CheckPrice(this.inventory, match.price);
            this.startButton.interactable = canStart;

            this.trySlots.SetValues(match.winsNeeded, match.maxRounds);
            this.descriptionText.text = this.info.miniGameUI.GetDescription(this.text, difficulty);
        }

        public void StartMiniGame()
        {
            int difficulty = this.slider.GetValue();
            MiniGameMatch match = this.info.matches.GetMatch(difficulty);
            MiniGameEvents.current.StartMiniGameRound(match);
            this.gameObject.SetActive(false);
        }
    }
}
