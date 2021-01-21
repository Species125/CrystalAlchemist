using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace CrystalAlchemist {
    public class OnlineMenuGroupButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI masterField;

        [SerializeField]
        private TextMeshProUGUI playerField;

        [SerializeField]
        private string roomName;

        [SerializeField]
        private NetworkSettings settings;

        private int maxPlayers;
        private int playerCount;
        public RoomInfo info;

        private void Start()
        {            
            this.masterField.text = this.roomName;
            this.playerField.text = "";
        }

        public void SetButton(RoomInfo info)
        {
            this.info = info;

            this.roomName = this.info.Name;
            this.maxPlayers = this.info.MaxPlayers;
            this.playerCount = this.info.PlayerCount;            

            this.masterField.text = this.roomName;
            this.playerField.text = this.playerCount + " / " + maxPlayers;
        }

        public void OnClick()
        {
            //PhotonNetwork.JoinRoom(this.roomName);
            this.settings.SetOfflineStatus(false, this.roomName);
            GameEvents.current.DoChangeScene(this.settings.currentScene.GetValue()); //save current scene
        }
    }
}
