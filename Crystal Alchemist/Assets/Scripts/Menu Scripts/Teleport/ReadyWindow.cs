using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class ReadyWindow : MenuBehaviour
    {
        [BoxGroup("Confirmation Window")]
        [SerializeField]
        private int duration = 30;

        [BoxGroup("Confirmation Window")]
        [SerializeField]
        private float delay = 1f;

        [BoxGroup("Confirmation Window")]
        [Required]
        [SerializeField]
        private PlayerTeleportList teleportList;

        [BoxGroup("Confirmation Window")]
        [Required]
        [SerializeField]
        private StringValue path;

        [BoxGroup("Confirmation Window UI")]
        [SerializeField]
        private TextMeshProUGUI textField;



        [BoxGroup("Confirmation Window UI")]
        [SerializeField]
        private ReadyWindowBox template;

        [BoxGroup("Confirmation Window UI")]
        [SerializeField]
        private Transform content;

        [BoxGroup("Confirmation Window UI")]
        [SerializeField]
        private GameObject buttons;

        [BoxGroup("Confirmation Window UI")]
        [SerializeField]
        private CustomCursor cursor;

        [BoxGroup("Confirmation Window Time")]
        [SerializeField]
        private GameObject loadingBar;

        [BoxGroup("Confirmation Window Time")]
        [SerializeField]
        private Image bar;

        [BoxGroup("Confirmation Window Time")]
        [SerializeField]
        private TextMeshProUGUI countDownField;

        private int countDown;
        private List<ReadyWindowBox> boxes = new List<ReadyWindowBox>();
        private TeleportStats stats;


        public override void Start()
        {
            base.Start();
            Inititialize();            
        }

        private void Inititialize()
        {           
            MenuEvents.current.OnTeleportStatus += UpdateStatus; //Listener to others RPC
            this.stats = Resources.Load<TeleportStats>(this.path.GetValue());
            this.textField.text = this.stats.GetTeleportName();

            this.countDown = this.duration;
            InvokeRepeating("Updating", 0, 1); //countdown
            this.bar.DOFillAmount(0, this.duration);

            AddBoxes(); //add players to UI
        }

        private void AddBoxes()
        {
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                Player player = (Player)p.TagObject;
                int ID = p.GetPlayerNumber();

                ReadyWindowBox newBox = Instantiate(this.template, this.content);
                newBox.SetBox(player.GetCharacterName(), ID);
                this.boxes.Add(newBox);
            }

            Destroy(this.template.gameObject);
        }

        private void Updating()
        {
            this.countDown--;
            this.countDownField.text = this.countDown + "s";

            if (this.countDown <= 0) this.ExitMenu();
        }

        public override void OnDestroy()
        {
            MenuEvents.current.OnTeleportStatus -= UpdateStatus;
            base.OnDestroy();
        }

        public void Ready() => NetworkEvents.current.SetReadyWindow(true);        

        public void NotReady() => ExitMenu();

        private void UpdateStatus(int ID, bool value)
        {
            foreach (ReadyWindowBox box in this.boxes)
            {
                if (box.ID == ID)
                {
                    box.UpdateBox(value);
                    continue;
                }
            }

            if (IsReady()) InitiateTeleport();
        }

        private bool IsReady()
        {
            foreach (ReadyWindowBox box in this.boxes)
            {
                if (!box.GetReady()) return false;
            }
            return true;
        }

        private void InitiateTeleport()
        {
            this.buttons.SetActive(false);
            this.cursor.gameObject.SetActive(false);
            this.loadingBar.SetActive(false);

            CancelInvoke();
            Invoke("DoTeleport", this.delay);
        }

        private void DoTeleport()
        {
            this.teleportList.SetNextTeleport(this.stats);
            GameEvents.current.DoTeleport();            

            this.ExitMenu();
        }
    }
}
