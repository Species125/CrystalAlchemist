﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;

    private Player player;

    [SerializeField]
    private GameObject cursor;

    [SerializeField]
    private GameObject blackScreen;

    [SerializeField]
    private GameObject parentMenue;

    [SerializeField]
    private GameObject childMenue;

    [SerializeField]
    private FloatSignal musicVolumeSignal;

    private CharacterState lastState;
    private bool overrideState = true;

    private void Awake()
    {
        this.player = this.playerStats.player;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause")) exitMenu();
    }

    private void OnEnable()
    {        
        this.cursor.SetActive(true);

        this.parentMenue.SetActive(true);
        this.childMenue.SetActive(false);

        if (this.overrideState)
        {
            this.lastState = this.player.currentState;
            this.overrideState = false;
        }
        this.player.currentState = CharacterState.inMenu;

        this.musicVolumeSignal.Raise(GlobalValues.getMusicInMenu());
    }

    private void OnDisable()
    {
        this.cursor.SetActive(false);

        this.musicVolumeSignal.Raise(GlobalValues.backgroundMusicVolume);
    }

    public void exitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void exitMenu()
    {
        this.overrideState = true;
        this.player.delay(this.lastState);
        this.transform.parent.gameObject.SetActive(false);
        this.blackScreen.SetActive(false);
    }

    public void showControls()
    {
        this.parentMenue.SetActive(false);
        this.childMenue.SetActive(true);
    }

   
}
