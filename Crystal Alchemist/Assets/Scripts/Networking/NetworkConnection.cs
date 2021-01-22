using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace CrystalAlchemist
{
    public class NetworkConnection : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private NetworkSettings settings;

        [SerializeField]
        private PlayerSaveGame saveGame;

        [SerializeField]
        private StringValue scene;

        [SerializeField]
        private TextMeshProUGUI debug;

        [SerializeField]
        private Image loadingBar;

        private bool loaded = false;

        private void Start()
        {
            this.debug.text = "";
            SetProgress(0);
            NetworkUtil.SetRoomStatus(false);

            if (this.settings.offlineMode != PhotonNetwork.OfflineMode) //Offline Mode changed
            {
                if (!PhotonNetwork.OfflineMode && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect(); //when online -> disconnect
                else PhotonNetwork.OfflineMode = this.settings.offlineMode; //offline
            }            

            if (!PhotonNetwork.IsConnected)
            {
                Connect();
                return;
            }

            if (PhotonNetwork.InRoom)
            {
                FinishConnection();
                return;
            }
        }

        public void Connect()
        {
            PhotonNetwork.SendRate = this.settings.sendRate;
            PhotonNetwork.SerializationRate = this.settings.serializationRate;
            PhotonNetwork.AutomaticallySyncScene = this.settings;
            PhotonNetwork.NickName = this.settings.nickname;
            PhotonNetwork.GameVersion = this.settings.version;
            PhotonNetwork.ConnectUsingSettings();
        }

        private void FinishConnection()
        {
            if (this.loaded) return;
            SetProgress(1);
            AddText("Loading " + this.scene.GetValue());
            if (NetworkUtil.IsMaster()) PhotonNetwork.LoadLevel(this.scene.GetValue());
            this.loaded = true;
        }

        private void AddText(string text)
        {
            if (this.debug) this.debug.text += text + Environment.NewLine;
            Debug.Log(text);
        }

        private void SetProgress(float value)
        {
            if (this.loadingBar) this.loadingBar.fillAmount = value;
        }

        private void OnDestroy()
        {
            NetworkUtil.SetRoomStatus(true);
        }

        private void CreateRoom()
        {
            if (!PhotonNetwork.IsConnected) return;

            RoomOptions options = new RoomOptions();
            options.PublishUserId = true;
            options.MaxPlayers = this.settings.maxPlayers;
            options.IsVisible = true;
            options.IsOpen = true;
            PhotonNetwork.JoinOrCreateRoom(this.settings.roomName, options, TypedLobby.Default);
        }

        public override void OnCreatedRoom()
        {
            SetProgress(0.75f);
            AddText("Room created");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            AddText("Room failed on " + message);
        }

        public override void OnJoinedRoom()
        {
            AddText("Room joined");
            FinishConnection();
        }

        public override void OnJoinedLobby()
        {
            AddText("Lobby joined");
            SetProgress(0.5f);
            CreateRoom();            
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            AddText("Join failed on " + message);
        }

        public override void OnLeftRoom()
        {
            AddText("Room left");
        }

        public override void OnLeftLobby()
        {
            AddText("Lobby left");
        }

        public override void OnConnectedToMaster()
        {
            AddText("Connected To Server");
            SetProgress(0.25f);
            if (!PhotonNetwork.OfflineMode && !PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
            else CreateRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            AddText("Disconnected because " + cause.ToString());

            if (!PhotonNetwork.OfflineMode)
            {
                AddText("Start Offline Mode");
                this.settings.SetOfflineStatus(true);
                PhotonNetwork.OfflineMode = true;
            }
        }
    }
}
