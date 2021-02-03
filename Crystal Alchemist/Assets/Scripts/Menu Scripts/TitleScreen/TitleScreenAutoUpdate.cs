using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class TitleScreenAutoUpdate : MonoBehaviour
    {
        private UnityWebRequest www;
        private bool isDownloading = false;
        private string current;

        [SerializeField]
        private UnityEvent OnNewVersion;

        [SerializeField]
        private UnityEvent OnDownloadCompleted;

        [SerializeField]
        private UnityEvent NoNewVersion;

        [SerializeField]
        private Image bar;

        private string updaterPath;

        private void Start() => Invoke("StartUp", 1f);

        private void StartUp()
        {
            this.current = Directory.GetCurrentDirectory();
            updaterPath = Path.Combine(current, "CrystalAlchemistUpdater.exe");

            if (File.Exists(updaterPath)) StartCoroutine(Check());
            else NoNewVersion?.Invoke();
        }

        private IEnumerator Check()
        {
            WWW w = new WWW("http://www.gungnir-arts.com/Download/Version.txt");
            yield return w;

            bool update = false;

            if (w.error == null)
            {
                string version = w.text;
                string text = version.Replace(".", "");

                if (text.Length > 1)
                {
                    int onlineVersion = Convert.ToInt32(text);
                    int currentVersion = Convert.ToInt32(Application.version.Replace(".", ""));

                    if (onlineVersion > currentVersion) update = true;
                }
            }

            if (update) this.OnNewVersion?.Invoke();
            else this.NoNewVersion?.Invoke();
        }

        public void Download() => StartCoroutine(DownloadFile());

        public void UpdateGame()
        {
            if (File.Exists(updaterPath))
            {
                Process.Start(updaterPath);
                Application.Quit();
            }
            else NoNewVersion?.Invoke();
        }

        private void ShowProgress() => bar.fillAmount = this.www.downloadProgress;

        IEnumerator DownloadFile()
        {
            this.www = new UnityWebRequest("http://www.gungnir-arts.com/Download/CrystalAlchemist.zip");
            string path = Path.Combine(current, "CrystalAlchemist.zip");
            www.downloadHandler = new DownloadHandlerFile(path);

            InvokeRepeating("ShowProgress", 0, 0.3f);
            yield return www.SendWebRequest();
            CancelInvoke("ShowProgress");

            if (www.isNetworkError || www.isHttpError) UnityEngine.Debug.Log(www.error);
            else this.OnDownloadCompleted?.Invoke();
        }
    }
}
