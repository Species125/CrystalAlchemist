using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SceneTransition : MonoBehaviour
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

        public void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (!NetworkUtil.IsMaster() || !NetworkUtil.IsLocal(player)) return;

            if (!other.isTrigger)
            {
                if (this.dialogBox == null) transferToScene();
                else this.dialogBox.ShowDialogBox();
            }
        }

        public void transferToScene()
        { 
            if (PhotonNetwork.OfflineMode || PhotonNetwork.PlayerList.Length <= 1)
            {                
                this.teleportList.SetNextTeleport(this.stats);
                GameEvents.current.DoTeleport();  
            }
            else ShowReadywindow(this.stats);
        }

        public void ShowReadywindow(TeleportStats stats)
        {
            object[] datas = new object[] { stats.path };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.READY_SHOW, datas, options, SendOptions.SendUnreliable);
        }
    }
}

