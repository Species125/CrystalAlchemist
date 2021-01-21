using Photon.Pun;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class OnlineMenu : MenuBehaviour
    {
        [BoxGroup("Online Menu")]
        [SerializeField]
        private GameObject joinControls;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private GameObject leaveControls;

        [BoxGroup("Online Menu")]
        [SerializeField]
        private NetworkSettings settings;

        string sceneName;
        //Room name
        //scene Name
        //is online or offline

        //Are you sure to create/leave/join?
        //Cannot join because

        public override void Start()
        {
            base.Start();

            /*
            if (PhotonNetwork.OfflineMode == false && PhotonNetwork.InRoom)
            {
                this.joinControls.SetActive(false);
                this.leaveControls.SetActive(true);
            } 
            else
            {
                this.joinControls.SetActive(true);
                this.leaveControls.SetActive(false);
            }*/
        }

        public void CreateGroup()
        {
            settings.SetOfflineStatus(false);
            ExitMenu();
            GameEvents.current.DoChangeScene(this.settings.currentScene.GetValue()); //save current scene
        }

        public void LeaveGroup()
        {
            settings.SetOfflineStatus(true);
            ExitMenu();
            GameEvents.current.DoChangeScene(this.settings.currentScene.GetValue()); //save current scene
        }        
    }
}
