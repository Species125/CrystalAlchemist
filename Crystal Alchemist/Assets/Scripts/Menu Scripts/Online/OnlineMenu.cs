using Photon.Pun;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using TMPro;

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
        private GameObject leave;

        public override void Start()
        {
            base.Start();

            joinOrCreate.SetActive(false);
            leave.SetActive(false);

            if (PhotonNetwork.OfflineMode) joinOrCreate.SetActive(true);
            else leave.SetActive(true);
        }

        public void JoinOrCreateGroup()
        {
            settings.offlineMode = false;
            ExitMenu();
            SceneManager.LoadScene("Loading");
        }

        public void LeaveGroup()
        {
            settings.offlineMode = true;
            PhotonNetwork.Disconnect();
            ExitMenu();
            GameEvents.current.DoChangeScene(this.settings.currentScene.GetValue()); //go back to last scene
        }        
    }
}
