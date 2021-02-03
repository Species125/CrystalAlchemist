


using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGame : Interactable
    {
        [SerializeField]
        [Required]
        [BoxGroup("Mandatory")]
        private MiniGameInfo miniGame;

        [SerializeField]
        [Required]
        [BoxGroup("Mandatory")]
        private MiniGameInfo UIInfo;

        public override void Start()
        {
            base.Start();
            this.UIInfo.matches = this.miniGame.matches;
            this.UIInfo.miniGameUI = this.miniGame.miniGameUI;
            this.UIInfo.miniGameName = this.miniGame.miniGameName;
        }

        public override void DoOnSubmit()
        {
            MenuEvents.current.OpenMiniGame();
        }
    }
}
