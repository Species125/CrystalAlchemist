using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class OnlineMenuGroupList : MonoBehaviourPunCallbacks
    {
        [BoxGroup("UI")]
        [SerializeField]
        private OnlineMenuGroupButton template;

        [BoxGroup("UI")]
        [SerializeField]
        private Transform content;

        [BoxGroup("UI")]
        [SerializeField]
        private Selectable createButton;

        [BoxGroup("UI")]
        [SerializeField]
        private IntValue maxPlayerCount;

        [BoxGroup("UI")]
        [SerializeField]
        private GameObject errorMessage;

        private List<OnlineMenuGroupButton> groupList = new List<OnlineMenuGroupButton>();

        [BoxGroup("Debug")]
        [SerializeField]
        [ReadOnly]
        private int playerCount;

        [BoxGroup("Debug")]
        [SerializeField]
        [ReadOnly]
        private bool canJoin = true;

        private void Start()
        {
            template.gameObject.SetActive(false);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            this.playerCount = 0;

            foreach (OnlineMenuGroupButton button in this.groupList) Destroy(button.gameObject);
            this.groupList.Clear();

            foreach (RoomInfo info in roomList)
            {
                if (info.PlayerCount <= 0) continue;
                this.playerCount += info.PlayerCount;

                OnlineMenuGroupButton button = Instantiate(this.template, this.content);
                button.gameObject.SetActive(true);
                button.SetButton(info);
                this.groupList.Add(button);
            }

            UpdateJoinPossible();
        }

        private void UpdateJoinPossible()
        {
            this.canJoin = this.playerCount < this.maxPlayerCount.GetValue();

            this.errorMessage.SetActive(!this.canJoin);

            this.createButton.interactable = this.canJoin;
            foreach (OnlineMenuGroupButton button in this.groupList)
            {
                button.GetComponent<Button>().interactable = this.canJoin;
            }
        }

        public override void OnJoinedRoom()
        {
            //close
        }
    }
}
