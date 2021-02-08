using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private FloatValue timePlayed;

        [SerializeField]
        private FloatValue fadeDuration;

        [SerializeField]
        private StringValue nextScene;

        [SerializeField]
        private PlayerInventory inventory;

        private void Awake()
        {
            MasterManager.globalValues.openedMenues.Clear();
        }

        private void Start()
        {
            GameEvents.current.OnKeyItem += HasKeyItemAlready; 
            GameEvents.current.OnSceneChanged += ChangeScene;
            GameEvents.current.OnTitleScreen += ReturnTitleScreen;
            GameEvents.current.OnDeath += CheckDeathOfPlayers;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnKeyItem -= HasKeyItemAlready; 
            GameEvents.current.OnSceneChanged -= ChangeScene;
            GameEvents.current.OnTitleScreen -= ReturnTitleScreen;
            GameEvents.current.OnDeath -= CheckDeathOfPlayers;
            this.timePlayed.SetValue(this.timePlayed.GetValue() + Time.timeSinceLevelLoad);
        }

        public bool HasKeyItemAlready(string name)
        {
            if (this.inventory == null) return false;
            foreach (ItemStats elem in this.inventory.keyItems) if (elem != null && name == elem.name) return true;
            return false;
        }

        private void ReturnTitleScreen()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0);
        }

        private void ChangeScene(string newScene)
        {
            StartCoroutine(loadSceneCo(newScene));
        }

        private IEnumerator loadSceneCo(string targetScene)
        {
            if (MenuEvents.current) MenuEvents.current.DoFadeOut();
            yield return new WaitForSeconds(this.fadeDuration.GetValue());

            if (NetworkUtil.IsMaster()) //Master in online and offline mode
            {
                this.nextScene.SetValue(targetScene);
                PhotonNetwork.LoadLevel("Loading");
            }
            else if (!PhotonNetwork.IsConnected) //Not connected and also no master (startup)
            {
                this.nextScene.SetValue(targetScene);                
                SceneManager.LoadScene("Loading");

                /*
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Loading");
                asyncOperation.allowSceneActivation = false;

                while (!asyncOperation.isDone)
                {
                    if (asyncOperation.progress >= 0.9f) asyncOperation.allowSceneActivation = true;
                    yield return null;
                }*/
            }
        }

        private void CheckDeathOfPlayers()
        {
            if (!NetworkUtil.IsMaster()) return;

            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                Player player = (Player)p.TagObject;
                if (player.values.currentState != CharacterState.dead) return;
            }

            NetworkEvents.current.RaiseDeathEvent();
        }
    }
}
