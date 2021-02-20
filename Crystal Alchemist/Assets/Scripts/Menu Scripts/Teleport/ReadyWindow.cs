using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;

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
        private bool blnIsReady = false;


        public override void Start()
        {
            base.Start();
            Inititialize();            
        }

        private void Inititialize()
        {            
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;

            this.stats = Resources.Load<TeleportStats>(this.path.GetValue());

            this.countDown = this.duration;
            InvokeRepeating("Updating", 0, 1); //countdown
            this.bar.DOFillAmount(0, this.duration);

            UpdateBoxList(); //add players to UI
        }

        private void NetworkingEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.READY_SET)
            {
                object[] datas = (object[])obj.CustomData;
                int ID = (int)datas[0];
                bool value = (bool)datas[1];

                UpdateStatus(ID, value);
            }  
            else if(obj.Code == NetworkUtil.READY_CANCEL)
            {
                GameEvents.current.DoIngameMessage("Teleport abgebrochen");
                base.ExitMenu();
            }
        }

        private void UpdateBoxList()
        {
            this.template.gameObject.SetActive(true);

            foreach(ReadyWindowBox box in this.boxes) Destroy(box.gameObject);
            this.boxes.Clear();

            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                Player player = (Player)p.TagObject;
                int ID = p.GetPlayerNumber();

                ReadyWindowBox newBox = Instantiate(this.template, this.content);
                newBox.SetBox(player.GetCharacterName(), ID);
                this.boxes.Add(newBox);
            }

            this.template.gameObject.SetActive(false);
        }

        private void Updating()
        {
            this.countDown--;
            this.countDownField.text = this.countDown + "s";

            if (this.countDown <= 0) this.ExitMenu();
        }

        public override void OnDestroy()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingEvent;
            base.OnDestroy();
        }

        public void Ready() => SetReadyWindow();        

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
            this.teleportList.SetNextTeleport(this.stats, true, true);
            GameEvents.current.DoTeleport();            

            ExitMenu();
        }

        public void AbortTeleport()
        {
            ExitMenu();
            RPCCancelTeleport();
        }

        public void SetReadyWindow()
        {
            if (this.blnIsReady) this.blnIsReady = false;
            else this.blnIsReady = true;

            RPCSetReadyStatus(this.blnIsReady);
        }

        private void RPCSetReadyStatus(bool value)
        {
            int ID = PhotonNetwork.LocalPlayer.GetPlayerNumber();

            object[] datas = new object[] { ID, value };
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.READY_SET, datas, options, SendOptions.SendUnreliable);
        }

        private void RPCCancelTeleport()
        {
            object[] datas = new object[] { };
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.READY_CANCEL, datas, options, SendOptions.SendUnreliable);
        }

        public override void OnJoinedRoom() => UpdateBoxList();
        
        public override void OnLeftRoom() => UpdateBoxList();
    }
}
