using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

namespace CrystalAlchemist
{
    public class OnlineMenuGroupButton : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private TextMeshProUGUI masterField;

        [SerializeField]
        private TextMeshProUGUI playerField;

        [SerializeField]
        private NetworkSettings settings;

        [SerializeField]
        private Image lockImage;

        private int maxPlayers;
        private int playerCount;
        private string roomName;
        private bool isPrivate;
        public RoomInfo info;

        public void SetButton(RoomInfo info)
        {
            this.info = info;

            this.roomName = this.info.Name;
            this.maxPlayers = this.info.MaxPlayers;
            this.playerCount = this.info.PlayerCount;

            this.masterField.text = this.roomName;
            this.playerField.text = this.playerCount + "/" + maxPlayers;
            
            this.isPrivate = Convert.ToBoolean(this.info.CustomProperties["Private"]);
            this.lockImage.gameObject.SetActive(this.isPrivate);
        }
    }
}
