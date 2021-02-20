using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class NetworkConnection : MonoBehaviourPunCallbacks
    {
        [BoxGroup("Network")]
        [SerializeField]
        private NetworkSettings settings;

        [BoxGroup("Network")]
        [SerializeField]
        private PlayerSaveGame saveGame;

        [BoxGroup("Network")]
        [SerializeField]
        private StringValue scene;

        [BoxGroup("Network")]
        [SerializeField]
        private TextMeshProUGUI debug;

        [BoxGroup("Network")]
        [SerializeField]
        private Image loadingBar;

        [BoxGroup("UI")]
        [SerializeField]
        private GameObject loadingInfo;

        [BoxGroup("UI")]
        [SerializeField]
        private GameObject partyFinderMenu;

        [BoxGroup("UI")]
        [SerializeField]
        private Camera cam;

        private bool loaded = false;

        private void Awake()
        {
            this.debug.text = "";
            SetProgress(0);
            NetworkUtil.SetRoomStatus(false);
            this.partyFinderMenu.SetActive(false);
            SetLoadingAppearance();
        }
        
        private void Start()
        {
            if (this.settings.offlineMode.GetValue() != PhotonNetwork.OfflineMode) //Offline Mode changed
            {
                if (!PhotonNetwork.OfflineMode && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect(); //when online -> disconnect
                else PhotonNetwork.OfflineMode = this.settings.offlineMode.GetValue(); //offline
            }            

            if (!PhotonNetwork.IsConnected) //Connect neither if online or offline
            {
                Connect();
                return;
            }

            if (PhotonNetwork.InRoom) //just load level
            {
                LoadScene();
                return;
            }
        }

        private void SetLoadingAppearance()
        {
            if (PhotonNetwork.IsConnected) this.loadingInfo.SetActive(false);
            if (PhotonNetwork.InRoom) this.cam.backgroundColor = Color.black;         
        }

        public void Connect()
        {
            PhotonNetwork.SendRate = this.settings.sendRate;
            PhotonNetwork.SerializationRate = this.settings.serializationRate;
            PhotonNetwork.AutomaticallySyncScene = this.settings;
            PhotonNetwork.NickName = this.settings.nickname;
            PhotonNetwork.GameVersion = this.settings.version;
            PhotonNetwork.ConnectUsingSettings();

            AddText("Connecting to server...");
            SetProgress(0);
            this.loadingInfo.SetActive(true);
        }

        private void LoadScene()
        {
            if (this.loaded) return;
            PhotonNetwork.DestroyAll();
            if (NetworkUtil.IsMaster()) PhotonNetwork.LoadLevel(this.scene.GetValue());
            this.loaded = true;
        }

        public void Disconnect()
        {
            this.settings.offlineMode.SetValue(true);
            PhotonNetwork.Disconnect();
        }

        private void AddText(string text)
        {
            if (PhotonNetwork.OfflineMode) return;

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

        public override void OnCreatedRoom()
        {
            if (PhotonNetwork.InLobby) PhotonNetwork.LeaveLobby();
            SetProgress(0.75f);
            if (!PhotonNetwork.OfflineMode) AddText("Room created");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            AddText("Room failed on " + message);
        }

        public override void OnJoinedRoom()
        {
            if (!PhotonNetwork.OfflineMode)AddText("Room joined");
            LoadScene();
        }

        public override void OnJoinedLobby()
        {
            AddText("Lobby joined");
            SetProgress(1);
            this.loadingInfo.SetActive(true);
            this.partyFinderMenu.SetActive(true);          
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
            else NetworkUtil.CreateRoom("Offline"); //Offline Mode
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            AddText("Disconnected because " + cause.ToString());

            if (!PhotonNetwork.OfflineMode)
            {
                AddText("Start Offline Mode");
                this.settings.offlineMode.SetValue(true);
                PhotonNetwork.OfflineMode = this.settings.offlineMode.GetValue();
            }
        }
    }
}
