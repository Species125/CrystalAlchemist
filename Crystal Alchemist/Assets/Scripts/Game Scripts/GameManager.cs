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

        private void Start()
        {
            GameEvents.current.OnSceneChanged += ChangeScene;
            GameEvents.current.OnDeath += CheckDeathOfPlayers;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnSceneChanged -= ChangeScene;
            GameEvents.current.OnDeath -= CheckDeathOfPlayers;
            this.timePlayed.SetValue(this.timePlayed.GetValue() + Time.timeSinceLevelLoad);
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
