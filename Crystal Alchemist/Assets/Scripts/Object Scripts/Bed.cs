﻿using Sirenix.OdinInspector;
using UnityEngine;

public class Bed : Interactable
{
    public enum BedState
    {
        awake,
        sleeping,
        wakingup
    }

    [BoxGroup("Bett")]
    [SerializeField]
    private TimeValue time;

    [BoxGroup("Bett")]
    [SerializeField]
    private float newValue;

    [BoxGroup("Bett")]
    [SerializeField]
    private float offset = 0.35f;

    [BoxGroup("Bett")]
    [SerializeField]
    private GameObject blanket;

    [BoxGroup("Bett")]
    [SerializeField]
    private string wakeUpActionID;

    [BoxGroup("Bett")]
    [SerializeField]
    private AudioClip music;

    private Vector2 position;
    private bool isSleeping;
    private string oldID;

    public override void Start()
    {
        base.Start();
        this.blanket.SetActive(false);
        this.oldID = this.translationID;
    }

    public override void DoOnSubmit()
    {
        if (!this.isSleeping)
        {
            this.position = this.player.transform.position;
            Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y + offset);

            GameEvents.current.DoSleep(position, () => this.blanket.SetActive(true), () => StartSleeping()); 
            this.translationID = this.wakeUpActionID;

            MusicEvents.current.StopMusic();
            MusicEvents.current.PlayMusic(this.music, false);
        }
        else
        {
            GameEvents.current.DoWakeUp(this.position, () => this.time.Reset(), () => PlayerAwake());
            this.translationID = this.oldID;
        }
    }

    private void StartSleeping()
    {
        this.time.SetFactor(this.newValue);
        this.isSleeping = true;
    }

    private void PlayerAwake()
    {
        this.blanket.SetActive(false);
        this.isSleeping = false;
        MusicEvents.current.RestartMusic();
    }
}
