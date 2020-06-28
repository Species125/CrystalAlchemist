﻿using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class DeathScreen : MonoBehaviour
{
    [BoxGroup("Mandatory")]
    [SerializeField]
    private PlayerTeleportList playerTeleport;

    [BoxGroup("Music")]
    [SerializeField]
    private AudioClip deathMusic;

    [BoxGroup("Music")]
    [SerializeField]
    private float fadeIn;

    [BoxGroup("Music")]
    [SerializeField]
    private float fadeOut = 1f;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private CustomCursor cursor;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private TextMeshProUGUI textField;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private TextMeshProUGUI countDown;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject returnTitleScreen;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject returnSavePoint;

    [BoxGroup("Time")]
    [SerializeField]
    private float timer = 30;

    [BoxGroup("Time")]
    [SerializeField]
    private float textDelay = 0.1f;

    [BoxGroup("Time")]
    [SerializeField]
    private float cursorDelay = 2f;

    [BoxGroup("Time")]
    [SerializeField]
    private float inputDelay = 3f;

    private string currentText;
    private string fullText;
    private bool skip = false;
    private bool inputPossible = false;
    private bool startCountdown = false;

    private void Start()
    {
        Invoke("ReturnToTitleScreen", 60);
        this.cursor.gameObject.SetActive(false);
        this.returnSavePoint.SetActive(false);
        this.returnTitleScreen.SetActive(false);
        this.countDown.gameObject.SetActive(false);
        this.textField.gameObject.SetActive(false);

        SceneManager.UnloadSceneAsync("UI");
        MusicEvents.current.StopMusic(this.fadeOut);
        MenuEvents.current.DoPostProcessingFade(ShowText);
    }

    private void Update()
    {
        if (Input.anyKeyDown && this.inputPossible) this.skip = true;
        if (this.startCountdown)
        {
            if (this.timer <= 0) ReturnToTitleScreen();
            else this.timer -= Time.deltaTime;
            this.countDown.text = (int)this.timer + "s";
        }
    }

    private void ShowText()
    {
        this.inputPossible = true;
        MusicEvents.current.PlayMusicOnce(this.deathMusic, 0, this.fadeIn);
        this.textField.gameObject.SetActive(true);
        this.fullText = this.textField.text;
        StartCoroutine(this.ShowTextCo(this.textDelay));
    }

    public void ShowButtons()
    {
        this.cursor.gameObject.SetActive(true);
        this.returnTitleScreen.SetActive(true);
        this.returnSavePoint.SetActive(true);

        this.countDown.gameObject.SetActive(true);
        startCountdown = true;
    }

    public void ReturnToTitleScreen() => SceneManager.LoadSceneAsync(0);

    public void ReturnSaveGame()
    {
        this.playerTeleport.SetReturnTeleport();
        GameEvents.current.DoTeleport();
    }

    private IEnumerator ShowTextCo(float delay)
    {
        for (int i = 0; i <= this.fullText.Length; i++)
        {
            this.currentText = this.fullText.Substring(0, i);

            if (skip)
            {
                this.currentText = this.fullText;
                i = this.fullText.Length;
            }

            this.textField.text = this.currentText;

            if (i >= this.fullText.Length)
            {
                StartCoroutine(showButtonCo(this.cursorDelay));
                break;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator showButtonCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowButtons();
    }
}
