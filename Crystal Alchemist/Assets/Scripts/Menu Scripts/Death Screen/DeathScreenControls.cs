using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace CrystalAlchemist
{
    public class DeathScreenControls : MonoBehaviour
    {
        [BoxGroup("Mandatory")]
        [SerializeField]
        private PlayerTeleportList playerTeleport;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private TextMeshProUGUI countDown;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private GameObject returnSavePoint;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private GameObject returnLastPoint;

        [BoxGroup("Time")]
        [SerializeField]
        private float timer = 30;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private TextMeshProUGUI textField;

        [BoxGroup("Time")]
        [SerializeField]
        private float countDownInterval = 1f;

        private void Start()
        {
            InvokeRepeating("Countdown", 0, this.countDownInterval);

            if (this.playerTeleport.HasReturn()) this.returnSavePoint.SetActive(true);
            if (this.playerTeleport.HasLatest()) this.returnLastPoint.SetActive(true);
        }

        private void Countdown()
        {
            if (this.timer <= 0) ReturnToTitleScreen();
            else this.timer -= this.countDownInterval;
            this.countDown.text = (int)this.timer + "s";
        }

        private void DisableButtons()
        {
            CancelInvoke();
            this.gameObject.SetActive(false);
            this.textField.gameObject.SetActive(false);
        }

        public void ReturnToTitleScreen()
        {
            GameEvents.current.DoTitleScreen();
            DisableButtons();
        }

        public void ReturnSaveGame()
        {
            this.playerTeleport.SetReturnTeleport();
            Teleport();
        }

        public void ReturnLastPoint()
        {
            Teleport();
        }

        private void Teleport()
        {
            this.playerTeleport.SetAnimation(true, true);
            GameEvents.current.DoTeleport();
            DisableButtons();
        }
    }
}
