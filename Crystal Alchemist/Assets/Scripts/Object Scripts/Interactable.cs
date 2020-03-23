﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

public enum objectState
{
    normal,
    opened,
    showItem
}

public class Interactable : MonoBehaviour
{
    #region Attribute
    [Required]
    [BoxGroup("Mandatory")]
    [Tooltip("Context-Objekt hier rein (nur für Interagierbare Objekte)")]
    public GameObject contextClueChild;

    [BoxGroup("Activation Requirements")]
    [HideLabel]
    public Costs costs;

    [BoxGroup("Sound")]
    [Tooltip("Standard-Soundeffekt")]
    public AudioClip soundEffect;

    [HideInInspector]
    public bool isPlayerInRange = false;
    [HideInInspector]
    public bool isPlayerLookingAtIt = false;

    [HideInInspector]
    public Player player; 

    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public GameObject context;

    [HideInInspector]
    public objectState currentState = objectState.normal;



    #endregion


    #region Start Funktionen (init, ContextClue, Item set bzw. Lootregeln)

    public virtual void Start()
    {
        init();
    }

    private void Update()
    {
        if (this.player != null
            && this.isPlayerInRange
            && this.isPlayerLookingAtIt
            && this.player.currentState == CharacterState.interact)
        {
            if (Input.GetButtonDown("Submit"))
            {
                doSomethingOnSubmit();
            }
        }

        doOnUpdate();
    }

    public virtual void doOnUpdate()
    {

    }

    public virtual void doSomethingOnSubmit()
    {

    }

    private void init()
    {
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
        setContext();
       
    }

    public void setContext()
    {
        if (this.contextClueChild != null)
        {
            this.context = Instantiate(this.contextClueChild, this.transform.position, Quaternion.identity, this.transform);
        }
    }

    #endregion

    #region Context Clue Funktionen

    private void interact(Collider2D characterCollisionBox)
    {
        if (characterCollisionBox.CompareTag("Player") && !characterCollisionBox.isTrigger)
        {
            Player player = characterCollisionBox.GetComponent<Player>();

            if (player != null)
            {
                if(this.player != player) this.player = player;
                this.isPlayerInRange = true;

                checkifLooking(this.player);
            }
        }
    }

    private void checkifLooking(Character character)
    {
        if (character != null
            && (character.currentState == CharacterState.interact 
             || character.currentState == CharacterState.idle
             || character.currentState == CharacterState.walk))
        {
            if (this.isPlayerInRange
                && CollisionUtil.checkIfGameObjectIsViewed(character, this.gameObject))
            {
                player.currentState = CharacterState.interact;
                this.context.SetActive(true);                
                this.isPlayerLookingAtIt = true;
            }
            else
            {
                player.currentState = CharacterState.idle;
                this.context.SetActive(false);                
                this.isPlayerLookingAtIt = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D characterCollisionBox)
    {
        interact(characterCollisionBox);
    }

    private void OnTriggerEnter2D(Collider2D characterCollisionBox)
    {
        interact(characterCollisionBox);
    }

    private void OnTriggerExit2D(Collider2D characterCollisionBox)
    {
        if (characterCollisionBox.CompareTag("Player") && !characterCollisionBox.isTrigger)
        {
            Player player = characterCollisionBox.GetComponent<Player>();

            if (player != null && player.currentState == CharacterState.interact)
            {
                player.currentState = CharacterState.idle;
                if (this.player == player) this.player = null;
            }

            this.isPlayerInRange = false;
            this.isPlayerLookingAtIt = false;
            this.context.SetActive(false);
        }
    }
    #endregion
}
