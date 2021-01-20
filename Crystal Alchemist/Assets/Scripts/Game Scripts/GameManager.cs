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
            Scene scene = SceneManager.GetActiveScene();
            if (
                //scene.name != newScene && 
                NetworkUtil.IsMaster())
            {
                //Only Master is allowed to load level
                //PhotonNetwork.CurrentRoom.IsOpen = false; //no join when room is open
                //PhotonNetwork.CurrentRoom.IsVisible = false; //not visible when room is there
                PhotonNetwork.LoadLevel(newScene);
            }

            //StartCoroutine(loadSceneCo(newScene));
        }

        private IEnumerator loadSceneCo(string targetScene)
        {
            MenuEvents.current.DoFadeOut();
            yield return new WaitForSeconds(this.fadeDuration.GetValue());

            //GameEvents.current.DoOnlineChangeScene(targetScene);

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

            MenuEvents.current.OpenDeath();
        }
    }
}
