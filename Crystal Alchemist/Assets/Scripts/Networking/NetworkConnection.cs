using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class NetworkConnection : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private NetworkSettings settings;

        private void Start()
        {
            PhotonNetwork.SendRate = this.settings.sendRate;
            PhotonNetwork.SerializationRate = this.settings.serializationRate;
            PhotonNetwork.AutomaticallySyncScene = this.settings;
            PhotonNetwork.NickName = this.settings.nickname;
            PhotonNetwork.GameVersion = this.settings.version;

            if (!PhotonNetwork.IsConnected
                && !PhotonNetwork.InRoom) PhotonNetwork.ConnectUsingSettings();
        }

        private void CreateRoom()
        {
            if (!PhotonNetwork.IsConnected) return;

            RoomOptions options = new RoomOptions();
            options.PublishUserId = true;
            options.MaxPlayers = this.settings.maxPlayers;
            PhotonNetwork.JoinOrCreateRoom(this.settings.roomName, options, TypedLobby.Default);
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
            SceneManager.LoadScene("Photon Test");
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
            //PhotonNetwork.OfflineMode = true;
            //Debug.Log("Start Offline Mode");
        }
    }
}
