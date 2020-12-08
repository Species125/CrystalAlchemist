using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private FloatValue timePlayed;

    [SerializeField]
    private FloatValue fadeDuration;

    private void Start()
    {
        GameEvents.current.OnSceneChanged += ChangeScene;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnSceneChanged -= ChangeScene;
        this.timePlayed.SetValue(this.timePlayed.GetValue()+Time.timeSinceLevelLoad);
    }

    private void ChangeScene(string newScene)
    {
        StartCoroutine(loadSceneCo(newScene));        
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
}
