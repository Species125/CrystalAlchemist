using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Sirenix.OdinInspector;
using ExitGames.Client.Photon;

public class CrystalNetworkTest : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private bool offlineMode = false;

    [SerializeField]
    private int sendRate = 10;

    [SerializeField]
    private int serializationRate = 10;

    [SerializeField]
    private string nickname = "Gungnir";

    [SerializeField]
    private string version = "0.0.1";

    [SerializeField]
    private byte maxPlayers = 4;

    [SerializeField]
    private string roomName = "Online";

    [SerializeField]
    private Player player;

    [SerializeField]
    private Vector3 startPosition = new Vector3(0, 0, -99);

    public static GameObject local;

    private void Start()
    {       
        PhotonNetwork.OfflineMode = this.offlineMode;
        PhotonNetwork.SendRate = this.sendRate;
        PhotonNetwork.SerializationRate = this.serializationRate;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = this.nickname;
        PhotonNetwork.GameVersion = this.version;

        Debug.Log("Connecting To Server");
        PhotonNetwork.ConnectUsingSettings();
    }

    [Button]
    public void switchScene(string name)
    {
        if (NetworkUtil.IsMaster())
        {
            //PhotonNetwork.CurrentRoom.IsOpen = false; //no join when room is open
            //PhotonNetwork.CurrentRoom.IsVisible = false; //not visible when room is there
            PhotonNetwork.LoadLevel(name);           
        }
    }

    [Button]
    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected) return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = this.maxPlayers;
        PhotonNetwork.JoinOrCreateRoom(this.roomName, options, TypedLobby.Default);
    }

    [Button]
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [Button]
    public void Instantiate()
    {
        local = PhotonNetwork.Instantiate(this.player.path, this.startPosition, Quaternion.identity,0);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room failed on "+message);
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
        //PhotonNetwork.OfflineMode = true;
        //Debug.Log("Start Offline Mode");
    }
}
