using Photon.Pun;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class OnlineMenu : MenuBehaviour
    {
        private enum OnlineState
        {
            goOnline,
            leave
        }

        [BoxGroup("Online Menu")]
        [SerializeField]
        private NetworkSettings settings;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private Selectable edit;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private Selectable leave;

        private OnlineState state;

        public override void Start()
        {
            base.Start();

            this.edit.interactable = !PhotonNetwork.OfflineMode;
            this.leave.interactable = !PhotonNetwork.OfflineMode;
        }

        public void JoinOrCreateGroup()
        {
            settings.offlineMode.SetValue(false);
            this.state = OnlineState.goOnline;
            PhotonNetwork.LeaveRoom();                                  
        }

        public override void OnLeftRoom()
        {
            if (this.state == OnlineState.goOnline)
            {                
                NetworkUtil.LoadConnect();
            }
            ExitMenu();
        }

        public void LeaveGroup()
        {
            settings.offlineMode.SetValue(true);
            this.state = OnlineState.leave;            
            ExitMenu();
            GameEvents.current.DoChangeScene(this.settings.currentScene.GetValue()); //go back to last scene            
        }        
    }
}
