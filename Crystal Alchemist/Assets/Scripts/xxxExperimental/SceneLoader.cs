using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public static class SceneLoader
    {
        private class LoadingMonoBehaviour : MonoBehaviour { }

        private static Action onLoadedCallback;
        private static AsyncOperation asyncOperation;

        public static void Load(string scene)
        {
            onLoadedCallback = () =>
            {
                GameObject loadingGameObject = new GameObject("Loading Game Object");
                loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            };

            SceneManager.LoadScene("Loading");
        }

        private static IEnumerator LoadSceneAsync(string scene)
        {
            yield return null;
            asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            /*
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                //yield return new WaitForSeconds(this.player.fadingDuration.getValue());
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }*/
        }

        public static float GetLoadingProgress()
        {
            if (asyncOperation != null) return asyncOperation.progress;
            return 1f;
        }

        public static void LoaderCallback()
        {
            if (onLoadedCallback != null)
            {
                onLoadedCallback();
                onLoadedCallback = null;
            }
        }
    }
}
