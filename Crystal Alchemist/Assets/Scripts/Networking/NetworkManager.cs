using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private NetworkSettings settings;

        private bool playerSpawned = false;

        private void Start()
        {
            PhotonNetwork.OfflineMode = this.settings.offlineMode;

            if (PhotonNetwork.OfflineMode)
            {
                if (!this.playerSpawned) Instantiate();
                return;
            }

            PhotonNetwork.SendRate = this.settings.sendRate;
            PhotonNetwork.SerializationRate = this.settings.serializationRate;
            PhotonNetwork.AutomaticallySyncScene = this.settings;
            PhotonNetwork.NickName = this.settings.nickname;
            PhotonNetwork.GameVersion = this.settings.version;

            if (!PhotonNetwork.IsConnected
                && !PhotonNetwork.InRoom) PhotonNetwork.ConnectUsingSettings();
            else Instantiate();
        }


        private void CreateRoom()
        {
            if (!PhotonNetwork.IsConnected) return;

            RoomOptions options = new RoomOptions();
            options.PublishUserId = true;
            options.MaxPlayers = this.settings.maxPlayers;
            PhotonNetwork.JoinOrCreateRoom(this.settings.roomName, options, TypedLobby.Default);
        }

        public void GoOnline()
        {
            SceneManager.LoadScene("Loading");
        }

        private void Instantiate()
        {
            GameObject player = PhotonNetwork.Instantiate(this.settings.playerPrefab.path, Vector2.zero, Quaternion.identity, 0);
            player.GetComponent<Player>().uniqueID = PhotonNetwork.LocalPlayer.UserId;
            this.playerSpawned = true;
        }

        public override void OnCreatedRoom()
        {            
            Debug.Log("Room created");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Room failed on " + message);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Room joined");
            Instantiate();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Join failed on " + message);
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Room left");
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Server");
            CreateRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected because " + cause.ToString());
        }
    }
}
