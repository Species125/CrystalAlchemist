using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace CrystalAlchemist
{
    public class OnlineMenuGroupList : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private OnlineMenuGroupButton template;

        [SerializeField]
        private Transform content;

        private List<OnlineMenuGroupButton> groupList = new List<OnlineMenuGroupButton>();

        private void Start()
        {
            template.gameObject.SetActive(false);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach(RoomInfo info in roomList)
            {
                if (info.RemovedFromList)
                {
                    int index = this.groupList.FindIndex(x => x.info.Name == info.Name);
                    if(index >= 0)
                    {
                        Destroy(this.groupList[index].gameObject);
                        this.groupList.RemoveAt(index);
                    }
                }
                else 
                {
                    OnlineMenuGroupButton button = Instantiate(this.template, this.content);
                    button.gameObject.SetActive(true);
                    button.SetButton(info);
                    this.groupList.Add(button);
                }
            }
        }
    }
}
