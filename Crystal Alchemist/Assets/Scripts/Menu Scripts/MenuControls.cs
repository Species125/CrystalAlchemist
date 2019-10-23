﻿using UnityEngine;
using Sirenix.OdinInspector;

public class MenuControls : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    [SerializeField]
    private PlayerStats playerStats;

    [BoxGroup("Signals")]
    [SerializeField]
    private FloatSignal musicVolumeSignal;

    [BoxGroup("Mandatory")]
    [Required]
    public myCursor cursor;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private GameObject blackScreen;

    [HideInInspector]
    public CharacterState lastState;

    public virtual void OnEnable()
    {
        this.player = this.playerStats.player;

        this.lastState = this.player.currentState;
        this.cursor.gameObject.SetActive(true);
        this.player.currentState = CharacterState.inMenu;

        this.musicVolumeSignal.Raise(GlobalValues.getMusicInMenu());
    }

    public virtual void OnDisable()
    {
        this.cursor.gameObject.SetActive(false);

        this.musicVolumeSignal.Raise(GlobalValues.backgroundMusicVolume);
    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (this.cursor.infoBox.gameObject.activeInHierarchy) this.cursor.infoBox.Hide();
            else exitMenu();
        }
        else if (Input.GetButtonDown("Inventory")) exitMenu();
    }

    public void exitMenu()
    {
        this.cursor.infoBox.Hide();
        this.player.delay(this.lastState);
        this.transform.parent.gameObject.SetActive(false);
        this.blackScreen.SetActive(false);
    }
}
