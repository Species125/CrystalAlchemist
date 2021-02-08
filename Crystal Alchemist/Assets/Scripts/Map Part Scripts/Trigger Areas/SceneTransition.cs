using Sirenix.OdinInspector;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace CrystalAlchemist
{
    public class SceneTransition : NetworkBehaviour
    {
        [BoxGroup("Required")]
        [Required]
        [SerializeField]
        private TeleportStats stats;

        [BoxGroup("Required")]
        [SerializeField]
        private MenuDialogBoxLauncher dialogBox;

        [BoxGroup("Required")]
        [Required]
        [SerializeField]
        private PlayerTeleportList teleportList;

        [BoxGroup("Required")]
        [SerializeField]
        private bool useReadyWindow = false;

        private PhotonView photon;
        private int players;
        private int playerCount;

        private void Start()
        {
            this.photon = this.GetComponent<PhotonView>();
            if (PhotonNetwork.InRoom) this.playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            this.playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            this.playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (other.isTrigger || !NetworkUtil.IsLocal(player)) return;

            if (this.dialogBox == null) TransferToScene();
            else this.dialogBox.ShowDialogBox();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (other.isTrigger || !NetworkUtil.IsLocal(player)) return;

            SetPlayer(-1);
        }        

        public void TransferToScene()
        {
            if(!this.useReadyWindow) SetPlayer(1);
            else NetworkEvents.current.ShowReadywindow(this.stats);            
        }

        private void SetPlayer(int value)
        {
            if (!PhotonNetwork.IsConnected) return;
            this.photon.RPC("RpcSetPlayer", RpcTarget.All, value);
        }

        [PunRPC]
        protected void RpcSetPlayer(int value)
        {
            this.players += value;

            if (this.players < this.playerCount) return;

            this.teleportList.SetNextTeleport(this.stats);
            GameEvents.current.DoTeleport();
        }

        public void SetTeleport(TeleportStats stats)
        {
            this.stats = stats;
            this.teleportList.SetAnimation(true, true);
        }

        /*
        private void ShowPoints()
        {
            for (int i = 1; i <= this.points.Count; i++)
            {
                if (i <= this.players) this.points[i - 1].SetActive(true);
                else this.points[i - 1].SetActive(false);
            }
        }*/
    }
}

