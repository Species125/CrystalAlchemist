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

        private void Start()
        {
            GameEvents.current.OnKeyItem += HasKeyItemAlready; 
            GameEvents.current.OnSceneChanged += ChangeScene;
            GameEvents.current.OnDeath += CheckDeathOfPlayers;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnKeyItem -= HasKeyItemAlready; 
            GameEvents.current.OnSceneChanged -= ChangeScene;
            GameEvents.current.OnDeath -= CheckDeathOfPlayers;
            this.timePlayed.SetValue(this.timePlayed.GetValue() + Time.timeSinceLevelLoad);
        }

        public bool HasKeyItemAlready(string name)
        {
            if (this.inventory == null) return false;
            foreach (ItemStats elem in this.inventory.keyItems) if (elem != null && name == elem.name) return true;
            return false;
        }

        private void ChangeScene(string newScene)
        {
            if (NetworkUtil.IsMaster()) //Master in online and offline mode
            {
                this.nextScene.SetValue(newScene);
                PhotonNetwork.LoadLevel("Loading");
            }
            else if (!PhotonNetwork.IsConnected) //Not connected and also no master (startup)
            {
                this.nextScene.SetValue(newScene);
                //StartCoroutine(loadSceneCo("Loading"));
                SceneManager.LoadScene("Loading");
            }
        }

        private IEnumerator loadSceneCo(string targetScene)
        {
            if (MenuEvents.current) MenuEvents.current.DoFadeOut();
            yield return new WaitForSeconds(this.fadeDuration.GetValue());

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f) asyncOperation.allowSceneActivation = true;
                yield return null;
            }
        }

        private void CheckDeathOfPlayers()
        {
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                Player player = (Player)p.TagObject;
                if (player.values.currentState != CharacterState.dead) return;
            }

            if (MenuEvents.current) MenuEvents.current.OpenDeath();
        }
    }
}
