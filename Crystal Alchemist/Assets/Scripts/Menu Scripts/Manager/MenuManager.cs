using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class MenuManager : MonoBehaviour
    {
        [BoxGroup("Required")]
        [Required]
        [SerializeField]
        private StringValue teleportPath;

        private void Start()
        {
            if (!NetworkUtil.IsLocal()) return;

            MenuEvents.current.OnInventory += OpenInventory;
            MenuEvents.current.OnPause += OpenPause;
            MenuEvents.current.OnMap += OpenMap;
            MenuEvents.current.OnSkills += OpenSkillBook;
            MenuEvents.current.OnAttributes += OpenAttributes;
            MenuEvents.current.OnDeath += OpenDeath;
            MenuEvents.current.OnMiniGame += OpenMiniGame;
            MenuEvents.current.OnEditor += OpenCharacterEditor;
            MenuEvents.current.OnSave += OpenSavePoint;
            MenuEvents.current.OnDialogBox += OpenDialogBox;
            MenuEvents.current.OnMenuDialogBox += OpenMenuDialogBox; 
            MenuEvents.current.OnJukeBox += OpenJukebox;
            MenuEvents.current.OnOnlineMenu += OpenOnlineMenu;
            MenuEvents.current.OnTutorial += OpenTutorial;
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
        }

        private void OnDestroy()
        {
            if (!NetworkUtil.IsLocal()) return;

            MenuEvents.current.OnInventory -= OpenInventory;
            MenuEvents.current.OnPause -= OpenPause;
            MenuEvents.current.OnMap -= OpenMap;
            MenuEvents.current.OnSkills -= OpenSkillBook;
            MenuEvents.current.OnAttributes -= OpenAttributes;
            MenuEvents.current.OnDeath -= OpenDeath;
            MenuEvents.current.OnMiniGame -= OpenMiniGame;
            MenuEvents.current.OnEditor -= OpenCharacterEditor;
            MenuEvents.current.OnSave -= OpenSavePoint;
            MenuEvents.current.OnDialogBox -= OpenDialogBox;
            MenuEvents.current.OnMenuDialogBox -= OpenMenuDialogBox;
            MenuEvents.current.OnJukeBox -= OpenJukebox;
            MenuEvents.current.OnOnlineMenu -= OpenOnlineMenu;
            MenuEvents.current.OnTutorial -= OpenTutorial;
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
        }

        public void OpenInventory() => OpenScene("InventoryMenu");

        public void OpenPause() => OpenScene("Pause");

        public void OpenMap() => OpenScene("Map");

        public void OpenSkillBook() => OpenScene("Skillbook");

        public void OpenAttributes() => OpenScene("Attributes");

        public void OpenCharacterEditor() => OpenScene("Character Creation");

        public void OpenSavePoint() => OpenScene("Quicktravel");

        public void OpenDeath() => OpenSceneOnce("Death Screen");

        public void OpenMiniGame() => OpenScene("Minigame");

        public void OpenDialogBox() => OpenScene("DialogBox");

        public void OpenMenuDialogBox() => OpenSceneAdditive("MenuDialogBox");

        public void OpenJukebox() => OpenScene("Jukebox");

        public void OpenTutorial() => OpenScene("Tutorial");

        public void OpenOnlineMenu() => OpenScene("Online Menu");

        private void OpenSceneOnce(string scene)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }

        private void OpenScene(string scene)
        {
            if (UnityUtil.SceneExists(scene)) SceneManager.UnloadSceneAsync(scene);
            else SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }

        private void OpenSceneAdditive(string scene)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }

        private void NetworkingEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.SET_NEXT_TELEPORT)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];

                this.teleportPath.SetValue(path);                
            }
            else if (obj.Code == NetworkUtil.READY_SHOW)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];

                this.teleportPath.SetValue(path);

                OpenScene("Teleport");
            }            
        }
    }
}
