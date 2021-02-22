using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace CrystalAlchemist
{
    public class OnlineMenuEditGroup : MonoBehaviour
    {
        [BoxGroup("UI Room")]
        [SerializeField]
        private TextMeshProUGUI nameField;

        [BoxGroup("UI Room")]
        [SerializeField]
        private StringValue roomName;

        [BoxGroup("UI Password")]
        [SerializeField]
        private Toggle toggle;

        [BoxGroup("UI Password")]
        [SerializeField]
        private TextMeshProUGUI passwordField;

        [BoxGroup("UI Password")]
        [SerializeField]
        private Selectable passwordButton;

        [BoxGroup("UI Password")]
        [SerializeField]
        private StringValue password;

        [BoxGroup("UI Password")]
        [SerializeField]
        private GameObject passwordBox;

        [BoxGroup("UI Password")]
        [SerializeField]
        private Selectable confirmButton;

        [BoxGroup("UI Players")]
        [SerializeField]
        private OnlineMenuPlayerInfo template;

        [BoxGroup("UI Players")]
        [SerializeField]
        private Transform content;

        private bool isMaster = false;
        private List<OnlineMenuPlayerInfo> infos = new List<OnlineMenuPlayerInfo>();

        private void OnEnable()
        {
            this.isMaster = NetworkUtil.IsMaster();            
            OnToggleChanged();
        }       

        private void Start()
        {
            NetworkEvents.current.OnPlayerSpawned += OnPlayerEnteredRoom;
            NetworkEvents.current.OnPlayerLeft += OnPlayerLeftRoom;

            GetRoomInfos();
            this.template.gameObject.SetActive(false);

            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                if (p.TagObject == null) continue;
                Player player = (Player)p.TagObject;
                if (player == null) continue;
                int ID = player.photonView.ViewID;
                Create(ID);
            }
        }

        private void OnDestroy()
        {
            NetworkEvents.current.OnPlayerSpawned -= OnPlayerEnteredRoom;
            NetworkEvents.current.OnPlayerLeft -= OnPlayerLeftRoom;
        }

        private void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Player p = (Player)otherPlayer.TagObject;
            RemoveInfos(p.photonView.ViewID);
        }

        private void OnPlayerEnteredRoom(PhotonView view)
        {
            AddNewInfos(view.ViewID);
        }

        private void DisableButton()
        {
            if (this.toggle.isOn && this.password.GetValue().Replace(" ", "").Length == 0) this.confirmButton.interactable = false;
            else this.confirmButton.interactable = true;
        }

        private void GetRoomInfos()
        {
            Photon.Realtime.Room room = PhotonNetwork.CurrentRoom;

            this.toggle.isOn = Convert.ToBoolean(room.CustomProperties["Private"]);
            this.roomName.SetValue(room.Name);
            this.password.SetValue(room.CustomProperties["Password"].ToString());
        }

        private void RemoveInfos(int ID)
        {
            foreach (OnlineMenuPlayerInfo info in this.infos)
            {
                if (info.ID == ID)
                {
                    Destroy(info.gameObject);
                }
            }

            this.infos.RemoveAll(x => x == null);
        }

        private void AddNewInfos(int ID)
        {
            foreach (OnlineMenuPlayerInfo info in this.infos)
            {
                if (info.ID == ID) return;
            }

            Create(ID);
        }

        private void Create(int ID)
        {
            OnlineMenuPlayerInfo info = Instantiate(this.template, this.content);
            info.gameObject.SetActive(true);
            info.SetInfos(ID, this.isMaster);
            this.infos.Add(info);
        }

        private void UpdateUI()
        {
            this.nameField.text = this.roomName.GetValue();
            this.passwordField.text = this.password.GetValue();

            this.passwordButton.interactable = this.isMaster;
            this.toggle.interactable = this.isMaster;
            this.confirmButton.interactable = this.isMaster;
        }

        public void OnToggleChanged()
        {
            this.passwordBox.SetActive(this.toggle.isOn);
            if (!this.toggle.isOn) this.password.SetValue("");
            UpdateUI();
            DisableButton();
        }

        public void Confirm()
        {
            Photon.Realtime.Room room = PhotonNetwork.CurrentRoom;            

            ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable { { "Private", this.toggle.isOn }, { "Password", this.password.GetValue() } };
            room.SetCustomProperties(table);            
        }
    }
}