using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace CrystalAlchemist
{
    public class PlayerListUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerListUIChild template;

        [SerializeField]
        private Transform content;

        private List<PlayerListUIChild> children = new List<PlayerListUIChild>();

        private void Start()
        {
            NetworkEvents.current.OnPlayerSpawned += OnPlayerEnteredRoom;
            NetworkEvents.current.OnPlayerLeft += OnPlayerLeftRoom;

            this.template.gameObject.SetActive(false);

            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                if (p.TagObject == null || p == PhotonNetwork.LocalPlayer) continue;
                Player player = (Player)p.TagObject;
                if (player == null) continue;
                int ID = player.photonView.ViewID;
                Create(ID);
            }
        }

        private void OnDestroy()
        {
            NetworkEvents.current.OnPlayerSpawned -= OnPlayerEnteredRoom;
            NetworkEvents.current.OnPlayerLeft -= OnPlayerLeftRoom;
        }

        private void AddNewInfos(int ID)
        {
            foreach (PlayerListUIChild child in this.children)
            {
                if (child.ID == ID) return;
            }

            Create(ID);
        }

        private void RemoveInfos(int ID)
        {
            foreach (PlayerListUIChild child in this.children)
            {
                if (child.ID == ID)
                {
                    Destroy(child.gameObject);
                }
            }

            this.children.RemoveAll(x => x == null);
        }

        private void Create(int ID)
        {
            PlayerListUIChild newChild = Instantiate(this.template, this.content);
            newChild.gameObject.SetActive(true);
            newChild.SetChild(ID);
            this.children.Add(newChild);
        }

        private void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Player p = (Player)otherPlayer.TagObject;
            RemoveInfos(p.photonView.ViewID);
        }

        private void OnPlayerEnteredRoom(PhotonView view)
        {            
            AddNewInfos(view.ViewID);
        }
    }
}