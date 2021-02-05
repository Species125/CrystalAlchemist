using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGameUI : MenuBehaviour
    {
        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private MiniGameInfo info;

        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private MiniGameDialogbox dialogBox;

        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private TextMeshProUGUI titleField;

        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private GameObject parent;

        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private MiniGameTrys trySlots;

        [BoxGroup("Texts")]
        [SerializeField]
        [Required]
        private MiniGameText successText;

        [BoxGroup("Texts")]
        [SerializeField]
        [Required]
        private MiniGameText failText;

        [BoxGroup("Texts")]
        [SerializeField]
        [Required]
        private MiniGameText winText;

        [BoxGroup("Texts")]
        [SerializeField]
        [Required]
        private MiniGameText loseText;

        private MiniGameRound activeRound;
        private MiniGameMatch activeMatch;

        public override void Start()
        {
            base.Start();
            MiniGameEvents.current.OnStartRound += StartMiniGame;
            MiniGameEvents.current.OnEndRound += RoundEnd;

            this.titleField.text = this.info.GetName();
            ShowDialogBox();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            MiniGameEvents.current.OnStartRound -= StartMiniGame;
            MiniGameEvents.current.OnEndRound -= RoundEnd;
        }

        public void ShowDialogBox()
        {
            this.trySlots.Reset();
            this.dialogBox.gameObject.SetActive(true);
            this.dialogBox.Show(this.info);
        }

        public void StartMiniGame(MiniGameMatch match)
        {
            GameEvents.current.DoReduce(match.price);
            this.activeMatch = match;
            StartRound();
        }

        public void StartRound()
        {
            if (this.activeRound != null) Destroy(this.activeRound.gameObject);

            MiniGameState state = this.trySlots.canStartNewRound();

            if (state == MiniGameState.play)
            {
                this.activeRound = Instantiate(info.miniGameUI, this.parent.transform);
                this.activeRound.Initialize(this.activeMatch.difficulty, this.activeMatch.maxDuration);
            }
            else
            {
                if (state == MiniGameState.win) PlayerWin(this.activeMatch.GetItem());
                else if (state == MiniGameState.lose) PlayerLose();
            }
        }

        private void PlayerWin(ItemDrop drop)
        {
            GameEvents.current.DoCollect(drop);
            ShowTextBox(this.winText);
        }

        private void PlayerLose()
        {
            ShowTextBox(this.loseText);
        }

        private void ShowTextBox(MiniGameText textObject)
        {
            if (textObject != null) textObject.gameObject.SetActive(true);
        }

        public void RoundEnd(bool success)
        {
            MiniGameState state = this.trySlots.canStartNewRound();

            if (success)
            {
                this.trySlots.SetSlot(true);
                if (state == MiniGameState.play) ShowTextBox(this.successText);
            }
            else
            {
                this.trySlots.SetSlot(false);
                if (state == MiniGameState.play) ShowTextBox(this.failText);
            }
        }
    }
}
