using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class OnlineMenuCCU : MonoBehaviourPunCallbacks
    {
        [BoxGroup("UI")]
        [SerializeField]
        private IntValue maxPlayerCount;

        /*
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            this.playerCount = 0;

            foreach (RoomInfo info in roomList)
            {
                this.playerCount += info.PlayerCount;
            }

            if (this.playerCount >= this.maxPlayerCount.GetValue()) this.createButton.interactable = false;
            else this.createButton.interactable = true;
        }*/
    }
}
