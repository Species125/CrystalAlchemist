using Photon.Pun;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class OnlineMenu : MenuBehaviour
    {
        [BoxGroup("Online Menu")]
        [SerializeField]
        private NetworkSettings settings;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private Selectable edit;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private Selectable leave;

        public override void Start()
        {
            base.Start();

            this.edit.interactable = !PhotonNetwork.OfflineMode;
            this.leave.interactable = !PhotonNetwork.OfflineMode;
        }

        public void JoinOrCreateGroup()
        {
            settings.offlineMode.SetValue(false);
            PhotonNetwork.LeaveRoom();            
            ExitMenu();
            SceneManager.LoadScene("Loading");
        }

        public void LeaveGroup()
        {
            settings.offlineMode.SetValue(true);
            PhotonNetwork.Disconnect();
            ExitMenu();
            GameEvents.current.DoChangeScene(this.settings.currentScene.GetValue()); //go back to last scene            
        }        
    }
}
