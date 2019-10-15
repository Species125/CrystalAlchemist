﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;
using Sirenix.OdinInspector;

#region Enums
public enum UIType
{
    resource,           
    buffsOnly,          //Nur Buffs
    debuffsOnly,        //Nur Debuffs
    BuffsAndDebuffs,    //Buffs und Debuffs
    DebuffsAndBuffs     //Debuffs und Buffs
}
#endregion

public class StatusBar : MonoBehaviour
{
    #region Attribute
    [SerializeField]
    [BoxGroup("Mandatory")]
    private PlayerStats playerStats;

    [BoxGroup("UI Typ")]
    public UIType UIType = UIType.resource;

    [BoxGroup("UI Typ")]
    [ShowIf("UIType", UIType.resource)]
    public ResourceType resourceType;

    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    public Sprite full;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    public Sprite quarterhalf;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    public Sprite half;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    public Sprite quarter;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    public Sprite empty;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    public GameObject icon;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    public GameObject warning;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private StatusEffectBar statusEffectBar;

    [FoldoutGroup("Sound", expanded: false)]
    public AudioClip lowSoundEffect;
    [FoldoutGroup("Sound", expanded: false)]
    public float audioInterval = 1.5f;



    private AudioSource audioSource;
    private float maximum;
    private float current;
    private bool playLow;
    private float elapsed;
    private Player player;
    #endregion


    #region Start Funktionen (Awake, Init, SetValues)

    void Start()
    {
        init();
    }

    private void init()
    {
        this.player = this.playerStats.player;
        this.audioSource = GetComponent<AudioSource>();
        setValues();
        setStatusBar();

        if(this.UIType == UIType.resource) UpdateGUIHealthMana();
    }

    private void LateUpdate()
    {
        if (this.playLow && this.player.currentState != CharacterState.dead)
        {
            if (elapsed <= 0)
            {
                elapsed = audioInterval;
                Utilities.Audio.playSoundEffect(this.audioSource, this.lowSoundEffect);
            }
            else
            {
                elapsed -= Time.deltaTime;
            }
        }
    }

    private void setValues()
    {
        if (this.player != null)
        {
            this.maximum = this.player.getMaxResource(this.resourceType, null);
            this.current = this.player.getResource(this.resourceType, null);
        }
    }

    private void setStatusBar()
    {
        for (int i = 0; i < (int)this.maximum - 1; i++)
        {
            GameObject temp = Instantiate(icon, this.gameObject.transform);
            //temp.hideFlags = HideFlags.HideInHierarchy;
        }
    }
    #endregion


    #region Update Signal Funktionen (Life, Mana, StatusEffects)    
    public void UpdateGUIHealthMana()
    {
        setValues();

        Sprite sprite = null;

        for (int i = 0; i < (int)this.maximum; i++)
        {
            if (i <= this.current - 1)
            {                
                sprite = this.full;
            }
            else if (i <= this.current - 0.75f)
            {
                sprite = this.quarterhalf;
            }
            else if (i <= this.current - 0.5f)
            {
                sprite = this.half;
            }
            else if (i <= this.current - 0.25f)
            {
                sprite = this.quarter;
            }
            else 
            {
                sprite = this.empty;
            }

            try
            {
                Transform temp = this.gameObject.transform.GetChild(i);
                temp.GetComponent<Image>().sprite = sprite;
            }
            catch(Exception ex)
            {
                string temp = ex.ToString();
            }
            
        }

        if (this.current <= 0.5f && this.player.currentState != CharacterState.dead)
        {
            this.playLow = true;
            if (this.warning != null) this.warning.SetActive(true);
        }
        else 
        {
            this.playLow = false;
            if (this.warning != null) this.warning.SetActive(false);
        }

    }
    #endregion
}
