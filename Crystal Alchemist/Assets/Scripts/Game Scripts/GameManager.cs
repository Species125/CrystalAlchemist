using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private FloatValue timePlayed;

        [SerializeField]
        private FloatValue fadeDuration;

        [SerializeField]
        private StringValue currentScene;

        public static GameManager current;

        public bool loadingCompleted = false;

        private void Awake()
        {
            current = this;
            MasterManager.globalValues.openedMenues.Clear();

            GameEvents.current.OnSceneChanged += ChangeScene;
            GameEvents.current.OnDeath += CheckDeathOfPlayers;
        }

        private void Start() => Invoke("LateStart", 1f);

        private void LateStart() => loadingCompleted = true;

        private void OnDestroy()
        {
            GameEvents.current.OnSceneChanged -= ChangeScene;
            GameEvents.current.OnDeath -= CheckDeathOfPlayers;

            this.timePlayed.SetValue(this.timePlayed.GetValue() + Time.timeSinceLevelLoad);
        }

        private void ChangeScene(string newScene)
        {
            StartCoroutine(LoadSceneCo(newScene));
        }

        private IEnumerator LoadSceneCo(string targetScene)
        {
            if (MenuEvents.current) MenuEvents.current.DoFadeOut();
            yield return new WaitForSeconds(this.fadeDuration.GetValue());

            if (PhotonNetwork.IsConnected) //Master in online and offline mode
            {
                //this.nextScene.SetValue(targetScene);
                NetworkUtil.LoadLevel(targetScene);
            }
            else if (!PhotonNetwork.IsConnected) //Not connected and also no master (startup)
            {
                this.currentScene.SetValue(targetScene);
                NetworkUtil.LoadConnect();
            }
        }

        private IEnumerator LoadSceneCo2(string scene)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
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

            MenuEvents.current.OpenDeath();
        }
    }
}
