﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objectState
{
    normal,
    opened
}

public enum Currency
{
    none,
    crystal,
    coin,
    key
}

public class Interactable : MonoBehaviour
{
    #region Attribute

    [Tooltip("Anzeige-Text für die Dialog-Box")]
    [TextAreaAttribute]
    public string text;

    [Tooltip("Context-Objekt hier rein (nur für Interagierbare Objekte)")]
    public GameObject contextClueChild;
    [Header("Loot")]
    [Tooltip("Items und deren Wahrscheinlichkeit zwischen 1 und 100")]
    public LootTable[] lootTable;
    [Tooltip("Multiloot = alle Items. Ansonsten nur das seltenste Item")]
    public bool multiLoot = false;
    [Tooltip("Was benötigt wird um zu öffnen")]
    public Currency currencyNeeded = Currency.none;
    [Range(0,Utilities.maxIntInfinite)]
    public int price = 0;

    [Header("Sound Effects")]
    [Tooltip("Standard-Soundeffekt")]
    public AudioClip soundEffect;

    [HideInInspector]
    public bool isPlayerInRange;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public AudioSource audioSource;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    [HideInInspector]
    public GameObject context;

    [HideInInspector]    
    public List<Item> items = new List<Item>();

    public objectState currentState = objectState.normal;

    #endregion


    #region Start Funktionen (init, ContextClue, Item set bzw. Lootregeln)

    private void Start()
    {
        init();
    }

    public void init()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.spriteRenderer != null) this.spriteRenderer.color = GlobalValues.color;

            this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
            this.audioSource.loop = false;
            this.audioSource.playOnAwake = false;
            this.animator = GetComponent<Animator>();            
            setContext();
            Utilities.setItem(this.lootTable, this.multiLoot, this.items);        
    }    

    public void setContext()
    {
        if(this.contextClueChild != null)
        {
            this.context = Instantiate(this.contextClueChild, this.transform.position, Quaternion.identity, this.transform);            
        }
    }

    #endregion


    public void updateColor()
    {
        if (this.spriteRenderer != null) this.spriteRenderer.color = GlobalValues.color;
    }


    #region Context Clue Funktionen

    private void OnTriggerEnter2D(Collider2D characterCollisionBox)
    {
        //Context Clue einblenden und Charakter nicht mehr angreifen lassen!

        if (characterCollisionBox.CompareTag("Player") && !characterCollisionBox.isTrigger)
        {            
            this.player = characterCollisionBox.gameObject.GetComponent<Player>();
            if(this.player != null) this.player.currentState = CharacterState.interact;
            this.isPlayerInRange = true;
            this.context.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D characterCollisionBox)
    {
        //Context Clue ausblenden und Charakter wieder normal agieren lassen 

        if (characterCollisionBox.CompareTag("Player") && !characterCollisionBox.isTrigger)
        {
            if (this.player != null) this.player.currentState = CharacterState.idle;
            this.player = null;            
            this.isPlayerInRange = false;
            this.context.SetActive(false);
        }
    }
    #endregion
}
