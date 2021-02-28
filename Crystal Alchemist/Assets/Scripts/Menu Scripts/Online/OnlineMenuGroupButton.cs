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
        private Selectable selectable;

        [SerializeField]
        private TextMeshProUGUI masterField;

        [SerializeField]
        private TextMeshProUGUI playerField;

        [SerializeField]
        private Image lockImage;

        [SerializeField]
        private Image combatImage;

        private int maxPlayers;
        private int playerCount;
        private string roomName;
        private bool isPrivate;
        private bool canJoin;
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
            this.canJoin = info.IsOpen;

            this.combatImage.gameObject.SetActive(!this.canJoin);
            this.lockImage.gameObject.SetActive(this.isPrivate);
        }

        public void SetInteractable(bool freeSlots)
        {
            this.selectable.interactable = freeSlots && this.canJoin;
        }
    }
}
