using Photon.Pun;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class OnlineMenu : MenuBehaviour
    {
        [BoxGroup("Online Menu")]
        [SerializeField]
        private NetworkSettings settings;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private GameObject joinOrCreate;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private GameObject edit;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private GameObject leave;

        public override void Start()
        {
            base.Start();

            joinOrCreate.SetActive(false);
            leave.SetActive(false);

            if (PhotonNetwork.OfflineMode)
            {
                joinOrCreate.SetActive(true);
                edit.SetActive(false);
            }
            else
            {
                edit.SetActive(true);
                leave.SetActive(true);
            }
        }

        public void JoinOrCreateGroup()
        {
            settings.offlineMode.SetValue(false);
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
