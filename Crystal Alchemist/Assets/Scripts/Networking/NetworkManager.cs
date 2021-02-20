using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

namespace CrystalAlchemist
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Player prefab;

        [SerializeField]
        private NetworkSettings settings;

        private bool returnToTitleScreen = false;

        public override void OnEnable()
        {
            base.OnEnable();
            GameEvents.current.OnTitleScreen += ReturnTitleScreen;
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
            GameEvents.current.OnPlayerSpawnCompleted += ShowKickMessage;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            GameEvents.current.OnTitleScreen -= ReturnTitleScreen;
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingEvent;
            GameEvents.current.OnPlayerSpawnCompleted -= ShowKickMessage;
        }

        private void NetworkingEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.DISCONNECT)
            {
                object[] datas = (object[])obj.CustomData;
                int ID = (int)datas[0];

                if (NetworkUtil.GetLocalPlayer().photonView.ViewID == ID)
                {
                    this.settings.gotKicked.SetValue(true);
                    Disconnect();
                    return;
                }
            }
        }

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                GameEvents.current.DoChangeScene(SceneManager.GetActiveScene().name);
                return;
            }
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom) Instantiate();
        }      
        
        private void ShowKickMessage()
        {
            if (this.settings.gotKicked.GetValue())
            {
                if (this.GetComponent<MenuDialogBoxLauncher>() != null) this.GetComponent<MenuDialogBoxLauncher>().ShowDialogBox();
                this.settings.gotKicked.SetValue(false);
            }
        }

        private void Instantiate()
        {
            GameObject player = PhotonNetwork.Instantiate(this.prefab.path, Vector2.zero, Quaternion.identity, 0);
            player.GetComponent<Player>().uniqueID = PhotonNetwork.LocalPlayer.UserId;
        }

        private void Disconnect()
        {
            settings.offlineMode.SetValue(true);
            PhotonNetwork.Disconnect();
            GameEvents.current.DoChangeScene(this.settings.currentScene.GetValue()); //go back to last scene
        }

        private void ReturnTitleScreen()
        {
            this.returnToTitleScreen = true;
            settings.offlineMode.SetValue(true);
            GameEvents.current.DoSaveGame();
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
        {
            if(this.returnToTitleScreen) SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            NetworkEvents.current.PlayerEnteredRoom(newPlayer);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player newPlayer)
        {
            NetworkEvents.current.PlayerLeftRoom(newPlayer);
        }
    }
}
